using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO representing the response from Google's API.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Gets or sets the access token issued by Google.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the lifetime in seconds of the access token.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the scope of the access token.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the ID token issued by Google.
        /// </summary>
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token issued by Google.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
