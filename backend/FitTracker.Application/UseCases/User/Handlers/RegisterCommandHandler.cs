using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers
{
    public class RegisterCommandHandler(IUserRepository userRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILocalizationService localization,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmailService emailService,
        IPasswordHasher hasher,
        IConfiguration configuration) : IRequestHandler<RegisterCommand, Result<RegisterResponse, ValidationResult>>
    {
        public async Task<Result<RegisterResponse, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userRequest = request.User;

            var existingUser = await userRepository.GetByUsernameReadonlyAsync(userRequest.Username, cancellationToken);
            if (existingUser != null)
            {
                return ResultExtensions.ValidationFailure<RegisterResponse>(nameof(userRequest.Username), localization.GetString("Auth.Register.UsernameAlreadyExists"));
            }

            var existingUserByEmail = await userRepository.GetByEmailReadonlyAsync(userRequest.Email, cancellationToken);
            if (existingUserByEmail != null)
            {
                return ResultExtensions.ValidationFailure<RegisterResponse>(nameof(userRequest.Email), localization.GetString("Auth.Register.EmailAlreadyExists"));
            }

            var userBuilderResult = new UserEntity.UserBuilder()
                .WithUsername(userRequest.Username)
                .WithEmail(userRequest.Email)
                .WithPasswordHash(hasher.HashPassword(request.User.Password))
                .Build();

            if (userBuilderResult.IsFailure)
            {
                var translatedErrors = userBuilderResult.Error.Errors
                    .Select(failure => new ValidationFailure(failure.PropertyName, localization.GetString(failure.ErrorMessage)))
                    .ToList();

                return ResultExtensions.ValidationFailure<RegisterResponse>(nameof(userBuilderResult), translatedErrors);
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

            var emailBody = $"Hello, {user.Username}!<br>" +
                $"Please confirm your email by clicking the link: <a href='{verificationUrl}'>Confirm</a><br>" +
                $"The link is valid for 15 minutes.";

            await emailService.SendEmailAsync(user.Email, "Registration confirmation FitTracker", emailBody);

            return Result.Success<RegisterResponse, ValidationResult>(response);


        }
    }
}
