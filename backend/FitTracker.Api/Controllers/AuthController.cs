using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.UseCases.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator, ILogger<AuthController> logger) : Controller
    {

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest,
        CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterCommand(registerRequest), cancellationToken);
            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }
            return Ok(result.Value);

        }


        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var command = new VerifyEmailCommand(token);
            var result = await mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error.Errors.Select(e => e.ErrorMessage));
            }

            var loginResponse = result.Value;
            var loginToken = loginResponse.JWT;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddYears(1),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("auth-token", loginToken, cookieOptions);


            return Ok(result.Value);
        }
    }
}
