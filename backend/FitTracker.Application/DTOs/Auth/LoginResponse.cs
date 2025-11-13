namespace FitTracker.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string JWT { get; set; }
    }
}
