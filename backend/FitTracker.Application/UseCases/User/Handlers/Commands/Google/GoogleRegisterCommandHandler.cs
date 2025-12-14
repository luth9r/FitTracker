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

namespace FitTracker.Application.UseCases.User.Handlers.Commands.Google
{
    /// <summary>
    /// Handler for processing Google registration commands.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger{GoogleRegistrationCommandHandler}"/>.</param>
    /// <param name="googleOAuthService">The <see cref="IGoogleOAuthService"/>.</param>
    /// <param name="userReadRepository">The <see cref="IUserReadRepository"/>.</param>
    /// <param name="unitOfWork">The <see cref="IUnitOfWork"/>.</param>
    /// <param name="jwtTokenGenerator">The <see cref="IJwtTokenGenerator"/>.</param>
    /// <param name="localization">The <see cref="ILocalizationService"/>.</param>
    /// <param name="mapper">The <see cref="IMapper"/>.</param>
    public sealed class GoogleRegisterCommandHandler(
        ILogger<GoogleRegisterCommandHandler> logger,
        IGoogleOAuthService googleOAuthService,
        IUserReadRepository userReadRepository,
        IUserWriteRepository userWriteRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        ILocalizationService localization,
        IMapper mapper) : IRequestHandler<GoogleRegisterCommand, Result<LoginResponse, ValidationResult>>
    {
        /// <summary>
        /// Handles the Google registration command.
        /// </summary>
        /// <param name="request">The <see cref="GoogleRegisterCommand"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="LoginResponse"/> result.</returns>
        public async Task<Result<LoginResponse, ValidationResult>> Handle(GoogleRegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting Google registration process.");
            var tokenResponse = await googleOAuthService.ExchangeCodeForTokensAsync(request.Request.Code, request.Request.CodeVerifier);
            if (tokenResponse == null)
            {
                logger.LogWarning("Google Token validation failed.");
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Google.Auth.InvalidToken"));
            }

            logger.LogDebug("Attempting to validate Google IdToken.");

            var googlePayload = await googleOAuthService.ValidateAsync(tokenResponse.IdToken);

            if (googlePayload == null)
            {
                logger.LogWarning("Google Token validation failed.");
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Google.Auth.InvalidToken"));
            }

            if (await userReadRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken) != null ||
                await userReadRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken) != null)
            {
                logger.LogWarning("Account already exists for {Email} or {GoogleId}", googlePayload.Email, googlePayload.GoogleId);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Code), localization.GetString("Auth.Register.AccountAlreadyExists"));
            }

            var user = UserEntity.CreateGoogleUser(
                email: googlePayload.Email,
                googleProviderId: googlePayload.GoogleId,
                firstName: googlePayload.FirstName,
                lastName: googlePayload.LastName);

            user.SetEmailVerified();

            await userWriteRepository.AddAsync(user, cancellationToken);
            _ = await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created and registered user {Username} with ID {UserId}", user.Username, user.Id);

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user) with
            {
                JWT = loginToken
            };

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
