using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Events;
using FitTracker.Application.Extensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers.Commands
{
    /// <summary>
    /// Handler for processing user registration commands.
    /// </summary>
    /// <param name="userReadRepository">The <see cref="IUserReadRepository"/>.</param>
    /// <param name="userWriteRepository">The <see cref="IUserWriteRepository"/>.</param>
    /// <param name="mapper">The <see cref="IMapper"/>.</param>
    /// <param name="unitOfWork">The <see cref="IUnitOfWork"/>.</param>
    /// <param name="localization">The <see cref="ILocalizationService"/>.</param>
    /// <param name="hasher">The <see cref="IPasswordHasher"/>.</param>
    /// <param name="logger">The <see cref="ILogger{RegisterCommandHandler}"/>.</param>
    public sealed class RegisterCommandHandler(
        IUserReadRepository userReadRepository,
        IUserWriteRepository userWriteRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILocalizationService localization,
        IPasswordHasher hasher,
        ILogger<RegisterCommandHandler> logger) : IRequestHandler<RegisterCommand, Result<LoginResponse, ValidationResult>>
    {
        /// <summary>
        /// Handles the register command.
        /// </summary>
        /// <param name="request">The <see cref="RegisterCommand"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="LoginResponse"/> result.</returns>
        public async Task<Result<LoginResponse, ValidationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Starting user registration process for username: {Username}", request.User.Username);

            var userRequest = request.User;

            var existingUser = await userReadRepository.GetByUsernameReadonlyAsync(userRequest.Username, cancellationToken);
            if (existingUser != null)
            {
                logger.LogWarning("Registration failed: Username {Username} already exists.", userRequest.Username);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(userRequest.Username), localization.GetString("Auth.Register.UsernameAlreadyExists"));
            }

            var existingUserByEmail = await userReadRepository.GetByEmailReadonlyAsync(userRequest.Email, cancellationToken);
            if (existingUserByEmail != null)
            {
                logger.LogWarning("Registration failed: Email {Email} already exists.", userRequest.Email);
                return ResultExtensions.ValidationFailure<LoginResponse>(nameof(userRequest.Email), localization.GetString("Auth.Register.EmailAlreadyExists"));
            }

            var user = UserEntity.Create(
                username: userRequest.Username,
                email: userRequest.Email,
                passwordHash: hasher.HashPassword(userRequest.Password));

            logger.LogDebug("BEFORE SaveChanges: {Id}", user.Id);

            // Add to trackings
            await userWriteRepository.AddAsync(user, cancellationToken);

            // Save changes to db
            _ = await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogDebug("AFTER SaveChanges: {Id}", user.Id);

            await mediator.Publish(new UserRegisteredEvent(user.Id, user.Email, user.Username), cancellationToken);

            var response = mapper.Map<LoginResponse>(user);

            return Result.Success<LoginResponse, ValidationResult>(response);
        }
    }
}
