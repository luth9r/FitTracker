using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.Interfaces;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace FitTracker.Infrastructure.Services
{
    public class GoogleOAuthService(HttpClient httpClient,
        ILogger<GoogleOAuthService> logger,
        IConfiguration configuration) : IGoogleOAuthService
    {
        public async Task<Application.DTOs.Auth.Google.TokenResponse> ExchangeCodeForTokensAsync(string code, string codeVerifier)
        {
            var tokenEndpoint = "https://oauth2.googleapis.com/token";

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = configuration["Google:ClientId"],
                ["client_secret"] = configuration["Google:ClientSecret"],
                ["redirect_uri"] = configuration["Google:RedirectUri"],
                ["grant_type"] = "authorization_code",
                ["code_verifier"] = codeVerifier,
            });

            var response = await httpClient.PostAsync(tokenEndpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Google token exchange response body:");
            Console.WriteLine(responseBody);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Google token exchange failed: {errorContent}");
            }

            var body = await response.Content.ReadFromJsonAsync<Application.DTOs.Auth.Google.TokenResponse>();
            return body;
        }

        public async Task<GoogleTokenPayload?> ValidateAsync(string idToken)
        {
            try
            {
                var googleClientId = configuration["Google:ClientId"];

                if (string.IsNullOrEmpty(googleClientId))
                {
                    logger.LogCritical("Google:ClientId not cofigured in appsettings.json");
                    return null;
                }

                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { googleClientId },
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                return new GoogleTokenPayload
                {
                    GoogleId = payload.Subject,
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                };
            }
            catch (InvalidJwtException ex)
            {
                logger.LogWarning("Not valid google token: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in Google-token validation");
                return null;
            }
        }
    }
}
