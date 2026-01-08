using FitTracker.Application.Interfaces;
using StackExchange.Redis;

namespace FitTracker.Infrastructure.Services;

/// <summary>
///     A service responsible for handling rate limiting using Redis as the underlying data store.
/// </summary>
/// <param name="redis">The <see cref="IConnectionMultiplexer" /> instance.</param>
public sealed class RedisRateLimitService(IConnectionMultiplexer redis) : IRateLimitService
{
    /// <inheritdoc />
    public async Task<bool> IsAllowedAsync(string key, TimeSpan expiration)
    {
        var db = redis.GetDatabase();

        // Set if Not Exists
        return await db.StringSetAsync(key, "locked", expiration, When.NotExists);
    }
}