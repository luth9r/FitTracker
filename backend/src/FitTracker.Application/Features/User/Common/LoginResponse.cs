using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Common;

/// <summary>
///     DTO for user login response.
/// </summary>
/// <param name="Username">Username of the logged in user.</param>
/// <param name="Email">Email of the logged in user.</param>
/// <param name="Jwt">JWT token for authenticated sessions.</param>
[ExcludeFromCodeCoverage]
public sealed record LoginResponse(string Username, string Email, string Jwt);
