using System.Collections.Concurrent;
using System.Reflection;
using FitTracker.Application.Constants;
using FitTracker.Application.Interfaces;
using FitTracker.Domain.Constants;
using MediatR;
using ResultExtensions = FitTracker.Application.Extensions.ResultExtensions;

namespace FitTracker.Application.Behaviors;

/// <summary>
///     Represents a rate-limiting behavior for MediatR pipeline handling.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
/// <param name="rateLimitService">The rate limit service.</param>
public sealed class RateLimitBehavior<TRequest, TResponse>(IRateLimitService rateLimitService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRateLimitedRequest
{
    /// <summary>
    ///     A static cache that stores mappings between types and their associated
    ///     <see cref="MethodInfo" /> objects for improved performance during repeated
    ///     reflective operations.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, MethodInfo> _methodCache = new();

    /// <summary>
    ///     Handles the rate-limiting behavior for the current request in the MediatR pipeline.
    /// </summary>
    /// <param name="request">The incoming request object that implements the <see cref="IRateLimitedRequest" /> interface.</param>
    /// <param name="next">The next delegate to be executed in the pipeline.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     The response of type <typeparamref name="TResponse" />. If the rate limit is exceeded, a validation failure
    ///     response is returned.
    /// </returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var key = request.GetRateLimitKey();
        var requestName = typeof(TRequest).Name;

        if (!await rateLimitService.IsAllowedAsync(key, request.GetLimitPeriod()))
        {
            Console.WriteLine($"[DEBUG] Rate limit exceeded for {requestName}. Key: {key}");
            var resultType = typeof(TResponse);

            var method = _methodCache.GetOrAdd(
                resultType,
                type =>
                {
                    Console.WriteLine($"[DEBUG] Reflecting ValidationFailure for {type.FullName}");

                    var args = type.GetGenericArguments();
                    if (args.Length == 0)
                    {
                        throw new InvalidOperationException(
                            $"Type {type.Name} has no generic arguments. Expected Result<T, E>.");
                    }

                    var valueType = args[0];
                    Console.WriteLine($"[DEBUG] Extracted ValueType: {valueType.Name}");

                    var mi = typeof(ResultExtensions)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .FirstOrDefault(m =>
                            m.Name == nameof(ResultExtensions.ValidationFailure) &&
                            m.IsGenericMethodDefinition &&
                            m.GetParameters().Length == 2 &&
                            m.GetParameters()[0].ParameterType == typeof(string) &&
                            m.GetParameters()[1].ParameterType == typeof(string));

                    if (mi == null)
                    {
                        throw new InvalidOperationException(
                            "Could not find method ResultExtensions.ValidationFailure(string, string)");
                    }

                    var closedMethod = mi.MakeGenericMethod(valueType);
                    Console.WriteLine($"[DEBUG] Created closed generic method: {closedMethod.Name}<{valueType.Name}>");

                    return closedMethod;
                });

            if (method == null)
            {
                throw new InvalidOperationException("Could not find ValidationFailure method.");
            }

            var errorResult = method?.Invoke(null, [ErrorKeys.General, DomainErrors.User.RateLimitExceeded]);

            return (TResponse)errorResult!;
        }

        return await next(cancellationToken);
    }
}
