using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Abstract.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.Features.User.Commands.ForgotPassword;

/// <summary>
///     Handler responsible for managing forgot password command execution.
/// </summary>
/// <param name="readRepository">The repository for reading user-related data.</param>
/// <param name="writeRepository">The repository for writing user-related data.</param>
/// <param name="unit">The unit of work for managing transactions.</param>
/// <param name="localization">The service for handling culture-specific information.</param>
/// <param name="logger">The logger for capturing diagnostic and error information.</param>
public sealed class ForgotPasswordCommandHandler(
    IUserReadRepository readRepository,
    IUserWriteRepository writeRepository,
    IUnitOfWork unit,
    ILocalizationService localization,
    ILogger<ForgotPasswordCommandHandler> logger) : IRequestHandler<ForgotPasswordCommand, Result>
{
    /// <summary>
    ///     Handles the ForgotPasswordCommand to initiate a password reset process for a user.
    /// </summary>
    /// <param name="request">The command containing the email address of the user requesting the password reset.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A Result instance indicating the success or failure of the operation.</returns>
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var culture = localization.GetCurrentCulture();
        var user = await readRepository.GetByEmailReadonlyAsync(request.Email, cancellationToken);
        if (user is null || !user.IsEmailVerified)
        {
            if (user is { IsEmailVerified: false })
            {
                logger.LogWarning("Password reset denied: Email {Email} is not verified.", request.Email);
            }

            return Result.Success();
        }

        user.RequestPasswordReset(culture);

        writeRepository.Update(user);

        await unit.SaveChangesAsync(CancellationToken.None);

        return Result.Success();
    }
}
