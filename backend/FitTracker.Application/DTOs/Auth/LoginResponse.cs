namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for user login response
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Username of the logged in user
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Email of the logged in user
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// JWT token for authenticated sessions
        /// </summary>
        public string JWT { get; set; } = string.Empty;
    }
}
