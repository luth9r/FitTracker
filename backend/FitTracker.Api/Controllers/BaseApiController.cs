using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    /// <summary>
    /// Base API controller for authenticated requests.
    /// </summary>
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        protected Guid CurrentUserId
        {
            get
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new InvalidOperationException(
                        "User ID is missing from claims, but authorization passed.");
                }

                return Guid.Parse(id);
            }
        }

        /// <summary>
        /// Gets the current user preferred units.
        /// </summary>
        protected string CurrentUserPreferredUnits
        {
            get
            {
                return User.FindFirstValue("preferred-units") ?? "metric";
            }
        }
    }
}
