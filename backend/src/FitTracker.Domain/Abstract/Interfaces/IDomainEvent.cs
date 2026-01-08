namespace FitTracker.Domain.Abstract.Interfaces;

/// <summary>
///     Marker interface for domain events.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    ///     Gets the unique identifier used to correlate multiple related domain events.
    /// </summary>
    Guid CorrelationId { get; }

    /// <summary>
    ///     Gets the date and time at which the domain event occurred.
    /// </summary>
    DateTime OccurredOnUtc { get; }
}