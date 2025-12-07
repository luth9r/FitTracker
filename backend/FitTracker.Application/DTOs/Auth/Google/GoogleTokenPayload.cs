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
        /// Gets the unique identifier for the Google user.
        /// </summary>
        public string GoogleId { get; init; } = default!;

        /// <summary>
        /// Gets the email address of the Google user.
        /// </summary>
        public string Email { get; init; } = default!;

        /// <summary>
        /// Gets the first name of the Google user.
        /// </summary>
        public string FirstName { get; init; } = default!;

        /// <summary>
        /// Gets the last name of the Google user.
        /// </summary>
        public string LastName { get; init; } = default!;
    }
}
