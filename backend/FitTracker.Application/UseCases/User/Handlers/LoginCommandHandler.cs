using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

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
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Email), localization.GetString("Auth.Login.InvalidCredentials"));
            }

            var checkPassword = hasher.VerifyPassword(userPassword, user.PasswordHash);
            if (!checkPassword)
            {
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(request.Request.Password), localization.GetString("Auth.Login.InvalidCredentials"));
            }

            var loginToken = jwtTokenGenerator.GenerateToken(user);

            var response = mapper.Map<LoginResponse>(user);
            response.JWT = loginToken;

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
