using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using Google.Apis.Auth;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Google
{
    /// <summary>
    /// Handler for processing Google login commands.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="googleOAuthService"></param>
    /// <param name="userRepository"></param>
    /// <param name="jwtTokenGenerator"></param>
    /// <param name="localization"></param>
    /// <param name="mapper"></param>
    public class GoogleLoginCommandHandler(
        ILogger<GoogleLoginCommandHandler> logger,
        IGoogleOAuthService googleOAuthService,
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILocalizationService localization,
        IMapper mapper) : IRequestHandler<GoogleLoginCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Google login process.");

            var tokenResponse = await googleOAuthService.ExchangeCodeForTokensAsync(request.Request.Code, request.Request.CodeVerifier);
            if (tokenResponse == null)
            {
                logger.LogWarning("Google Token validation failed.");
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Google.Auth.InvalidToken"));
            }

            logger.LogInformation("Attempting to validate Google IdToken.");

            var googlePayload = await googleOAuthService.ValidateAsync(tokenResponse.IdToken);

            if (googlePayload == null)
            {
                logger.LogWarning("Google Token validation failed.");
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Google.Auth.InvalidToken"));
            }

            logger.LogInformation("Google Token validated for email: {Email}", googlePayload.Email);

            var user = await userRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken);

            if (user == null)
            {
                logger.LogInformation("User not found by GoogleId. Checking by email: {Email}", googlePayload.Email);

                user = await userRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken);

                if (user == null)
                {
                    logger.LogInformation("User not found.");

                    return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Google.Auth.NotFound"));
                }
            }
            else
            {
                logger.LogInformation("User found by GoogleId. Skipping lookup.");
            }

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            logger.LogInformation("Google login process completed successfully for user: {Email}", googlePayload.Email);

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
