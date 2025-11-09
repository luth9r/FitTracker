using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using static FitTracker.Domain.Entities.TemplateSet;

namespace FitTracker.Infrastructure.Automapper
{
    public class TemplateSetProfile : Profile
    {
        public TemplateSetProfile()
        {
            CreateMap<TemplateSet, TemplateSetEf>()
                .ForMember(dest => dest.PlannedWeight, opt => opt.MapFrom(src => src.PlannedWeight.ToKilograms()))
                .ForMember(dest => dest.WorkoutTemplateExercise, opt => opt.Ignore());

            CreateMap<TemplateSetEf, TemplateSet>()
                .ConstructUsing(src => new TemplateSet(
                    src.WorkoutTemplateExerciseId,
                    src.SetNumber,
                    src.PlannedWeight,
                    src.PlannedReps,
                    src.RestSeconds,
                    (SetType)src.SetType))
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });
        }
    }
}
