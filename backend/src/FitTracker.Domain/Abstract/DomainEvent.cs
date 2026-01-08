using FitTracker.Domain.Abstract.Interfaces;

namespace FitTracker.Domain.Abstract;

public abstract record DomainEvent : IDomainEvent
{
    /// <inheritdoc />
    public Guid CorrelationId { get; init; } = Guid.NewGuid();

    /// <inheritdoc />
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}