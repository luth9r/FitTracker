using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            CreateMap<Exercise, ExerciseEf>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.WorkoutExercises, opt => opt.Ignore());

            CreateMap<ExerciseEf, Exercise>()
                .ConstructUsing(src => new Exercise(
                    src.Name,
                    src.Description,
                    src.ImageUrl,
                    src.VideoUrl,
                    (MuscleGroup)src.MuscleGroup,
                    (Equipment)src.Equipment,
                    src.IsCustom,
                    src.UserId))
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });
        }
    }
}
