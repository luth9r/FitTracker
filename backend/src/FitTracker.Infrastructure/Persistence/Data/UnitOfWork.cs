using FitTracker.Application.Interfaces;

namespace FitTracker.Infrastructure.Persistence.Data;

public class UnitOfWork(FitTrackerDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}