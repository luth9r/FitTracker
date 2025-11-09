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
    public class WorkoutExerciseProfile : Profile
    {
        public WorkoutExerciseProfile()
        {
            CreateMap<WorkoutExercise, WorkoutExerciseEf>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WorkoutId, opt => opt.MapFrom(src => src.WorkoutId))
                .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            CreateMap<WorkoutExerciseEf, WorkoutExercise>()
                .ConstructUsing(src => new WorkoutExercise(src.WorkoutId, src.ExerciseId, src.OrderIndex, src.Notes));
        }
    }
}
