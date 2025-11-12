using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(ILogger<AuthController> logger) : Controller
    {
        public class RegisterRequest
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        //{

        //}
    }
}
