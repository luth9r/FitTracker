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
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers
{
    /// <summary>
    /// Handler for processing user registration commands.
    /// </summary>
    /// <param name="userRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="localization"></param>
    /// <param name="jwtTokenGenerator"></param>
    /// <param name="emailService"></param>
    /// <param name="hasher"></param>
    /// <param name="configuration"></param>
    public class RegisterCommandHandler(IUserRepository userRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILocalizationService localization,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmailService emailService,
        IPasswordHasher hasher,
        IConfiguration configuration,
        ILogger<RegisterCommandHandler> logger) : IRequestHandler<RegisterCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting user registration process for username: {Username}", request.User.Username);

            var userRequest = request.User;

            var existingUser = await userRepository.GetByUsernameReadonlyAsync(userRequest.Username, cancellationToken);
            if (existingUser != null)
            {
                logger.LogWarning("Registration failed: Username {Username} already exists.", userRequest.Username);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(userRequest.Username), localization.GetString("Auth.Register.UsernameAlreadyExists"));
            }

            var existingUserByEmail = await userRepository.GetByEmailReadonlyAsync(userRequest.Email, cancellationToken);
            if (existingUserByEmail != null)
            {
                logger.LogWarning("Registration failed: Email {Email} already exists.", userRequest.Email);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(userRequest.Email), localization.GetString("Auth.Register.EmailAlreadyExists"));
            }

            var user = UserEntity.Create(
                username: userRequest.Username,
                email: userRequest.Email,
                passwordHash: hasher.HashPassword(userRequest.Password));

            // Add to trackings
            await userRepository.AddAsync(user, cancellationToken);

            // Save changes to db
            _ = await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<LoginResponse>(user);

            var verificationToken = jwtTokenGenerator.GenerateVerificationToken(user);

            var verificationLinkBase = configuration["App:VerificationLinkBase"];
            var verificationUrl = $"{verificationLinkBase}?token={verificationToken}";

            var emailBody = $"Hello, {user.Username}!<br>" +
                $"Please confirm your email by clicking the link: <a href='{verificationUrl}'>Confirm</a><br>" +
                $"The link is valid for 15 minutes.";

            await emailService.SendEmailAsync(user.Email, "Registration confirmation FitTracker", emailBody);

            logger.LogInformation("User registration process completed successfully for username: {Username}", request.User.Username);

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
