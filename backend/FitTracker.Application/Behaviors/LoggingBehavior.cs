using CSharpFunctionalExtensions;
using FitTracker.Application.Extensions.MedaitR;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FitTracker.Application.Behaviors
{
    /// <summary>
    /// Logs the execution time and outcome of MediatR requests.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="logger"></param>
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles the logging of request execution.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            var requestName = request.GetFormattedName();

            logger.LogInformation("[START] Handling {RequestName}", requestName);

            var stopwatch = Stopwatch.StartNew();
            TResponse response;

            try
            {
                response = await next();
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, "[FAILURE] Handling {RequestName} threw an exception after {TimeMs}ms", requestName, stopwatch.ElapsedMilliseconds);
                throw;
            }

            stopwatch.Stop();
            var timeTaken = stopwatch.ElapsedMilliseconds;

            if (response is Result result && result.IsFailure)
            {
                logger.LogWarning("[FAILURE] Handling {RequestName} failed after {TimeMs}ms. Error: {Error}", requestName, timeTaken, result.Error);
            }
            else
            {
                logger.LogInformation("[END] Handled {RequestName} successfully in {TimeMs}ms", requestName, timeTaken);
            }

            return response;
        }
    }
}
