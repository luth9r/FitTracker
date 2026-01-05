using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.DTOs.Auth.Google;

/// <summary>
///     DTO for Google registration response.
/// </summary>
/// <param name="Email">User's email address.</param>
/// <param name="FirstName">User's first name.</param>
/// <param name="LastName">User's last name.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleRegisterResponse(string Email, string FirstName, string LastName);
