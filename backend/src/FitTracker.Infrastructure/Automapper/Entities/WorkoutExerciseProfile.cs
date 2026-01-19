using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper.Entities;

public class WorkoutExerciseProfile : Profile
{
    public WorkoutExerciseProfile()
    {
        _ = CreateMap<WorkoutExerciseEf, WorkoutExercise>()
            .ConstructUsing(src => new WorkoutExercise(
                id: src.Id,
                workoutId: src.WorkoutId,
                exerciseId: src.ExerciseId,
                orderIndex: src.OrderIndex,
                notes: src.Notes,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt));

        _ = CreateMap<WorkoutExercise, WorkoutExerciseEf>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.WorkoutId, opt => opt.MapFrom(src => src.WorkoutId))
            .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId))
            .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.Workout, opt => opt.Ignore())
            .ForMember(dest => dest.Exercise, opt => opt.Ignore())
            .ForMember(dest => dest.Sets, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}