using System.Text.Json;
using FitTracker.Application.Interfaces;

namespace FitTracker.Infrastructure.Localization
{
    public class JsonLocalizationProvider : ILocalizationProvider
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new();

        public JsonLocalizationProvider()
        {
            LoadTranslations();
        }

        /// <inheritdoc/>
        public string GetString(string key, string culture = "en-US")
        {
            if (_translations.TryGetValue(culture, out var cultureDictionary))
            {
                if (cultureDictionary.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            // Fallback to English if nothing found
            if (_translations.TryGetValue("en-US", out var enDictionary))
            {
                if (enDictionary.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            return key; // Return key
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetAvailableCultures()
        {
            return _translations.Keys;
        }

        /// <summary>
        /// Load translations from storage.
        /// </summary>
        private void LoadTranslations()
        {
            var localizationPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Localization");

            var files = new[] { "en-US.json", "uk-UA.json" };

            foreach (var file in files)
            {
                var filePath = Path.Combine(localizationPath, file);
                if (File.Exists(filePath))
                {
                    var language = file.Replace(".json", string.Empty);
                    var json = File.ReadAllText(filePath);
                    var flat = FlattenJson(JsonDocument.Parse(json).RootElement);

                    _translations[language] = flat;
                }
            }
        }

        /// <summary>
        /// Flattens a nested JSON structure into a flat dictionary with dot-notation keys.
        /// This method recursively traverses the JSON tree and creates composite keys for nested properties.
        /// </summary>
        /// <param name="element">The JSON element to flatten.</param>
        /// <param name="prefix">The current key prefix for nested properties (used during recursion).</param>
        /// <returns>A dictionary where keys are dot-separated paths and values are the corresponding JSON string values.</returns>
        /// <example>
        /// Input JSON:
        /// <code>
        /// {
        ///   "common": {
        ///     "buttons": {
        ///       "save": "Save",
        ///       "cancel": "Cancel"
        ///     },
        ///     "greeting": "Hello"
        ///   },
        ///   "title": "Welcome"
        /// }
        /// </code>
        ///
        /// Output Dictionary:
        /// <code>
        /// {
        ///   ["common.buttons.save"] = "Save",
        ///   ["common.buttons.cancel"] = "Cancel",
        ///   ["common.greeting"] = "Hello",
        ///   ["title"] = "Welcome"
        /// }
        /// </code>
        ///
        /// Usage:
        /// <code>
        /// var jsonDoc = JsonDocument.Parse(jsonString);
        /// var flattened = FlattenJson(jsonDoc.RootElement);
        /// // Access: flattened["common.buttons.save"] returns "Save"
        /// </code>
        private Dictionary<string, string> FlattenJson(JsonElement element, string prefix = "")
        {
            var result = new Dictionary<string, string>();

            foreach (var property in element.EnumerateObject())
            {
                var key = string.IsNullOrEmpty(prefix)
                    ? property.Name
                    : $"{prefix}.{property.Name}";

                if (property.Value.ValueKind == JsonValueKind.Object)
                {
                    var nested = FlattenJson(property.Value, key);
                    foreach (var kvp in nested)
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
                else
                {
                    result[key] = property.Value.GetString() ?? string.Empty;
                }
            }

            return result;
        }
    }
}
