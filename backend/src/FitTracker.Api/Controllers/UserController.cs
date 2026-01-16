using System.Security.Claims;
using FitTracker.Application.Features.User.Queries.GetRecentWorkouts;
using FitTracker.Application.Features.User.Queries.GetUserStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers;

[Route("api/[controller]")]
public sealed class UserController(IMediator mediator) : BaseApiController
{
    /// <summary>
    ///     Gets the current user.
    /// </summary>
    /// <returns>The current user.</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = CurrentUserId;

        var username = User.FindFirstValue(ClaimTypes.Name);
        var email = User.FindFirstValue(ClaimTypes.Email);

        return Ok(new { UserId = userId, Username = username, Email = email });
    }

    /// <summary>
    ///     Gets the stats for the current user.
    /// </summary>
    /// <returns>The <see cref="UserStatsResponse"></see>.</returns>
    [HttpGet("stats")]
    public async Task<IActionResult> GetUserStats(CancellationToken cancellationToken)
    {
        var query = new GetUserStatsQuery(CurrentUserId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [HttpGet("workouts/recent")]
    public async Task<IActionResult> GetRecentWorkoutsForUser(
        CancellationToken cancellationToken,
        [FromQuery] int take = 5)
    {
        var query = new GetRecentWorkoutsQuery(CurrentUserId, take);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}
