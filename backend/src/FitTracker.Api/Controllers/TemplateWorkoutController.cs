using FitTracker.Api.Extensions;
using FitTracker.Application.Features.WorkoutTemplate.Commands.CreateWorkoutTemplate;
using FitTracker.Application.Features.WorkoutTemplate.Queries.GetWorkoutTemplate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers;

[Route("api/[controller]")]
public sealed class TemplateWorkoutController(IMediator mediator) : BaseApiController
{
    /// <summary>
    ///     Creates a new workout template.
    /// </summary>
    /// <param name="request">
    ///     The information required to create a new workout template, including name, description, and
    ///     exercises.
    /// </param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An IActionResult representing the result of the operation. Returns a CreatedAtAction result with the newly created
    ///     workout template's ID if successful, or a ValidationProblem response if errors occur.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTemplateWorkoutRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTemplateWorkoutCommand(
            CurrentUserId,
            request.Name,
            request.Description,
            request.Exercises);

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        return CreatedAtAction(
            nameof(GetTemplate),
            new { id = result.Value },
            result.Value);
    }

    /// <summary>
    ///     Retrieves a workout template by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the workout template to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     An IActionResult representing the result of the operation. Returns the workout template if found, or a
    ///     NotFound response if not.
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTemplate(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTemplateWorkoutQuery(id, CurrentUserId);

        var result = await mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }
}
