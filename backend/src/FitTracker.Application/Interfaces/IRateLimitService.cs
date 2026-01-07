namespace FitTracker.Application.Interfaces;

public interface IRateLimitService
{
    /// <summary>
    ///     Determines whether the operation associated with the given key is allowed, based on the rate limiting policy.
    /// </summary>
    /// <param name="key">The unique identifier for tracking the resource or operation being rate limited.</param>
    /// <param name="expiration">The time period after which the rate limit for the given key resets.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a boolean value indicating whether
    ///     the operation is allowed.
    /// </returns>
    Task<bool> IsAllowedAsync(string key, TimeSpan expiration);
}
