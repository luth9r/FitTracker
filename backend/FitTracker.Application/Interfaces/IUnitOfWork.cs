using System;
using System.Collections.Generic;
using System.Text;

namespace FitTracker.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
