namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for user login response.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Gets or sets username of the logged in user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets email of the logged in user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets JWT token for authenticated sessions.
        /// </summary>
        public string JWT { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets preferred units for user.
        /// </summary>
        public string PreferredUnits { get; set; } = string.Empty;
    }
}
