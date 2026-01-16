using System.Diagnostics;
using FitTracker.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FitTracker.Application.Behaviors;

/// <summary>
///     Logs the execution time and outcome of MediatR requests.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
/// <param name="logger">The logger.</param>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    ///     Handles the logging of request execution.
    /// </summary>
    /// <param name="request">The <see cref="IRequest{TResponse}" />.</param>
    /// <param name="next">The <see cref="RequestHandlerDelegate{TResponse}" />.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
    /// <returns>The <typeparamref name="TResponse" />.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetFormattedName();

        logger.LogInformation("[START] Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();
        TResponse response;

        try
        {
            response = await next(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            stopwatch.Stop();
            logger.LogWarning(
                "[CANCELLED] Handling {RequestName} was cancelled after {TimeMs}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
                ex,
                "[FAILURE] Handling {RequestName} threw an exception after {TimeMs}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);
            throw;
        }

        stopwatch.Stop();
        var timeTaken = stopwatch.ElapsedMilliseconds;

        LogResult(response, requestName, timeTaken);

        return response;
    }

    private void LogResult(TResponse response, string requestName, long timeTaken)
    {
        if (response == null)
        {
            logger.LogInformation(
                "[END] Handled {RequestName} successfully (null response) in {TimeMs}ms",
                requestName,
                timeTaken);
            return;
        }

        try
        {
            var type = response.GetType();

            var isFailure = type.GetProperty("IsFailure")?.GetValue(response) as bool? ?? false;

            if (isFailure)
            {
                var error = type.GetProperty("Error")?.GetValue(response);

                logger.LogWarning(
                    "[FAILURE] Handling {RequestName} failed after {TimeMs}ms. Error: {Error}",
                    requestName,
                    timeTaken,
                    error ?? "Unknown error");
            }
            else
            {
                logger.LogInformation(
                    "[END] Handled {RequestName} successfully in {TimeMs}ms",
                    requestName,
                    timeTaken);
            }
        }
        catch (Exception ex)
        {
            logger.LogTrace(ex, "Failed to reflect result properties for {RequestName}", requestName);
            logger.LogInformation("[END] Handled {RequestName} in {TimeMs}ms", requestName, timeTaken);
        }
    }
}
