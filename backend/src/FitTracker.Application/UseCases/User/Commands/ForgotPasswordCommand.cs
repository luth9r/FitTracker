using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands;

/// <summary>
///     Represents a command to initiate the forgot password process for a user.
/// </summary>
/// <param name="Email">The email address of the user.</param>
[ExcludeFromCodeCoverage]
public sealed record ForgotPasswordCommand(string Email) : IRequest<Result>, IRateLimitedRequest
{
    /// <inheritdoc />
    public string GetRateLimitKey()
    {
        return $"ratelimit:password-reset:{Email}";
    }

    /// <inheritdoc />
    public TimeSpan GetLimitPeriod()
    {
        return TimeSpan.FromMinutes(1);
    }
}
