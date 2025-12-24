using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Domain.Abstract.Interfaces
{
    /// <summary>
    /// Repository for reading set data.
    /// </summary>
    public interface ISetReadRepository
    {
        /// <summary>
        /// Gets the total weight lifted by a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The total weight lifted by the user.</returns>
        Task<double> GetTotalWeightLiftedAsync(Guid userId, CancellationToken cancellationToken);
    }
}
