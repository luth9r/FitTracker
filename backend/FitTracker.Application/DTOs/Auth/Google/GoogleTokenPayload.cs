using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.DTOs.Auth.Google
{
    /// <summary>
    /// DTO representing the payload of a Google authentication token.
    /// </summary>
    public class GoogleTokenPayload
    {
        /// <summary>
        /// Gets or sets the unique identifier for the Google user.
        /// </summary>
        public string GoogleId { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the Google user.
        /// </summary>
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the Google user.
        /// </summary>
        public string FirstName { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the Google user.
        /// </summary>
        public string LastName { get; init; } = string.Empty;
    }
}
