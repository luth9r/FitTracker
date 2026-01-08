namespace FitTracker.Domain.Abstract.Interfaces;

public interface IHasDomainEvents
{
    /// <summary>
    ///     Gets the collection of domain events associated with the implementing entity.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    ///     Adds a domain event to the entity's collection of domain events.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    ///     Clears all domain events from the entity's collection.
    /// </summary>
    void ClearDomainEvents();
}