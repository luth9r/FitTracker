namespace FitTracker.Application.Interfaces;

/// <summary>
///     Represents an interface for handling rate-limited requests.
/// </summary>
public interface IRateLimitedRequest
{
    /// <summary>
    ///     Gets the rate limit key.
    /// </summary>
    string GetRateLimitKey();

    /// <summary>
    ///     Gets the limit period.
    /// </summary>
    TimeSpan GetLimitPeriod();
}
