using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.BackgroundJobs;

/// <summary>
///     A background service responsible for periodically removing unverified users from
///     the system to maintain database integrity and reduce unnecessary data storage.
/// </summary>
/// <remarks>
///     This service executes at regular intervals and removes user accounts that have
///     not been verified within a defined timeframe. It runs on a daily basis by default.
/// </remarks>
/// <param name="scopeFactory">
///     The <see cref="IServiceScopeFactory" /> used to create a scope for dependency resolution
///     within the lifetime of the background task.
/// </param>
/// <param name="logger">
///     The <see cref="ILogger{UnverifiedUserCleaner}" /> used for logging information, warnings,
///     and errors related to the background service.
/// </param>
public sealed class UnverifiedUserCleaner(
    IServiceScopeFactory scopeFactory,
    ILogger<UnverifiedUserCleaner> logger) : BackgroundService
{
    /// <summary>
    ///     Executes the background task that periodically removes unverified users from the system.
    /// </summary>
    /// <param name="stoppingToken">
    ///     A <see cref="CancellationToken" /> that is triggered when the background service is stopping.
    ///     It can be used to gracefully terminate the task.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous execution of the background task.
    /// </returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("UnverifiedUserCleaner started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RemoveUnverifiedUsersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while removing unverified users.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    /// <summary>
    ///     Removes users from the database who have not verified their email addresses
    ///     within the last 24 hours.
    /// </summary>
    /// <param name="stoppingToken">
    ///     A <see cref="CancellationToken" /> that is triggered to abort the operation if the service is stopping.
    ///     It ensures that the cleanup task terminates gracefully.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation of removing unverified users.
    /// </returns>
    private async Task RemoveUnverifiedUsersAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitTrackerDbContext>();

        var thresholdUtc = DateTime.UtcNow.AddHours(-24);

        // Find and delete users who haven't verified their email address in the last 24 hours'
        var deletedCount = await context.Users
            .Where(u => !u.IsEmailVerified && u.CreatedAt < thresholdUtc)
            .ExecuteDeleteAsync(stoppingToken);

        if (deletedCount > 0)
        {
            logger.LogInformation("Cleanup: Removed {Count} unverified users who exceeded 24h limit.", deletedCount);
        }
    }
}
