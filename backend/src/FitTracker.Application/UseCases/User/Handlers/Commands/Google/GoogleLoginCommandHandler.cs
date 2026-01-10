using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Commands.Google;

/// <summary>
///     Handler for processing Google login commands.
/// </summary>
/// <param name="logger">The <see cref="ILogger{GoogleLoginCommandHandler}" />.</param>
/// <param name="googleOAuthService">The <see cref="IGoogleOAuthService" />.</param>
/// <param name="userReadRepository">The <see cref="IUserReadRepository" />.</param>
/// <param name="jwtTokenService">The <see cref="IJwtTokenGenerator" />.</param>
/// <param name="localization">The <see cref="ILocalizationService" />.</param>
/// <param name="mapper">The <see cref="IMapper" />.</param>
public sealed class GoogleLoginCommandHandler(
    ILogger<GoogleLoginCommandHandler> logger,
    IGoogleOAuthService googleOAuthService,
    IUserReadRepository userReadRepository,
    IJwtTokenGenerator jwtTokenService,
    ILocalizationService localization,
    IMapper mapper) : IRequestHandler<GoogleLoginCommand, Result<LoginResponse, ValidationResult>>
{
    /// <summary>
    ///     Handles the Google login command.
    /// </summary>
    /// <param name="request">The <see cref="GoogleLoginCommand" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The <see cref="LoginResponse" /> result.</returns>
    public async Task<Result<LoginResponse, ValidationResult>> Handle(
        GoogleLoginCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting Google login process.");

        var tokenResponse = await googleOAuthService.ExchangeCodeForTokensAsync(
            request.Request.Code,
            request.Request.CodeVerifier);

        logger.LogDebug("Attempting to validate Google IdToken.");

        var googlePayload = await googleOAuthService.ValidateAsync(tokenResponse.IdToken);

        if (googlePayload == null)
        {
            logger.LogWarning("Google Token validation failed.");
            return ResultExtensions.ValidationFailure<LoginResponse>(
                nameof(request.Request.Code),
                localization.GetString("Google.Auth.InvalidToken"));
        }

        logger.LogDebug("Google Token validated for email: {Email}", googlePayload.Email);

        var user = await userReadRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken);

        if (user == null)
        {
            logger.LogDebug("User not found by GoogleId. Checking by email: {Email}", googlePayload.Email);

            user = await userReadRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken);

            if (user == null)
            {
                logger.LogDebug("User not found.");

                return ResultExtensions.ValidationFailure<LoginResponse>(
                    nameof(request.Request.Code),
                    localization.GetString("Google.Auth.NotFound"));
            }
        }
        else
        {
            logger.LogDebug("User found by GoogleId. Skipping lookup.");
        }

        var loginToken = jwtTokenService.GenerateToken(user);

        var response = mapper.Map<LoginResponse>(user) with
        {
            JWT = loginToken,
        };

        logger.LogInformation("Google login process completed successfully for user: {Email}", googlePayload.Email);

        return Result.Success<LoginResponse, ValidationResult>(response);
    }
}
