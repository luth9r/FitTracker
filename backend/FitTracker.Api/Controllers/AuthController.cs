using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Application.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator, ILogger<AuthController> logger) : ControllerBase
    {
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(loginRequest);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }

            var loginResponse = result.Value;
            var loginToken = loginResponse.JWT;

            SetAuthCookie(loginToken);

            return Ok(loginResponse);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLoginAsync([FromBody] GoogleLoginRequest googleRequest,
            CancellationToken cancellationToken)
        {
            var command = new GoogleLoginCommand(googleRequest);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }
            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);
        }

        [HttpPost("google-register")]
        public async Task<IActionResult> GoogleRegisterAsync([FromBody] GoogleRegisterRequest googleRegisterRequest,
            CancellationToken cancellationToken)
        {
            var command = new GoogleRegisterCommand(googleRegisterRequest);
            var result = await mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }
            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest registerRequest,
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

        /// <summary>
        /// Sets the authentication cookie
        /// </summary>
        /// <param name="token"></param>
        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(30),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("auth-token", token, cookieOptions);
        }
    }
}
