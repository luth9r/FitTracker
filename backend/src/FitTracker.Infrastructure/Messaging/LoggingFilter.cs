using MassTransit;
using Microsoft.Extensions.Logging;

namespace FitTracker.Infrastructure.Messaging;

/// <summary>
///     Represents a filter that logs message-specific context information during the consume pipeline in MassTransit.
/// </summary>
/// <typeparam name="T">The type of the message being consumed.</typeparam>
public class LoggingFilter<T>(ILogger<LoggingFilter<T>> logger) : IFilter<ConsumeContext<T>>
    where T : class
{
    /// <summary>
    ///     Sends the message through the consume pipeline after logging relevant context information.
    /// </summary>
    /// <param name="context">The context that contains the message and associated metadata.</param>
    /// <param name="next">The next filter or pipe in the consume pipeline.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        using (logger.BeginScope(
                   new Dictionary<string, object>
                   {
                       ["CorrelationId"] = context.CorrelationId ?? Guid.Empty,
                       ["MessageType"] = typeof(T).Name,
                   }))
        {
            await next.Send(context);
        }
    }

    /// <summary>
    ///     Probes the filter by adding its details to the provided probe context.
    /// </summary>
    /// <param name="context">The context used to record diagnostic information about the filter.</param>
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("logging");
    }
}
