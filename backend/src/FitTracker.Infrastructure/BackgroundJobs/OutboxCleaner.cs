using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.BackgroundJobs;

/// <summary>
///     Background service responsible for periodically cleaning old messages from the outbox in the database.
/// </summary>
/// <remarks>
///     This service runs as a hosted background task, iterating at a fixed interval and performing cleanup
///     operations on old, processed outbox messages to maintain system performance and prevent unbounded growth
///     of stored data.
/// </remarks>
/// <param name="scopeFactory">
///     The <see cref="IServiceScopeFactory" /> used to create service scopes for accessing scoped
///     dependencies.
/// </param>
/// <param name="logger">
///     The <see cref="ILogger{OutboxCleaner}" /> used for logging information and errors during
///     execution.
/// </param>
public sealed class OutboxCleaner(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxCleaner> logger) : BackgroundService
{
    /// <summary>
    ///     Executes the periodic background task for cleaning old messages from the outbox in the database.
    /// </summary>
    /// <param name="stoppingToken">
    ///     A <see cref="CancellationToken" /> that is triggered when the background service is stopping, allowing
    ///     the operation to be gracefully canceled.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation, which performs cleanup of old, processed
    ///     outbox messages on a regular interval.
    /// </returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("OutboxCleaner started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanOldMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while cleaning outbox messages.");
            }

            // Wait for next iteration
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    /// <summary>
    ///     Cleans up old, processed outbox messages from the database that exceed the configured retention period.
    /// </summary>
    /// <param name="stoppingToken">
    ///     A <see cref="CancellationToken" /> that can be triggered to stop the cleanup operation
    ///     gracefully, ensuring resources are released properly.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation that deletes old outbox messages
    ///     and logs the cleanup action.
    /// </returns>
    private async Task CleanOldMessagesAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitTrackerDbContext>();

        // Check for messages older than 7 days
        var thresholdUtc = DateTime.UtcNow.AddDays(-7);

        // Use EF Core to delete old messages
        var deletedCount = await context.OutboxMessages
            .Where(m => m.ProcessedOnUtc != null && m.ProcessedOnUtc < thresholdUtc)
            .ExecuteDeleteAsync(stoppingToken);

        if (deletedCount > 0)
        {
            logger.LogInformation("Cleaned up {Count} old outbox messages from the database.", deletedCount);
        }
    }
}