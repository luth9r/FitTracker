using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.DTOs.Auth;

/// <summary>
///     DTO for forgot password request.
/// </summary>
/// <param name="Email">User's email address to send the password reset link.</param>
[ExcludeFromCodeCoverage]
public sealed record ForgotPasswordRequest(string Email);
