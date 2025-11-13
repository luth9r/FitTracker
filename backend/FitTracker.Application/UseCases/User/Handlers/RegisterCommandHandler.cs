using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers
{
    public class RegisterCommandHandler(IUserRepository userRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILocalizationService localization,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmailService emailService,
        IConfiguration configuration) : IRequestHandler<RegisterCommand, Result<RegisterResponse, ValidationResult>>
    {
        public async Task<Result<RegisterResponse, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userRequest = request.User;

            var existingUser = await userRepository.GetByUsernameReadonlyAsync(userRequest.Username, cancellationToken);
            if (existingUser != null)
            {
                var errorMessage = localization.GetString("Auth.Register.UsernameAlreadyExists");

                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(userRequest.Username), errorMessage)
                });
                return Result.Failure<RegisterResponse, ValidationResult>(errors);
            }

            var userBuilderResult = new UserEntity.UserBuilder()
                .WithUsername(userRequest.Username)
                .WithEmail(userRequest.Email)
                .WithPasswordHash(userRequest.Password)
                .Build();

            if (userBuilderResult.IsFailure)
            {
                var translatedErrors = userBuilderResult.Error.Errors
                    .Select(failure => new ValidationFailure(failure.PropertyName, localization.GetString(failure.ErrorMessage)))
                    .ToList();

                var translatedValidationResult = new ValidationResult(translatedErrors);

                return Result.Failure<RegisterResponse, ValidationResult>(translatedValidationResult);
            }

            var user = userBuilderResult.Value;

            // Add to trackings
            await userRepository.AddAsync(user, cancellationToken);

            // Save changes to db
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<RegisterResponse>(user);

            var verificationToken = jwtTokenGenerator.GenerateVerificationToken(user);

            var verificationLinkBase = configuration["App:VerificationLinkBase"];
            var verificationUrl = $"{verificationLinkBase}?token={verificationToken}";

            var emailBody = $"Здравствуйте, {user.Username}!<br>" +
                            $"Пожалуйста, подтвердите ваш email, перейдя по ссылке: <a href='{verificationUrl}'>Подтвердить</a><br>" +
                            $"Ссылка действительна 15 минут.";

            await emailService.SendEmailAsync(user.Email, "Подтверждение регистрации FitTracker", emailBody);

            return Result.Success<RegisterResponse, ValidationResult>(response);


        }
    }
}
