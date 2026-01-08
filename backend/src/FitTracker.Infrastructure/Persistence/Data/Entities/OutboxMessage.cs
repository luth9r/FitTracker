namespace FitTracker.Infrastructure.Persistence.Data.Entities;

/// <summary>
///     Represents a message intended for use with the outbox pattern, facilitating reliable communication between systems
///     by storing event data that can be eventually processed and dispatched.
/// </summary>
public class OutboxMessage
{
    public Guid Id { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime OccurredOnUtc { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}