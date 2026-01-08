using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace FitTracker.Application.DTOs.Auth.Google;

/// <summary>
///     DTO representing the response from Google's API.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record TokenResponse
{
    /// <summary>
    ///     Gets the access token issued by Google.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; }

    /// <summary>
    ///     Gets the lifetime in seconds of the access token.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    /// <summary>
    ///     Gets the scope of the access token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; init; }

    /// <summary>
    ///     Gets the type of the token.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }

    /// <summary>
    ///     Gets the ID token issued by Google.
    /// </summary>
    [JsonPropertyName("id_token")]
    public string IdToken { get; init; }

    /// <summary>
    ///     Gets the refresh token issued by Google.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; }
}