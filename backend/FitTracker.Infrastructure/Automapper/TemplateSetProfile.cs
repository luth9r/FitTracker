using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;

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
                    Weight.FromKilograms(src.PlannedWeight),
                    src.PlannedReps,
                    src.RestSeconds,
                    (SetType)src.SetType))
                .ForMember(dest => dest.PlannedWeight, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });
        }
    }
}
