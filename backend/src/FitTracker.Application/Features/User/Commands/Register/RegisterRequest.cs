using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Commands.Register;

/// <summary>
///     DTO for user registration requests.
/// </summary>
/// <param name="Username">The username for registration.</param>
/// <param name="Email">The email for registration.</param>
/// <param name="Password">The password for registration.</param>
[ExcludeFromCodeCoverage]
public sealed record RegisterRequest(string Username, string Email, string Password);
