using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Commands.Google;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    /// <summary>
    /// Authentication controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public sealed class AuthController(
        IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginRequest">The <see cref="LoginRequest"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/>. or <see cref="ValidationProblemDetails"/> if the login fails.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
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
        public async Task<IActionResult> GoogleLogin(
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
        public async Task<IActionResult> GoogleRegister(
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
        public async Task<IActionResult> Register(
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
        /// Resends the email verification link.
        /// </summary>
        /// <param name="resendRequest">The <see cref="ResendVerificationRequest"/>.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Success or <see cref="ValidationProblemDetails"/> if the resend fails.</returns>
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerificationEmail(
            [FromBody] ResendVerificationRequest resendRequest,
            CancellationToken cancellationToken)
        {
            var command = new ResendVerificationEmailCommand(resendRequest.Email);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }

            return NoContent();
        }

        /// <summary>
        /// Verifies a user's email address.
        /// </summary>
        /// <param name="token">The verification token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="LoginResponse"/> or <see cref="ValidationProblemDetails"/> if the verification fails.</returns>
        [HttpPost("verify-email")]
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
                return ValidationProblem(result.Error.ToModelState());
            }

            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);
        }

        /// <summary>
        /// Initiates the password reset process by sending a reset link to the user's email.
        /// </summary>
        /// <param name="request">The forgot password request containing the user's email address.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="NoContentResult"/> indicating the request was processed successfully.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var command = new ForgotPasswordCommand(request.Email);

            // Always returns success to prevent user enumeration attacks
            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Resets a user's password using a password reset token.
        /// </summary>
        /// <param name="token">The password reset token from the reset link.</param>
        /// <param name="request">The reset password request containing the new password.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="NoContentResult"/> if successful, or <see cref="ValidationProblemDetails"/> if the reset fails.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromQuery] string token,
            [FromBody] ResetPasswordRequest request,
            CancellationToken cancellationToken)
        {
            var command = new ResetPasswordCommand(request.NewPassword, token);

            var result = await mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }

            return NoContent();
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
