using FitTracker.Api.Extensions;
using FitTracker.Application.Features.Exercise.Commands.CreateExercise;
using FitTracker.Application.Features.Exercise.Queries.GetExerciseById;
using FitTracker.Application.Features.Exercise.Queries.GetExercises;
using FitTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers;

[Route("api/[controller]")]
public sealed class ExerciseController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetExercises(
        [FromQuery] ExerciseFilterType type = ExerciseFilterType.All,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExercisesQuery(type, CurrentUserId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("{exerciseId:guid}")]
    public async Task<IActionResult> GetExerciseDetailsById(
        [FromRoute] Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExerciseByIdQuery(exerciseId, CurrentUserId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    /// <summary>
    ///     Adds a new exercise to the system.
    /// </summary>
    /// <param name="request">
    ///     The details of the exercise to be added, including name, muscle group, equipment, description,
    ///     and an optional image.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An IActionResult indicating the outcome of the operation. Returns a Created response on success or a
    ///     validation error response otherwise.
    /// </returns>
    [HttpPost("add")]
    public async Task<IActionResult> AddExercise(
        [FromForm] CreateExerciseRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateExerciseCommand(
            request.Name,
            request.MuscleGroup,
            request.Equipment,
            request.Description,
            request.Image,
            CurrentUserId);

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        return Created();
    }
}
