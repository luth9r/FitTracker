using FitTracker.Application.UseCases.Exercise.Queries;
using FitTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    [Route("api/[controller]")]
    public sealed class ExerciseController(
        IMediator mediator) : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetExercisesAsync([FromQuery] ExerciseFilterType type = ExerciseFilterType.All, CancellationToken cancellationToken = default)
        {
            var query = new GetExerciseQuery(type, CurrentUserId);
            var result = await mediator.Send(query, cancellationToken);

            return Ok(result.Value);
        }

        [HttpGet("{exerciseId:guid}")]
        public async Task<IActionResult> GetExerciseByIdAsync([FromRoute] Guid exerciseId, CancellationToken cancellationToken = default)
        {
            var query = new GetExerciseByIdQuery(exerciseId, CurrentUserId);
            var result = await mediator.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
