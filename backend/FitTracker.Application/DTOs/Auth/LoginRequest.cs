using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for user login request.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user's password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
