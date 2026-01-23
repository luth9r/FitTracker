using System.Threading.Channels;

namespace FitTracker.Infrastructure.Persistence;

/// <summary>
///     Represents a signal mechanism to communicate events for the outbox pattern.
/// </summary>
/// <remarks>
///     The <see cref="OutboxSignal" /> is designed to manage communication between the
///     database context and background services, such as <c>OutboxProcessor</c>.
///     Producers can use the <see cref="Writer" /> property to signal new events,
///     while consumers can utilize the <see cref="Reader" /> property to process incoming signals.
/// </remarks>
public sealed class OutboxSignal
{
    /// <summary>
    ///     A private channel used for communication of boolean signals
    ///     within the outbox mechanism.
    /// </summary>
    private readonly Channel<bool> _channel = Channel.CreateUnbounded<bool>();

    /// <summary>
    ///     Gets the channel writer used to signal new events.
    /// </summary>
    public ChannelWriter<bool> Writer => _channel.Writer;

    /// <summary>
    ///     Gets the channel reader used to process incoming signals.
    /// </summary>
    public ChannelReader<bool> Reader => _channel.Reader;
}
