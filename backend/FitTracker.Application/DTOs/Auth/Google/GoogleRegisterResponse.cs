using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO for Google registration response.
    /// </summary>
    public class GoogleRegisterResponse
    {
        /// <summary>
        /// Gets or sets user's email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user's first name.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets user's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;
    }
}
