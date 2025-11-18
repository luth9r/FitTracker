namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for user registration requests.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the username for registration.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email for registration.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for registration.
        /// </summary>
        public string Password { get; set; } = string.Empty;

    }
}
