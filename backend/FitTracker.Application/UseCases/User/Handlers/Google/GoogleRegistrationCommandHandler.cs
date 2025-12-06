using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers.Google
{
    /// <summary>
    /// Handler for processing Google registration commands.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="googleOAuthService"></param>
    /// <param name="userRepository"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="jwtTokenGenerator"></param>
    /// <param name="localization"></param>
    /// <param name="mapper"></param>
    public class GoogleRegistrationCommandHandler(
        ILogger<GoogleLoginCommandHandler> logger,
        IGoogleOAuthService googleOAuthService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        ILocalizationService localization,
        IMapper mapper) : IRequestHandler<GoogleRegisterCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(GoogleRegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Google registration process.");
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

            if (await userRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken) != null ||
                await userRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken) != null)
            {
                logger.LogWarning("Account already exists for {Email} or {GoogleId}", googlePayload.Email, googlePayload.GoogleId);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Auth.Register.AccountAlreadyExists"));
            }

            var user = UserEntity.CreateGoogleUser(
                email: googlePayload.Email,
                firstName: googlePayload.FirstName,
                lastName: googlePayload.LastName,
                googleProviderId: googlePayload.GoogleId);

            user.SetEmailVerified();

            await userRepository.AddAsync(user, cancellationToken);
            _ = await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created and registered user {Username} with ID {UserId}", user.Username, user.Id);

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
