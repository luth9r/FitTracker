using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers.Extensions;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    }
}
