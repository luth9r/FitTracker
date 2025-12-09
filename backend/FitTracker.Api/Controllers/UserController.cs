using System.Security.Claims;
using FitTracker.Application.UseCases.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(
        IMediator mediator) : BaseApiController
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>The current user.</returns>
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var userId = CurrentUserId;

            return Ok(new { UserId = userId });
        }

        /// <summary>
        /// Gets the stats for the current user.
        /// </summary>
        /// <returns>The <see cref="UserStatsResponse"></see>.</returns>
        [HttpGet("stats")]
        public async Task<IActionResult> GetUserStats()
        {
            var query = new GetUserStatsQuery(CurrentUserId, CurrentUserPreferredUnits);
            var result = await mediator.Send(query);

            return Ok(result.Value);
        }
    }
}
