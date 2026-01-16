using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.Features.User.Commands.GoogleMobileAuth;

/// <summary>
///     Represents a request for authenticating a user via Google's mobile authentication flow.
/// </summary>
/// <param name="Code">The authorization code received from Google.</param>
[ExcludeFromCodeCoverage]
public sealed record GoogleMobileAuthRequest(string Code);
