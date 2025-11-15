using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Application.Extensions;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Google
{
    public class GoogleLoginCommandHandler(
        ILogger<GoogleLoginCommandHandler> logger,
        IGoogleTokenValidator googleTokenValidator,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        ILocalizationService localization,
        IMapper mapper) : IRequestHandler<GoogleLoginCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Attempting to validate Google IdToken.");

            var googlePayload = await googleTokenValidator.ValidateAsync(request.Request.IdToken);

            if (googlePayload == null)
            {
                logger.LogWarning("Google Token validation failed.");
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.IdToken), localization.GetString("Google.Auth.InvalidToken"));
            }

            logger.LogInformation("Google Token validated for email: {Email}", googlePayload.Email);

            var user = await userRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken);

            if (user == null)
            {
                logger.LogInformation("User not found by GoogleId. Checking by email: {Email}", googlePayload.Email);

                user = await userRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken);

                if (user == null)
                {
                    logger.LogInformation("User not found. Returning 'NEEDS_REGISTRATION' flag.");

                    string errorCode = $"NEEDS_REGISTRATION::{googlePayload.Email}::{googlePayload.FirstName}::{googlePayload.LastName}";

                    return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.IdToken), errorCode);
                }
                else
                {
                    logger.LogInformation("User found by email. Linking GoogleProviderId.");
                    user.SetGoogleProviderId(googlePayload.GoogleId);
                    userRepository.Update(user);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                logger.LogInformation("User found by GoogleId. Skipping lookup.");
            }

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
