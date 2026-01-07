using FitTracker.Infrastructure.Persistence;
using FitTracker.Infrastructure.Persistence.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitTracker.Infrastructure.BackgroundJobs;

/// <summary>
///     Background service responsible for processing outbox messages from the database in batches.
/// </summary>
/// <param name="scopeFactory">The <see cref="IServiceScopeFactory" />.</param>
/// <param name="logger">The <see cref="ILogger{OutboxProcessor}" />.</param>
public sealed class OutboxProcessor(
    IServiceScopeFactory scopeFactory,
    OutboxSignal signal,
    ILogger<OutboxProcessor> logger) : BackgroundService
{
    /// <summary>
    ///     Executes the background processing logic for the outbox messages in a loop until the service is stopped.
    /// </summary>
    /// <param name="stoppingToken">
    ///     A <see cref="CancellationToken" /> that can be used to stop the background service
    ///     execution.
    /// </param>
    /// <return>
    ///     A <see cref="Task" /> that represents the asynchronous operation of the background processing logic.
    /// </return>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("OutboxProcessor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing outbox messages.");
            }

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));
            try
            {
                await signal.Reader.WaitToReadAsync(timeoutCts.Token);
            }
            catch (OperationCanceledException)
            {
                // Everything is fine, we just timed out
            }
        }
    }

    /// <summary>
    ///     Processes outbox messages in batches, deserializing and dispatching events for each unprocessed message.
    /// </summary>
    /// <param name="stoppingToken">A <see cref="CancellationToken" /> to signal the operation should be canceled.</param>
    /// <return>
    ///     A <see cref="Task" /> that represents the asynchronous operation of processing outbox messages.
    /// </return>
    private async Task ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitTrackerDbContext>();

        // Use MassTransit to publish events to RabbitMQ
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        // Take 20 messages from the database
        var messages = await context.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(20)
            .ToListAsync(stoppingToken);

        if (messages.Count == 0)
        {
            return;
        }

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonConvert.DeserializeObject(
                    message.Content,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                if (domainEvent != null)
                {
                    // Publish the event to RabbitMQ
                    await publishEndpoint.Publish(domainEvent, stoppingToken);
                }

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while processing outbox message with Id: {MessageId}",
                    message.Id);
                message.Error = ex.Message;
            }
        }

        await context.SaveChangesAsync(stoppingToken);
    }
}
