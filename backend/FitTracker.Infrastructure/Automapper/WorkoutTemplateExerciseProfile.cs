using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class WorkoutTemplateExerciseProfile : Profile
    {
        public WorkoutTemplateExerciseProfile()
        {
            _ = CreateMap<WorkoutTemplateExerciseEf, WorkoutTemplateExercise>()
                .ConstructUsing(src => new WorkoutTemplateExercise(
                    id: src.Id,
                    workoutTemplateId: src.WorkoutTemplateId,
                    exerciseId: src.ExerciseId,
                    orderIndex: src.OrderIndex,
                    notes: src.Notes,
                    createdAt: src.CreatedAt,
                    updatedAt: src.UpdatedAt));

            _ = CreateMap<WorkoutTemplateExercise, WorkoutTemplateExerciseEf>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.WorkoutTemplateId, opt => opt.MapFrom(src => src.WorkoutTemplateId))
                .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.WorkoutTemplate, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.PlannedSets, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
