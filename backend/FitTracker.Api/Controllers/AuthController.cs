using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Commands.Google;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    /// <summary>
    /// Authentication controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        IMediator mediator,
        ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequest">The <see cref="LoginRequest"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/>. or <see cref="ValidationProblemDetails"/> if the login fails.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(
            [FromBody] LoginRequest loginRequest,
            CancellationToken cancellationToken)
        {
            var command = new LoginCommand(loginRequest);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }

            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);
        }

        /// <summary>
        /// Logs in a user using Google credentials.
        /// </summary>
        /// <param name="googleRequest">The <see cref="GoogleLoginRequest"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/> or <see cref="ValidationProblemDetails"/> if the login fails.</returns>
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLoginAsync(
            [FromBody] GoogleLoginRequest googleRequest,
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

        /// <summary>
        /// Registers a new user using Google credentials.
        /// </summary>
        /// <param name="googleRegisterRequest">The <see cref="GoogleRegisterRequest"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/> or <see cref="ValidationProblemDetails"/> if the registration fails.</returns>
        [HttpPost("google-register")]
        public async Task<IActionResult> GoogleRegisterAsync(
            [FromBody] GoogleRegisterRequest googleRegisterRequest,
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerRequest">The <see cref="RegisterRequest"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/> or <see cref="ValidationProblemDetails"/> if the registration fails.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterRequest registerRequest,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterCommand(registerRequest), cancellationToken);
            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Verifies a user's email address.
        /// </summary>
        /// <param name="token">The verification token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/> or <see cref="ValidationProblemDetails"/> if the verification fails.</returns>
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromQuery] string token,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var command = new VerifyEmailCommand(token);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return BadRequest(result.Error.ToModelState());
            }

            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);
        }

        /// <summary>
        /// Sets the authentication cookie.
        /// </summary>
        /// <param name="token">The authentication token (JWT).</param>
        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(30),
                Secure = true,
                SameSite = SameSiteMode.Lax,
            };

            Response.Cookies.Append("auth-token", token, cookieOptions);
        }
    }
}
