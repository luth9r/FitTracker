using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Automapper.Extensions;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper.Entities;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        _ = CreateMap<ExerciseEf, Exercise>()
            .ForCtorParam("muscleGroup", opt => opt.MapFrom(src => (MuscleGroup)src.MuscleGroup))
            .ForCtorParam("equipment", opt => opt.MapFrom(src => (Equipment)src.Equipment))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        _ = CreateMap<Exercise, ExerciseEf>()
            .ForMember(dest => dest.MuscleGroup, opt => opt.MapFrom(src => (int)src.MuscleGroup))
            .ForMember(dest => dest.Equipment, opt => opt.MapFrom(src => (int)src.Equipment))
            .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
            .ForMember(dest => dest.WorkoutExercises, opt => opt.Ignore())
            .IgnoreDomainEventsAndAudit();
    }
}
