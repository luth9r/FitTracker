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
    public class ExerciseRecordProfile : Profile
    {
        public ExerciseRecordProfile()
        {
            CreateMap<ExerciseRecord, ExerciseRecordEf>()
                .ForMember(dest => dest.MaxWeight_Kilograms, opt => opt.MapFrom(src => src.MaxWeight.ToKilograms()));

            CreateMap<ExerciseRecordEf, ExerciseRecord>()
                .ConstructUsing(src => new ExerciseRecord(
                    src.UserId,
                    src.ExerciseId,
                    Weight.FromKilograms(src.MaxWeight_Kilograms),
                    src.MaxReps,
                    src.MaxVolume,
                    src.MaxTotalVolume,
                    src.MaxWeightDate,
                    src.MaxRepsDate,
                    src.MaxVolumeDate,
                    src.MaxTotalVolumeDate,
                    src.TotalWorkouts,
                    src.TotalSets,
                    src.TotalReps,
                    src.TotalLifted,
                    src.LastPerformed));
        }
    }
}
