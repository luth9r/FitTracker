using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;
using UserEntity = FitTracker.Domain.Entities.User;

namespace FitTracker.Application.Features.User.Commands.Register;

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
        logger.LogDebug("Starting user registration process for username: {Username}", request.Username);

        var currentCulture = localization.GetCurrentCulture();

        var existingUser = await userReadRepository.GetByUsernameReadonlyAsync(request.Username, cancellationToken);
        if (existingUser != null)
        {
            logger.LogWarning("Registration failed: Username {Username} already exists.", request.Username);
            return ResultExtensions.ValidationFailure<RegisterResponse>(
                nameof(request.Username),
                DomainErrors.Auth.UsernameAlreadyExists);
        }

        var existingUserByEmail =
            await userReadRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);
        if (existingUserByEmail != null)
        {
            logger.LogWarning("Registration failed: Email {Email} already exists.", request.Email);
            return ResultExtensions.ValidationFailure<RegisterResponse>(
                nameof(request.Email),
                DomainErrors.Auth.EmailAlreadyExists);
        }

        var user = UserEntity.Create(
            request.Username,
            request.Email,
            hasher.HashPassword(request.Password),
            culture: currentCulture);

        // Add to trackings
        await userWriteRepository.AddAsync(user, cancellationToken);

        // Save changes to db
        _ = await unitOfWork.SaveChangesAsync(CancellationToken.None);

        logger.LogDebug("AFTER SaveChanges: {Id}", user.Id);

        var response = mapper.Map<RegisterResponse>(user);

        return Result.Success<RegisterResponse, ValidationResult>(response);
    }
}
