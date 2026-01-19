using AutoMapper;
using AutoMapper.QueryableExtensions;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ReadModels;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitTracker.Infrastructure.Persistence.Repositories;

internal sealed class ExerciseReadRepository(
    FitTrackerDbContext context,
    IMapper mapper) : IExerciseReadRepository
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<Exercise>> GetExercisesAsync(
        ExerciseFilterType filter,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        var query = context.Exercises
            .AsNoTracking()
            .AsQueryable();

        switch (filter)
        {
            case ExerciseFilterType.Standard:
                query = query.Where(x => x.CreatedByUserId == null);
                break;

            case ExerciseFilterType.Custom:
                if (!userId.HasValue || userId.Value == Guid.Empty)
                {
                    throw new ArgumentException("UserId is required when filter is Custom.", nameof(userId));
                }

                query = query.Where(x => x.CreatedByUserId == userId);
                break;

            default:
                if (!userId.HasValue || userId.Value == Guid.Empty)
                {
                    throw new ArgumentException("UserId is required when filter is All.", nameof(userId));
                }

                query = query.Where(x =>
                    x.CreatedByUserId == null || x.CreatedByUserId == userId);
                break;
        }

        var exercises = await query
            .ProjectTo<Exercise>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return exercises;
    }

    /// <inheritdoc />
    public async Task<ExerciseDetails?> GetExerciseDetailsAsync(
        Guid exerciseId,
        Guid userId,
        int fromDateMonths = 24,
        CancellationToken cancellationToken = default)
    {
        var exerciseEf = await GetExerciseAsync(exerciseId, userId, cancellationToken);
        if (exerciseEf == null)
        {
            return null;
        }

        var recordTask = GetExerciseRecordAsync(exerciseId, userId, cancellationToken);
        var historyTask = GetVolumeHistoryAsync(exerciseId, userId, fromDateMonths, cancellationToken);

        await Task.WhenAll(recordTask, historyTask);

        return BuildExerciseDetails(
            exerciseEf,
            recordTask.Result,
            historyTask.Result);
    }

    private async Task<ExerciseEf?> GetExerciseAsync(Guid exerciseId, Guid userId, CancellationToken cancellationToken)
    {
        return await context.Exercises
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == exerciseId && (x.CreatedByUserId == null || x.CreatedByUserId == userId),
                cancellationToken);
    }

    private async Task<ExerciseRecordEf?> GetExerciseRecordAsync(
        Guid exerciseId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await context.ExerciseRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.ExerciseId == exerciseId && r.UserId == userId, cancellationToken);
    }

    private async Task<List<ExerciseHistoryPoint>> GetVolumeHistoryAsync(
        Guid exerciseId,
        Guid userId,
        int fromDateMonths,
        CancellationToken cancellationToken)
    {
        var filterDate = fromDateMonths > 0
            ? DateTime.UtcNow.Date.AddMonths(-fromDateMonths)
            : (DateTime?)null;

        var query = await (
                from w in context.Workouts.AsNoTracking()
                join we in context.WorkoutExercises on w.Id equals we.WorkoutId
                join s in context.Sets on we.Id equals s.WorkoutExerciseId
                where w.UserId == userId
                      && we.ExerciseId == exerciseId
                      && s.IsCompleted
                      && (filterDate == null || w.WorkoutDate >= filterDate)
                group s by w.WorkoutDate.Date
                into g
                select new
                {
                    Date = g.Key,
                    Volume = g.Sum(x => x.WeightKg * x.Reps),
                })
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);

        return query
            .Select(x => new ExerciseHistoryPoint(DateOnly.FromDateTime(x.Date), x.Volume))
            .ToList();
    }

    private static ExerciseDetails BuildExerciseDetails(
        ExerciseEf ex,
        ExerciseRecordEf? rec,
        List<ExerciseHistoryPoint> history)
    {
        static double Avg(double total, int count)
        {
            return count > 0 ? total / count : 0;
        }

        var hasRec = rec != null && rec.TotalWorkouts > 0;

        var totalLifted = rec?.TotalLiftedKg ?? 0;
        var totalSets = rec?.TotalSets ?? 0;
        var totalReps = rec?.TotalReps ?? 0;

        return new ExerciseDetails(

            // Basic
            ex.Id,
            ex.Name,
            (MuscleGroup)ex.MuscleGroup,
            (Equipment)ex.Equipment,
            ex.Description,
            ex.ImageUrl,
            ex.VideoUrl,
            ex.CreatedByUserId.HasValue,

            // Records
            rec?.MaxWeightKg ?? 0,
            rec?.MaxReps ?? 0,
            rec?.MaxVolumeKg ?? 0,
            rec?.MaxTotalVolumeKg ?? 0,

            // Dates
            hasRec ? rec!.MaxWeightDate : null,
            hasRec ? rec!.MaxRepsDate : null,
            hasRec ? rec!.MaxVolumeDate : null,
            hasRec ? rec!.MaxTotalVolumeDate : null,

            // Stats
            rec?.TotalWorkouts ?? 0,
            totalSets,
            totalReps,
            totalLifted,
            Avg(totalLifted, totalSets),
            Avg(totalReps, totalSets),
            hasRec ? rec!.LastPerformed : null,
            history);
    }

    /// <inheritdoc />
    public async Task<Exercise?> GetExerciseByName(
        string exerciseName,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var exercise = await context.Exercises
            .AsNoTracking()
            .Where(x => x.CreatedByUserId == userId && x.Name == exerciseName)
            .ProjectTo<Exercise>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return exercise;
    }
}
