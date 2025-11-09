using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class SetProfile : Profile
    {
        public SetProfile()
        {
            CreateMap<Set, SetEf>()
                .ForMember(dest => dest.WeightKg, opt => opt.MapFrom(src => src.Weight.ToKilograms()))
                .ForMember(dest => dest.WorkoutExercise, opt => opt.Ignore());

            CreateMap<SetEf, Set>()
                .ConstructUsing(src => new Set(
                    src.WorkoutExerciseId,
                    src.SetNumber,
                    Weight.FromKilograms(src.WeightKg),
                    src.Reps,
                    src.RestSeconds,
                    (Domain.Enums.SetType)src.SetType,
                    src.IsCompleted,
                    src.CompletedAt))
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });

        }
    }
}
