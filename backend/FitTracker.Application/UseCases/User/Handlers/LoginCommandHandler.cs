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

namespace FitTracker.Application.UseCases.User.Handlers
{
    public class LoginCommandHandler(IUserRepository repository,
        IMapper mapper,
        IPasswordHasher hasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILocalizationService localization) : IRequestHandler<LoginCommand, Result<LoginResponse, ValidationResult>>
    {
        public async Task<Result<LoginResponse, ValidationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userEmail = request.Request.Email;
            var userPassword = request.Request.Password;

            var user = await repository.GetByEmailReadonlyAsync(userEmail, cancellationToken);

            if (user == null || !user.IsEmailVerified)
            {
                var errorMessage = localization.GetString("Auth.Login.InvalidCredentials");

                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(request.Request.Email), errorMessage)
                });
                return Result.Failure<LoginResponse, ValidationResult>(errors);
            }

            var checkPassword = hasher.VerifyPassword(userPassword, user.PasswordHash);
            if (!checkPassword)
            {
                var errorMessage = localization.GetString("Auth.Login.InvalidCredentials");

                var errors = new ValidationResult(new[]
                {
                    new ValidationFailure(nameof(request.Request.Password), errorMessage)
                });
                return Result.Failure<LoginResponse, ValidationResult>(errors);
            }

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
