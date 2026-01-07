using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.UseCases.User.Handlers.Commands;

/// <summary>
/// Handles the process of resending a verification email to a user.
/// </summary> <param name="userReadRepository"> Repository for retrieving user data in a read-only context.</param>
/// <param name="userWriteRepository">Repository for performing modifications on user data.</param>
/// <param name="logger"> Logging service used to log information, warnings, and errors during processing. </param>
/// <param name="localization"> Service for obtaining localization and cultural settings.</param>
/// <param name="rateLimitService"> Service used to enforce rate-limiting rules.</param>
/// <param name="unit">Unit of work responsible for committing transactional operations.</param>
public sealed class ResendVerificationEmailCommandHandler(
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    ILogger<ResendVerificationEmailCommandHandler> logger,
    ILocalizationService localization,
    IRateLimitService rateLimitService,
    IUnitOfWork unit)
    : IRequestHandler<ResendVerificationEmailCommand, Result<Unit, ValidationResult>>
{
    /// <summary>
    ///     Handles the resend verification email query.
    /// </summary>
    /// <param name="request">The <see cref="ResendVerificationEmailCommand" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>A <see cref="Result{Unit, ValidationResult}" /> indicating success or validation failure.</returns>
    public async Task<Result<Unit, ValidationResult>> Handle(
        ResendVerificationEmailCommand request,
        CancellationToken cancellationToken)
    {
        var culture = localization.GetCurrentCulture();

        // Retrieve user by email
        var user = await userReadRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);

        if (user == null)
        {
            // Security: Return success to prevent email enumeration
            logger.LogWarning("Resend verification email requested for non-existing email: {Email}", request.Email);
            return Result.Success<Unit, ValidationResult>(Unit.Value);
        }

        // Check if email is already verified
        if (user.IsEmailVerified)
        {
            logger.LogInformation(
                "Resend verification email requested for already verified email: {Email}",
                request.Email);
            return ResultExtensions.ValidationFailure<Unit>(
                nameof(request.Email),
                localization.GetString("User.EmailAlreadyVerified", culture));
        }

        var key = $"ratelimit:email:{user.Id}";
        if (!await rateLimitService.IsAllowedAsync(key, TimeSpan.FromMinutes(1)))
        {
            return ResultExtensions.ValidationFailure<Unit>(
                nameof(request.Email),
                localization.GetString("User.RateLimitExceeded", culture));
        }

        user.RequestVerificationEmail(culture);

        userWriteRepository.Update(user);

        await unit.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Verification email resend requested for user: {UserId}", user.Id);

        return Result.Success<Unit, ValidationResult>(Unit.Value);
    }
}
