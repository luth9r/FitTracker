using System.Globalization;
using FitTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Services;

public class LocalizationService(
    ILocalizationProvider provider,
    IHttpContextAccessor httpContextAccessor,
    ILogger<LocalizationService> logger) : ILocalizationService
{
    /// <inheritdoc />
    public IEnumerable<string> GetAvailableCultures()
    {
        return provider.GetAvailableCultures();
    }

    /// <inheritdoc />
    public string GetString(string key)
    {
        var culture = GetCurrentCulture();
        return provider.GetString(key, culture);
    }

    /// <inheritdoc />
    public string GetString(string key, string culture)
    {
        return provider.GetString(key, culture);
    }

    /// <summary>
    ///     Get current culture from Accept-Language Header.
    /// </summary>
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
                    logger.LogDebug("Using culture from Accept-Language: {Culture}", culture);
                    return culture;
                }
            }

            // Fallback
            var systemCulture = CultureInfo.CurrentCulture.Name ?? "en-US";
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
