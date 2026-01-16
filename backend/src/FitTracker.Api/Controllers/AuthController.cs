using FitTracker.Api.Extensions;
using FitTracker.Application.Features.User.Commands.ForgotPassword;
using FitTracker.Application.Features.User.Commands.GoogleLogin;
using FitTracker.Application.Features.User.Commands.GoogleMobileAuth;
using FitTracker.Application.Features.User.Commands.GoogleRegister;
using FitTracker.Application.Features.User.Commands.Login;
using FitTracker.Application.Features.User.Commands.Register;
using FitTracker.Application.Features.User.Commands.ResendVerificationEmail;
using FitTracker.Application.Features.User.Commands.ResetPassword;
using FitTracker.Application.Features.User.Commands.VerifyEmail;
using FitTracker.Application.Features.User.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace FitTracker.Api.Controllers;

/// <summary>
///     Authentication controller.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("auth-policy")]
[AllowAnonymous]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Logs in a user.
    /// </summary>
    /// <param name="loginRequest">The <see cref="LoginRequest" />.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="LoginResponse" />. or <see cref="ValidationProblemDetails" /> if the login fails.</returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest loginRequest,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(loginRequest.Email, loginRequest.Password);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        SetAuthCookie(result.Value.Jwt);
        return Ok(result.Value);
    }

    /// <summary>
    ///     Logs in a user using Google credentials.
    /// </summary>
    /// <param name="googleRequest">The <see cref="GoogleLoginRequest" />.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="LoginResponse" /> or <see cref="ValidationProblemDetails" /> if the login fails.</returns>
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin(
        [FromBody] GoogleLoginRequest googleRequest,
        CancellationToken cancellationToken)
    {
        var command = new GoogleLoginCommand(googleRequest.Code, googleRequest.CodeVerifier);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        SetAuthCookie(result.Value.Jwt);
        return Ok(result.Value);
    }

    /// <summary>
    ///     Registers a new user using Google credentials.
    /// </summary>
    /// <param name="googleRegisterRequest">The <see cref="GoogleRegisterRequest" />.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="LoginResponse" /> or <see cref="ValidationProblemDetails" /> if the registration fails.</returns>
    [HttpPost("google-register")]
    public async Task<IActionResult> GoogleRegister(
        [FromBody] GoogleRegisterRequest googleRegisterRequest,
        CancellationToken cancellationToken)
    {
        var command = new GoogleRegisterCommand(googleRegisterRequest.Code, googleRegisterRequest.CodeVerifier);
        var result = await mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        SetAuthCookie(result.Value.Jwt);
        return Ok(result.Value);
    }

    /// <summary>
    ///     Authenticates a user via Google Mobile authentication.
    /// </summary>
    /// <param name="googleMobileAuthRequest">
    ///     The <see cref="GoogleMobileAuthRequest" /> containing the Google authentication
    ///     code.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     The <see cref="LoginResponse" /> containing the authentication details, or
    ///     <see cref="ValidationProblemDetails" /> if authentication fails.
    /// </returns>
    [HttpPost("mobile-google-auth")]
    public async Task<IActionResult> MobileGoogleAuth(
        [FromBody] GoogleMobileAuthRequest googleMobileAuthRequest,
        CancellationToken cancellationToken)
    {
        var command = new GoogleMobileAuthCommand(googleMobileAuthRequest.Code);
        var result = await mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        SetAuthCookie(result.Value.Jwt);
        return Ok(result.Value);
    }

    /// <summary>
    ///     Registers a new user.
    /// </summary>
    /// <param name="registerRequest">The <see cref="RegisterRequest" />.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="LoginResponse" /> or <see cref="ValidationProblemDetails" /> if the registration fails.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new RegisterCommand(registerRequest.Username, registerRequest.Email, registerRequest.Password),
            cancellationToken);
        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        return Ok(result.Value);
    }

    /// <summary>
    ///     Resends the email verification link.
    /// </summary>
    /// <param name="resendEmailRequest">The <see cref="ResendVerificationEmailRequest" />.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Success or <see cref="ValidationProblemDetails" /> if the resend fails.</returns>
    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerificationEmail(
        [FromBody] ResendVerificationEmailRequest resendEmailRequest,
        CancellationToken cancellationToken)
    {
        var command = new ResendVerificationEmailCommand(resendEmailRequest.Email);
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        return NoContent();
    }

    /// <summary>
    ///     Verifies a user's email address.
    /// </summary>
    /// <param name="token">The verification token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="LoginResponse" /> or <see cref="ValidationProblemDetails" /> if the verification fails.</returns>
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

        SetAuthCookie(result.Value.Jwt);
        return Ok(result.Value);
    }

    /// <summary>
    ///     Initiates the password reset process by sending a reset link to the user's email.
    /// </summary>
    /// <param name="request">The forgot password request containing the user's email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="NoContentResult" /> indicating the request was processed successfully.</returns>
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
    ///     Resets a user's password using a password reset token.
    /// </summary>
    /// <param name="token">The password reset token from the reset link.</param>
    /// <param name="request">The reset password request containing the new password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="NoContentResult" /> if successful, or <see cref="ValidationProblemDetails" /> if the reset fails.</returns>
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
    ///     Sets the authentication cookie.
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
