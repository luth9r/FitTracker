using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Commands
{
    /// <summary>
    /// Handler for processing login commands.
    /// </summary>
    /// <param name="userReadRepository">The <see cref="IUserReadRepository"/>.</param>
    /// <param name="mapper">The <see cref="IMapper"/>.</param>
    /// <param name="hasher">The <see cref="IPasswordHasher"/>.</param>
    /// <param name="jwtTokenService">The <see cref="IJwtTokenGenerator"/>.</param>
    /// <param name="localization">The <see cref="ILocalizationService"/>.</param>
    /// <param name="logger">The <see cref="ILogger{LoginCommandHandler}"/>.</param>
    public sealed class LoginCommandHandler(
        IUserReadRepository userReadRepository,
        IMapper mapper,
        IPasswordHasher hasher,
        IJwtTokenGenerator jwtTokenService,
        ILocalizationService localization,
        ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, Result<LoginResponse, ValidationResult>>
    {
        /// <summary>
        /// Handles the login command.
        /// </summary>
        /// <param name="request">The <see cref="LoginCommand"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="LoginResponse"/> result.</returns>
        public async Task<Result<LoginResponse, ValidationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting login process for email: {Email}", request.Request.Email);

            var userEmail = request.Request.Email;
            var userPassword = request.Request.Password;

            var user = await userReadRepository.GetByEmailReadonlyAsync(userEmail, cancellationToken);

            if (user == null || !user.IsEmailVerified)
            {
                logger.LogWarning("Login failed for email: {Email}. User not found or email not verified.", userEmail);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Email), localization.GetString("Auth.Login.InvalidCredentials"));
            }

            var checkPassword = hasher.VerifyPassword(userPassword, user.PasswordHash ?? string.Empty); // If PasswordHash is null (e.g., social login), treat as invalid

            if (!checkPassword)
            {
                logger.LogWarning("Login failed for email: {Email}. Invalid password.", userEmail);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Password), localization.GetString("Auth.Login.InvalidCredentials"));
            }

            var loginToken = jwtTokenService.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user) with
            {
                JWT = loginToken
            };

            logger.LogInformation("Login process completed successfully for email: {Email}", userEmail);

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
