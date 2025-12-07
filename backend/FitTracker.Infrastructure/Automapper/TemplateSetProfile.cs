using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class TemplateSetProfile : Profile
    {
        public TemplateSetProfile()
        {
            _ = CreateMap<TemplateSetEf, TemplateSet>()
                .ConstructUsing(src => new TemplateSet(
                    id: src.Id,
                    workoutTemplateExerciseId: src.WorkoutTemplateExerciseId,
                    setNumber: src.SetNumber,
                    plannedWeight: Weight.FromKilograms(src.PlannedWeight),
                    plannedReps: src.PlannedReps,
                    restSeconds: src.RestSeconds,
                    setType: (SetType)src.SetType,
                    createdAt: src.CreatedAt,
                    updatedAt: src.UpdatedAt));

            _ = CreateMap<TemplateSet, TemplateSetEf>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PlannedWeight, opt => opt.MapFrom(src => src.PlannedWeight.ToKilograms()))
                .ForMember(dest => dest.WorkoutTemplateExerciseId, opt => opt.MapFrom(src => src.WorkoutTemplateExerciseId))
                .ForMember(dest => dest.SetNumber, opt => opt.MapFrom(src => src.SetNumber))
                .ForMember(dest => dest.PlannedReps, opt => opt.MapFrom(src => src.PlannedReps))
                .ForMember(dest => dest.RestSeconds, opt => opt.MapFrom(src => src.RestSeconds))
                .ForMember(dest => dest.SetType, opt => opt.MapFrom(src => (int)src.SetType))
                .ForMember(dest => dest.WorkoutTemplateExercise, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
