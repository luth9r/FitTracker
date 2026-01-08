namespace FitTracker.Application.Interfaces;

public interface ILocalizationService
{
    /// <summary>
    ///     Gets string by key for current culture.
    /// </summary>
    /// <param name="key">Key for localization.</param>
    /// <returns>Localized string.</returns>
    string GetString(string key);

    /// <summary>
    ///     Gets string by key for special culture.
    /// </summary>
    /// <param name="key">Key for localization.</param>
    /// <returns>Localized string.</returns>
    string GetString(string key, string culture);

    /// <summary>
    ///     Gets a list of all available culture codes that have been loaded into the translation service.
    /// </summary>
    /// <returns>
    ///     An enumerable collection of culture codes (e.g., "en-US", "pl-PL", "de-DE")
    ///     that are currently available in the service.
    /// </returns>
    IEnumerable<string> GetAvailableCultures();

    /// <summary>
    ///     Retrieves the culture information for the current context.
    /// </summary>
    /// <returns>
    ///     A string representing the current culture's code (e.g., "en-US").
    /// </returns>
    string GetCurrentCulture();
}