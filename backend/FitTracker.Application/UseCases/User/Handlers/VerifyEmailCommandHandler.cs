using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers
{
    /// <summary>
    /// Handler for processing email verification commands.
    /// </summary>
    /// <param name="jwtTokenGenerator">The <see cref="IJwtTokenGenerator"/>.</param>
    /// <param name="userRepository">The <see cref="IUserRepository"/>.</param>
    /// <param name="unitOfWork">The <see cref="IUnitOfWork"/>.</param>
    /// <param name="localization">The <see cref="ILocalizationService"/>.</param>
    /// <param name="mapper">The <see cref="IMapper"/>.</param>
    /// <param name="logger">The <see cref="ILogger{VerifyEmailCommandHandler}"/>.</param>
    public class VerifyEmailCommandHandler(IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILocalizationService localization,
        IMapper mapper,
        ILogger<VerifyEmailCommandHandler> logger) : IRequestHandler<VerifyEmailCommand, Result<LoginResponse, ValidationResult>>
    {
        /// <summary>
        /// Handles the verify email command.
        /// </summary>
        /// <param name="request">The <see cref="VerifyEmailCommand"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="LoginResponse"/> result.</returns>
        public async Task<Result<LoginResponse, ValidationResult>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting email verification process.");

            var claimsPrincipal = jwtTokenGenerator.ValidateToken(request.Token);

            if (claimsPrincipal == null)
            {
                logger.LogWarning("Email verification token validation failed.");
                return ResultExtensions.ValidationFailure<LoginResponse>(string.Empty, "Auth.VerifyEmail.InvalidToken");
            }

            var purposeClaim = claimsPrincipal.FindFirst("purpose");
            if (purposeClaim != null && purposeClaim.Value != "email_verification")
            {
                logger.LogWarning("Email verification token has wrong purpose: {Purpose}", purposeClaim.Value);
                return ResultExtensions.ValidationFailure<LoginResponse>(string.Empty, "Auth.VerifyEmail.WrongPurposeToken");
            }

            var userIdString = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdString == null || !Guid.TryParse(userIdString.Value, out var userId))
            {
                logger.LogWarning("Email verification token is missing or has invalid user ID.");
                return ResultExtensions.ValidationFailure<LoginResponse>(string.Empty, "Auth.VerifyEmail.InvalidToken");
            }

            var user = await userRepository.GetByIdReadonlyAsync(userId, cancellationToken);
            if (user == null)
            {
                logger.LogWarning("User not found for email verification. UserId: {UserId}", userId);
                return ResultExtensions.ValidationFailure<LoginResponse>(string.Empty, "Auth.VerifyEmail.UserNotFound");
            }

            if (user.IsEmailVerified)
            {
                logger.LogInformation("User email is already verified. UserId: {UserId}", userId);
                return ResultExtensions.ValidationFailure<LoginResponse>(string.Empty, "Auth.VerifyEmail.AlreadyVerified");
            }

            user.SetEmailVerified();

            userRepository.Update(user);

            _ = await unitOfWork.SaveChangesAsync(cancellationToken);

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            logger.LogInformation("Email verification process completed successfully for user: {Email}", user.Email);

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
