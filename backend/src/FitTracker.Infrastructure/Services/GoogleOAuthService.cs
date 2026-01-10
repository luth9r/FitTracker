using System.Net.Http.Json;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Services;

public class GoogleOAuthService(
    HttpClient httpClient,
    ILogger<GoogleOAuthService> logger,
    IConfiguration configuration) : IGoogleOAuthService
{
    private readonly string _tokenEndpoint = "https://oauth2.googleapis.com/token";

    private readonly string _clientId = configuration["Google:ClientId"] ??
                                        throw new InvalidOperationException("Google:ClientId not configured");

    private readonly string _clientSecret = configuration["Google:ClientSecret"] ??
                                            throw new InvalidOperationException("Google:ClientSecret not configured");

    private readonly string _redirectUri = configuration["Google:RedirectUri"] ??
                                           throw new InvalidOperationException("Google:RedirectUri not configured");

    /// <inheritdoc />
    public async Task<TokenResponse> ExchangeCodeForTokensAsync(string code, string codeVerifier)
    {
        var content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["redirect_uri"] = _redirectUri,
                ["grant_type"] = "authorization_code",
                ["code_verifier"] = codeVerifier,
            });

        var response = await httpClient.PostAsync(_tokenEndpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Google token exchange response body:");
        Console.WriteLine(responseBody);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Google token exchange failed: {errorContent}");
        }

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return body ?? throw new InvalidOperationException("Google API returned an empty response body.");
    }

    /// <inheritdoc />
    public async Task<TokenResponse> ExchangeCodeForTokensAsync(string code)
    {
        var parameters = new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = _clientId,
            ["client_secret"] = _clientSecret,
            ["grant_type"] = "authorization_code",
            ["redirect_uri"] = string.Empty,
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await httpClient.PostAsync(_tokenEndpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Google token exchange failed: {errorContent}");
        }

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return body ?? throw new InvalidOperationException("Google API returned an empty response body.");
    }

    /// <inheritdoc />
    public async Task<GoogleTokenPayload?> ValidateAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _clientId },
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            return new GoogleTokenPayload(payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);
        }
        catch (InvalidJwtException ex)
        {
            logger.LogWarning(ex, "Not valid google token");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in Google-token validation");
            return null;
        }
    }
}
