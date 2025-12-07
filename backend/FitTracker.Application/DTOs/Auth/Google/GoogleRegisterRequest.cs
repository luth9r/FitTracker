using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO for Google registration request.
    /// </summary>
    public class GoogleRegisterRequest
    {
        /// <summary>
        /// Gets or sets authorization code received from Google.
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// Gets or sets code verifier for PKCE flow.
        /// </summary>
        public string CodeVerifier { get; set; } = default!;
    }
}
