using FitTracker.Application.Interfaces;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Mvc;

namespace FitTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController(
        FitTrackerDbContext context,
        ILocalizationService localization,
        ILogger<HealthController> logger) : ControllerBase
    {
        [HttpGet("db-connection")]
        public async Task<IActionResult> CheckDatabaseConnection(
            CancellationToken cancellationToken)
        {
            try
            {
                var canConnect = await context.Database.CanConnectAsync(cancellationToken);

                if (canConnect)
                {
                    logger.LogInformation("Database connection successful");
                    return Ok(new
                    {
                        status = localization.GetString("Health.Status.Ok"),
                        connected = true,
                        timestamp = DateTime.UtcNow,
                    });
                }
                else
                {
                    logger.LogWarning("Database connection failed");
                    return StatusCode(503, new
                    {
                        status = localization.GetString("Health.Status.Failed"),
                        connected = false,
                        timestamp = DateTime.UtcNow,
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking database: {ErrorMessage}", ex.Message);
                return StatusCode(503, new
                {
                    status = localization.GetString("Health.Status.Error"),
                    message = ex.Message,
                    timestamp = DateTime.UtcNow,
                });
            }
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok(new
            {
                status = localization.GetString("Health.Live"),
                timestamp = DateTime.UtcNow,
            });
        }
    }
}
