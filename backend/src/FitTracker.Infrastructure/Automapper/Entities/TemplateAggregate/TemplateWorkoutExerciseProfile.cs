using AutoMapper;
using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Automapper.Extensions;
using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;

namespace FitTracker.Infrastructure.Automapper.Entities.TemplateAggregate;

public class TemplateWorkoutExerciseProfile : Profile
{
    public TemplateWorkoutExerciseProfile()
    {
        _ = CreateMap<TemplateWorkoutExerciseEf, TemplateWorkoutExercise>()
            .ForCtorParam("sets", opt => opt.MapFrom(src => src.PlannedSets))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
            .ForMember(dest => dest.Sets, opt => opt.Ignore());

        _ = CreateMap<TemplateWorkoutExercise, TemplateWorkoutExerciseEf>()
            .ForMember(dest => dest.PlannedSets, opt => opt.MapFrom(src => src.Sets))
            .ForMember(dest => dest.WorkoutTemplate, opt => opt.Ignore())
            .ForMember(dest => dest.Exercise, opt => opt.Ignore())
            .IgnoreDomainEventsAndAudit();
    }
}
