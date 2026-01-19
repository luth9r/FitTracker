using AutoMapper;
using FitTracker.Application.Features.WorkoutTemplate.Commands.CreateWorkoutTemplate;
using FitTracker.Domain.Entities.TemplateAggregate;

namespace FitTracker.Application.Mapping.Exercise;

public class WorkoutTemplateProfile : Profile
{
    public WorkoutTemplateProfile()
    {
        CreateMap<CreateTemplateSetDto, TemplateSetData>();
    }
}
