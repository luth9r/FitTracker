using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers
{
    public class RegisterCommandHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork, ILocalizationService localization) : IRequestHandler<RegisterCommand, Result<RegisterResponse, ValidationResult>>
    {
        public async Task<Result<RegisterResponse, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userRequest = request.User;

            var existingUser = await userRepository.GetByUsernameAsync(userRequest.Username, cancellationToken);
            if (existingUser != null)
            {
                var errorMessage = localization.GetString("Auth.Register.UsernameAlreadyExists");

                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(userRequest.Username), errorMessage)
                });
                return Result.Failure<RegisterResponse, ValidationResult>(errors);
            }

            var userBuilderResult = new UserEntity.UserBuilder()
                .WithUsername(userRequest.Username)
                .WithEmail(userRequest.Email)
                .WithPasswordHash(userRequest.Password)
                .Build();

            if (userBuilderResult.IsFailure)
            {
                var translatedErrors = userBuilderResult.Error.Errors
                    .Select(failure => new ValidationFailure(failure.PropertyName, localization.GetString(failure.ErrorMessage)))
                    .ToList();

                var translatedValidationResult = new ValidationResult(translatedErrors);

                return Result.Failure<RegisterResponse, ValidationResult>(translatedValidationResult);
            }

            var user = userBuilderResult.Value;

            // Add to tracking
            await userRepository.AddAsync(user, cancellationToken);

            // Save changes to db
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = mapper.Map<RegisterResponse>(user);

            response.JWT = "abracadabra"; // TODO

            return Result.Success<RegisterResponse, ValidationResult>(response);


        }
    }
}
