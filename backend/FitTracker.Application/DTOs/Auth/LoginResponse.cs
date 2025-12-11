namespace FitTracker.Application.DTOs.Auth
{
    /// <summary>
    /// DTO for user login response.
    /// </summary>
    /// <param name="Username">Username of the logged in user.</param>
    /// <param name="Email">Email of the logged in user.</param>
    /// <param name="JWT">JWT token for authenticated sessions.</param>
    /// <param name="PreferredUnits">Preferred units for user.</param>
    public sealed record LoginResponse(string Username, string Email, string JWT, string PreferredUnits);
}
