using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Repositories;

public class TemplateWorkoutReadRepository(FitTrackerDbContext context, IMapper mapper) : IWorkoutTemplateReadRepository
{
    /// <inheritdoc />
    public async Task<TemplateWorkout?> GetTemplateByNameAsync(
        string name,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var entityEf = await context.WorkoutTemplates
            .AsNoTracking()
            .Include(t => t.Exercises)
            .ThenInclude(e => e.PlannedSets)
            .FirstOrDefaultAsync(t => t.Name == name && t.UserId == userId, cancellationToken);

        if (entityEf == null)
        {
            return null;
        }

        return mapper.Map<TemplateWorkout>(entityEf);
    }

    /// <inheritdoc />
    public async Task<TemplateWorkout?> GetTemplateByIdAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var entity = await context.WorkoutTemplates
            .AsNoTracking()
            .Include(t => t.Exercises)
            .ThenInclude(e => e.PlannedSets)
            .Include(t => t.Exercises)
            .ThenInclude(e => e.Exercise)
            .FirstOrDefaultAsync(
                t => t.Id == id && t.UserId == userId,
                cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return mapper.Map<TemplateWorkout>(entity);
    }
}
