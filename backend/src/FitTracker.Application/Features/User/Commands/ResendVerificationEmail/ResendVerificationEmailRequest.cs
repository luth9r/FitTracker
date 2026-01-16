using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Commands.ResendVerificationEmail;

/// <summary>
///     Request to resend the email verification link to a user.
/// </summary>
/// <param name="Email">The email address to send the verification link to.</param>
[ExcludeFromCodeCoverage]
public sealed record ResendVerificationEmailRequest(string Email);
