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
        public async Task<ActionResult<RegisterResponse>> RegisterAsync(RegisterRequest registerRequest,
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
        public async Task<ActionResult<LoginResponse>> VerifyEmail([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var command = new VerifyEmailCommand(token);
            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error.Errors.Select(e => e.ErrorMessage));
        }
    }
}
