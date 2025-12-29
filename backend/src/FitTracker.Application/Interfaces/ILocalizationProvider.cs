using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Interfaces
{
    /// <summary>
    /// Provides localization functionality for retrieving translated strings.
    /// </summary>
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Retrieves a translated string for the specified key and culture.
        /// </summary>
        /// <param name="key">The translation key in dot-notation format.</param>
        /// <param name="culture">The culture code (e.g., "en-US", "uk-UA").</param>
        /// <returns>The translated string if found; otherwise, fallback value.</returns>
        string GetString(string key, string culture);

        /// <summary>
        /// Gets a list of all available culture codes.
        /// </summary>
        /// <returns>An enumerable collection of culture codes.</returns>
        IEnumerable<string> GetAvailableCultures();
    }
}
