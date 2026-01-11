using AutoMapper;
using CSharpFunctionalExtensions;
using FitTracker.Application.Constants;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Constants;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Commands;

/// <summary>
///     Handler for processing email verification commands.
/// </summary>
/// <param name="jwtTokenGenerator">The <see cref="IJwtTokenGenerator" />.</param>
/// <param name="jwtTokenValidator">The <see cref="IJwtTokenValidator" />.</param>
/// <param name="userReadRepository">The <see cref="IUserReadRepository" />.</param>
/// <param name="userWriteRepository">The <see cref="IUserWriteRepository" />.</param>
/// <param name="unitOfWork">The <see cref="IUnitOfWork" />.</param>
/// <param name="mapper">The <see cref="IMapper" />.</param>
/// <param name="logger">The <see cref="ILogger{VerifyEmailCommandHandler}" />.</param>
public sealed class VerifyEmailCommandHandler(
    IJwtTokenGenerator jwtTokenGenerator,
    IJwtTokenValidator jwtTokenValidator,
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<VerifyEmailCommandHandler> logger)
    : IRequestHandler<VerifyEmailCommand, Result<LoginResponse, ValidationResult>>
{
    /// <summary>
    ///     Handles the verify email command.
    /// </summary>
    /// <param name="request">The <see cref="VerifyEmailCommand" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The <see cref="LoginResponse" /> result.</returns>
    public async Task<Result<LoginResponse, ValidationResult>> Handle(
        VerifyEmailCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = jwtTokenValidator.ValidatePurposeToken(request.Token, TokenPurposes.EmailVerification);

        if (validationResult.IsFailure)
        {
            logger.LogWarning("Email verification failed: {Error}", validationResult.Error);
            return ResultExtensions.ValidationFailure<LoginResponse>(
                string.Empty,
                DomainErrors.Auth.InvalidToken);
        }

        var userId = validationResult.Value;
        var user = await userReadRepository.GetByIdReadonlyAsync(userId, cancellationToken);
        if (user == null)
        {
            logger.LogWarning("User not found for email verification. UserId: {UserId}", userId);
            return ResultExtensions.ValidationFailure<LoginResponse>(
                string.Empty,
                DomainErrors.User.NotFound);
        }

        if (user.IsEmailVerified)
        {
            logger.LogInformation("User email is already verified. UserId: {UserId}", userId);
            return ResultExtensions.ValidationFailure<LoginResponse>(
                string.Empty,
                DomainErrors.User.EmailAlreadyVerified);
        }

        user.SetEmailVerified();

        userWriteRepository.Update(user);

        _ = await unitOfWork.SaveChangesAsync(cancellationToken);

        var loginToken = jwtTokenGenerator.GenerateToken(user);

        var response = mapper.Map<LoginResponse>(user) with
        {
            JWT = loginToken,
        };

        logger.LogInformation("Email verification process completed successfully for user: {Email}", user.Email);

        return Result.Success<LoginResponse, ValidationResult>(response);
    }
}
