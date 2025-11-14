using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.UseCases.User.Commands;
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
            // Предполагаем, что у вас есть LoginCommand, который принимает LoginRequest
            var command = new LoginCommand(loginRequest);
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                // Ошибка аутентификации (неверный логин/пароль)
                // Возвращаем ValidationProblem, как и в RegisterAsync, для единообразия
                return ValidationProblem(result.Error.ToModelState());
            }

            // Предполагаем, что result.Value - это LoginResponse, содержащий JWT
            var loginResponse = result.Value;
            var loginToken = loginResponse.JWT;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Важно: Куки недоступны из JavaScript на клиенте
                Expires = DateTime.UtcNow.AddDays(30), // Устанавливаем разумное время жизни
                Secure = true, // Передача только по HTTPS
                SameSite = SameSiteMode.Strict // Защита от CSRF
            };

            Response.Cookies.Append("auth-token", loginToken, cookieOptions);

            // Возвращаем ответ. Клиенту не обязательно видеть JWT (он в куки),
            // но мы можем вернуть DTO с информацией о пользователе.
            return Ok(loginResponse);
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(Application.DTOs.Auth.RegisterRequest registerRequest,
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
