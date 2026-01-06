using FitTracker.Domain.Abstract.Interfaces;

namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Base class for all database entities with common properties.
/// </summary>
public abstract class BaseEntityEf : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    ///     Gets or sets unique identifier for the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets timestamp when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <inheritdoc />
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void AddDomainEvent(IDomainEvent de)
    {
        _domainEvents.Add(de);
    }

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
