using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Commands.Login;

/// <summary>
///     DTO for user login request.
/// </summary>
/// <param name="Email">User's email address.</param>
/// <param name="Password">User's password.</param>
[ExcludeFromCodeCoverage]
public sealed record LoginRequest(string Email, string Password);
