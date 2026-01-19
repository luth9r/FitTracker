using AutoMapper;
using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Automapper.Extensions;
using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;

namespace FitTracker.Infrastructure.Automapper.Entities.TemplateAggregate;

public class TemplateWorkoutProfile : Profile
{
    public TemplateWorkoutProfile()
    {
        _ = CreateMap<TemplateWorkoutEf, TemplateWorkout>()
            .ForCtorParam("exercises", opt => opt.MapFrom(src => src.Exercises))
            .ForMember(dest => dest.Exercises, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        _ = CreateMap<TemplateWorkout, TemplateWorkoutEf>()
            .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .IgnoreDomainEventsAndAudit();
    }
}
