using AutoMapper;
using FitTracker.Application.Features.WorkoutTemplate.Queries.GetWorkoutTemplate;
using FitTracker.Domain.Entities.TemplateAggregate;

namespace FitTracker.Infrastructure.Automapper.Features;

public class GetTemplateWorkoutProfile : Profile
{
    public GetTemplateWorkoutProfile()
    {
        CreateMap<TemplateWorkout, GetTemplateWorkoutResponses>();

        CreateMap<TemplateWorkoutExercise, WorkoutTemplateExerciseDto>();

        CreateMap<TemplateSet, WorkoutTemplateSetDto>();
    }
}
