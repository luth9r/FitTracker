using CSharpFunctionalExtensions;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;
using UserEntity = FitTracker.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;

namespace FitTracker.Application.UseCases.User.Handlers
{
    public class RegisterCommandHandler(IUserRepository userRepository) : IRequestHandler<RegisterCommand, Result<UserEntity, ValidationResult>>
    {
        public async Task<Result<UserEntity, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userRequest = request.User;
            var userBuilderResult = new UserEntity.UserBuilder()
                .WithUsername(userRequest.Username)
                .WithEmail(userRequest.Email)
                .WithPasswordHash(userRequest.Password)
                .Build();

            return userBuilderResult.IsFailure ? userBuilderResult : await userRepository.AddAsync(userBuilderResult.Value, cancellationToken);

        }
    }
}
