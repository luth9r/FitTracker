using AutoMapper;
using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Automapper.Extensions;
using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;

namespace FitTracker.Infrastructure.Automapper.Entities.TemplateAggregate;

public class TemplateSetProfile : Profile
{
    public TemplateSetProfile()
    {
        _ = CreateMap<TemplateSetEf, TemplateSet>()
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        _ = CreateMap<TemplateSet, TemplateSetEf>()
            .ForMember(dest => dest.SetType, opt => opt.MapFrom(src => (int)src.SetType))
            .ForMember(dest => dest.WorkoutTemplateExercise, opt => opt.Ignore())
            .IgnoreDomainEventsAndAudit();
    }
}
