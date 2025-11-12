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
        public async Task<IResult> RegisterAsync(RegisterDto registerRequest, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new RegisterCommand(registerRequest));
            if (result.IsFailure)
            {
                return result.Error.ValidationProblem();
            }

            return Results.Ok(result.Value);

        }
    }
}
