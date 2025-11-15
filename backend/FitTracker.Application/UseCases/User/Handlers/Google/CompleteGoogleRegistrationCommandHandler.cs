using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Application.Validators;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
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
    public class CompleteGoogleRegistrationCommandHandler(
        ILogger<CompleteGoogleRegistrationCommandHandler> logger,
        IGoogleTokenValidator googleTokenValidator,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        IMapper mapper,
        ILocalizationService localization)
        : IRequestHandler<CompleteGoogleRegistrationCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(CompleteGoogleRegistrationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Attempting to complete Google registration for {Username}", request.Request.UserName);

            var validator = new CompleteGoogleRegistrationValidator(userRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Validation failed for {Username}: {Errors}", request.Request.UserName, validationResult.Errors);
                return Result.Failure<LoginResponse, ValidationResult>(validationResult);
            }

            // Validate the Google IdToken
            var googlePayload = await googleTokenValidator.ValidateAsync(request.Request.IdToken);
            if (googlePayload == null)
            {
                logger.LogWarning("Invalid IdToken provided during registration completion.");
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.IdToken), localization.GetString("Google.Auth.InvalidToken"));
            }

            logger.LogInformation("Token validated for {Email}", googlePayload.Email);


            // Check if user already exists
            if (await userRepository.GetByUsernameReadonlyAsync(request.Request.UserName, cancellationToken) != null ||
                await userRepository.GetByEmailReadonlyAsync(googlePayload.Email, cancellationToken) != null ||
                await userRepository.GetByGoogleTokenReadonlyAsync(googlePayload.GoogleId, cancellationToken) != null)
            {
                logger.LogWarning("Account already exists for {Email} or {GoogleId}", googlePayload.Email, googlePayload.GoogleId);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.IdToken), localization.GetString("Auth.Register.UsernameAlreadyExists"));
            }

            var userBuilderResult = new UserEntity.UserBuilder()
                .WithUsername(request.Request.UserName)
                .WithEmail(googlePayload.Email)
                .WithGoogleProvidedId(googlePayload.GoogleId)
                .WithFirstName(googlePayload.FirstName)
                .WithLastName(googlePayload.LastName)
                .Build();

            if (userBuilderResult.IsFailure)
            {
                var translatedErrors = userBuilderResult.Error.Errors
                    .Select(failure => new ValidationFailure(failure.PropertyName, localization.GetString(failure.ErrorMessage)))
                    .ToList();

                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(UserEntity), translatedErrors);
            }

            var newUser = userBuilderResult.Value;
            newUser.SetEmailVerified();

            await userRepository.AddAsync(newUser, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Successfully created and registered user {Username} with ID {UserId}", newUser.Username, newUser.Id);

            var loginToken = jwtTokenGenerator.GenerateToken(newUser);

            var response = mapper.Map<LoginResponse>(newUser);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
