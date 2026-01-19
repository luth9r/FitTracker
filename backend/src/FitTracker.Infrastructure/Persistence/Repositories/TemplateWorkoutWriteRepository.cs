using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;

namespace FitTracker.Infrastructure.Persistence.Repositories;

public class TemplateWorkoutWriteRepository(FitTrackerDbContext context, IMapper mapper)
    : IWorkoutTemplateWriteRepository
{
    /// <inheritdoc />
    public async Task AddAsync(TemplateWorkout template, CancellationToken cancellationToken = default)
    {
        var templateEf = mapper.Map<TemplateWorkoutEf>(template);

        _ = await context.WorkoutTemplates.AddAsync(templateEf, cancellationToken);
    }
}
