using AutoMapper;
using FitTracker.Application.Features.User.Queries.GetRecentWorkouts;
using FitTracker.Domain.ReadModels;

namespace FitTracker.Application.Mapping.User;

public class WorkoutProfile : Profile
{
    public WorkoutProfile()
    {
        CreateMap<WorkoutSummary, RecentWorkoutResponse>()
            .ConstructUsing(src => new RecentWorkoutResponse(
                Id: src.Id,
                WorkoutDate: src.WorkoutDate,
                Name: src.Name,
                IsCompleted: src.IsCompleted,
                DurationMinutes: src.DurationMinutes,
                TotalVolumeKg: src.TotalVolumeKg));
    }
}
