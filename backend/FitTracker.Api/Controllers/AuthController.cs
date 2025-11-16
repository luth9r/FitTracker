using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Commands.Google;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator, ILogger<AuthController> logger) : Controller
    {

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Application.DTOs.Auth.LoginRequest loginRequest, CancellationToken cancellationToken)
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
                var registrationError = result.Error.Errors
                    .FirstOrDefault(e => e.ErrorMessage.StartsWith("NEEDS_REGISTRATION::"));

                if (registrationError != null)
                {
                    var parts = registrationError.ErrorMessage.Split("::");

                    var responseData = new
                    {
                        needsRegistration = true,
                        email = parts.ElementAtOrDefault(1),
                        firstName = parts.ElementAtOrDefault(2),
                        lastName = parts.ElementAtOrDefault(3)
                    };
                    return Ok(responseData);
                }

                return ValidationProblem(result.Error.ToModelState());
            }
            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);
        }

        [HttpPost("complete-google-registration")]
        public async Task<IActionResult> CompleteGoogleRegistrationAsync([FromBody] CompleteGoogleRegistrationRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CompleteGoogleRegistrationCommand(request);
            var result = await mediator.Send(command, cancellationToken);
            if (result.IsFailure)
            {
                return ValidationProblem(result.Error.ToModelState());
            }
            SetAuthCookie(result.Value.JWT);
            return Ok(result.Value);

        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Application.DTOs.Auth.RegisterRequest registerRequest,
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
