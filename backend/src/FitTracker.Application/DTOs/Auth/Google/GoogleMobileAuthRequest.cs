using System.Diagnostics.CodeAnalysis;

namespace FitTracker.Application.DTOs.Auth.Google;

/// <summary>
///     Represents a request for authenticating a user via Google's mobile authentication flow.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record GoogleMobileAuthRequest(string Code);
