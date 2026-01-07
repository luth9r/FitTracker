using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.UseCases.User.Handlers.Commands;

/// <summary>
///     Handler for processing user registration commands.
/// </summary>
/// <param name="userReadRepository">The <see cref="IUserReadRepository" />.</param>
/// <param name="userWriteRepository">The <see cref="IUserWriteRepository" />.</param>
/// <param name="mapper">The <see cref="IMapper" />.</param>
/// <param name="unitOfWork">The <see cref="IUnitOfWork" />.</param>
/// <param name="localization">The <see cref="ILocalizationService" />.</param>
/// <param name="hasher">The <see cref="IPasswordHasher" />.</param>
/// <param name="logger">The <see cref="ILogger{RegisterCommandHandler}" />.</param>
public sealed class RegisterCommandHandler(
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ILocalizationService localization,
    IPasswordHasher hasher,
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, Result<RegisterResponse, ValidationResult>>
{
    /// <summary>
    ///     Handles the register command.
    /// </summary>
    /// <param name="request">The <see cref="RegisterCommand" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The <see cref="RegisterResponse" /> result.</returns>
    public async Task<Result<RegisterResponse, ValidationResult>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Starting user registration process for username: {Username}", request.User.Username);

        var currentCulture = localization.GetCurrentCulture();
        var userRequest = request.User;

        var existingUser = await userReadRepository.GetByUsernameReadonlyAsync(userRequest.Username, cancellationToken);
        if (existingUser != null)
        {
            logger.LogWarning("Registration failed: Username {Username} already exists.", userRequest.Username);
            return ResultExtensions.ValidationFailure<RegisterResponse>(
                nameof(userRequest.Username),
                localization.GetString("Auth.Register.UsernameAlreadyExists"));
        }

        var existingUserByEmail = await userReadRepository.GetByEmailReadonlyAsync(userRequest.Email, cancellationToken);
        if (existingUserByEmail != null)
        {
            logger.LogWarning("Registration failed: Email {Email} already exists.", userRequest.Email);
            return ResultExtensions.ValidationFailure<RegisterResponse>(
                nameof(userRequest.Email),
                localization.GetString("Auth.Register.EmailAlreadyExists"));
        }

        var user = UserEntity.Create(
            userRequest.Username,
            userRequest.Email,
            hasher.HashPassword(userRequest.Password),
            culture: currentCulture);

        // Add to trackings
        await userWriteRepository.AddAsync(user, cancellationToken);

        // Save changes to db
        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogDebug("AFTER SaveChanges: {Id}", user.Id);

        var response = mapper.Map<RegisterResponse>(user);

        return Result.Success<RegisterResponse, ValidationResult>(response);
    }
}
