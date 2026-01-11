using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using FitTracker.Application.Interfaces;
using FluentValidation.Results;
using MediatR;

namespace FitTracker.Application.UseCases.User.Commands;

/// <summary>
///     Query to resend email verification link to a user who has not yet verified their account.
/// </summary>
/// <param name="Email">The email address to send the verification link to.</param>
[ExcludeFromCodeCoverage]
public sealed record ResendVerificationEmailCommand(string Email)
    : IRequest<Result<Unit, ValidationResult>>, IRateLimitedRequest
{
    /// <inheritdoc />
    public string GetRateLimitKey()
    {
        return $"ratelimit:email:{Email}";
    }

    /// <inheritdoc />
    public TimeSpan GetLimitPeriod()
    {
        return TimeSpan.FromMinutes(1);
    }
}
