using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class SetProfile : Profile
    {
        public SetProfile()
        {
            _ = CreateMap<SetEf, Set>()
                .ConstructUsing(src => new Set(
                    id: src.Id,
                    workoutExerciseId: src.WorkoutExerciseId,
                    setNumber: src.SetNumber,
                    weight: src.WeightKg,
                    reps: src.Reps,
                    restSeconds: src.RestSeconds,
                    setType: (SetType)src.SetType,
                    isCompleted: src.IsCompleted,
                    completedAt: src.CompletedAt,
                    createdAt: src.CreatedAt,
                    updatedAt: src.UpdatedAt));

            _ = CreateMap<Set, SetEf>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WeightKg, opt => opt.MapFrom(src => src.WeightKg))
                .ForMember(dest => dest.WorkoutExerciseId, opt => opt.MapFrom(src => src.WorkoutExerciseId))
                .ForMember(dest => dest.SetNumber, opt => opt.MapFrom(src => src.SetNumber))
                .ForMember(dest => dest.Reps, opt => opt.MapFrom(src => src.Reps))
                .ForMember(dest => dest.RestSeconds, opt => opt.MapFrom(src => src.RestSeconds))
                .ForMember(dest => dest.SetType, opt => opt.MapFrom(src => (int)src.SetType))
                .ForMember(dest => dest.WorkoutExercise, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
