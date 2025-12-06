using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Extensions.MedaitR
{
    public static class MediatRExtensions
    {
        /// <summary>
        /// Get clean formatted name of MediatR request without "Command" and "Query".
        /// </summary>
        /// <param name="request">MediatR request.</param>
        /// <returns>Cleaned name.</returns>
        public static string GetFormattedName(this IBaseRequest request)
        {
            return request.GetType().Name
                .Replace("Command", string.Empty)
                .Replace("Query", string.Empty);
        }
    }
}
