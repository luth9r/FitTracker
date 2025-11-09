using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class WorkoutTemplateExerciseProfile : Profile
    {
        public WorkoutTemplateExerciseProfile()
        {
            CreateMap<WorkoutTemplateExercise, WorkoutTemplateExerciseEf>()
                .ForMember(dest => dest.WorkoutTemplate, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.PlannedSets, opt => opt.Ignore());

            CreateMap<WorkoutTemplateExerciseEf, WorkoutTemplateExercise>()
                .ConstructUsing(src => new WorkoutTemplateExercise(
                    src.WorkoutTemplateId,
                    src.ExerciseId,
                    src.OrderIndex,
                    src.Notes))
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });
        }
    }
}
