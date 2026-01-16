using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Commands.Register;

/// <summary>
///     Response returned after successful user registration.
/// </summary>
/// <param name="Email">The registered user's email address.</param>
/// <param name="Username">The registered user's username.</param>
[ExcludeFromCodeCoverage]
public sealed record RegisterResponse(string Username, string Email);
