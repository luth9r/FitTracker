using System.Security.Claims;
using FitTracker.Api.Extensions;
using FitTracker.Application.Features.User.Commands.ChangePassword;
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

    /// <summary>
    ///     Changes the password for the current user.
    /// </summary>
    /// <param name="request">
    ///     The request containing the old and new passwords.
    /// </param>
    /// <param name="cancellationToken">
    ///     A token to cancel the operation.
    /// </param>
    /// <returns>A no content response if the operation succeeds, or a validation problem response if it fails.</returns>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(request.OldPassword, request.NewPassword, CurrentUserId);

        var result = await mediator.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return ValidationProblem(result.Error.ToModelState());
        }

        return NoContent();
    }
}
