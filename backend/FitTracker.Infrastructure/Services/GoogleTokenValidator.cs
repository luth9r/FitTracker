using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Infrastructure.Services
{
    public class GoogleTokenValidator(
        ILogger<GoogleTokenValidator> logger,
        IConfiguration configuration) : IGoogleTokenValidator
    {
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
                    Audience = new[] { googleClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                return new GoogleTokenPayload
                {
                    GoogleId = payload.Subject,
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName
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
