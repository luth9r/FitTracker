using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Application.Interfaces;
using FitTracker.Infrastructure.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Services
{
    public class LocalizationService(JsonLocalizationProvider provider, IHttpContextAccessor httpContextAccessor, ILogger<LocalizationService> logger) : ILocalizationService
    {
        public IEnumerable<string> GetAvailableCultures()
        {
            return provider.GetAvailableCultures();
        }

        public string GetString(string key)
        {
            var culture = GetCurrentCulture();
            return provider.GetString(key, culture);
        }

        public string GetString(string key, string culture)
        {
            return provider.GetString(key, culture);
        }


        /// <summary>
        /// Get current culture from Accept-Language Header
        /// </summary>
        /// <returns></returns>
        private string GetCurrentCulture()
        {
            try
            {
                var context = httpContextAccessor.HttpContext;
                if (context == null)
                {
                    logger.LogWarning("HttpContext is null, using default culture");
                    return "en-US";
                }

                var acceptLanguage = context.Request.Headers["Accept-Language"]
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(acceptLanguage))
                {
                    // Parse "uk-UA;q=0.9,en-US;q=0.8" -> "uk-UA"
                    var culture = acceptLanguage
                        .Split(',')[0]
                        .Split(';')[0]
                        .Trim();

                    if (culture.Length > 0)
                    {
                        logger.LogInformation("Using culture from Accept-Language: {Culture}", culture);
                        return culture;
                    }
                }

                // Fallback
                var systemCulture = System.Globalization.CultureInfo.CurrentCulture.Name ?? "en-US";
                logger.LogInformation("Using system culture: {SystemCulture}", systemCulture);
                return systemCulture;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error determining current culture, using default");
                return "en-US";
            }
        }

    }
}
