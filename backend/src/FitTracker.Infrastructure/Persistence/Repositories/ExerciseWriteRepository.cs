using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Persistence.Repositories;

public class ExerciseWriteRepository(
    FitTrackerDbContext context,
    IMapper mapper) : IExerciseWriteRepository
{
    public async Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
    {
        var exerciseEf = mapper.Map<ExerciseEf>(exercise);

        _ = await context.Exercises.AddAsync(exerciseEf, cancellationToken);
    }

    public Task UpdateAsync(Exercise exercise)
    {
        throw new NotImplementedException();
    }
}
