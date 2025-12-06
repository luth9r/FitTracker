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
            _ = CreateMap<ExerciseRecordEf, ExerciseRecord>()
                .ConstructUsing(src => new ExerciseRecord(
                    id: src.Id,
                    userId: src.UserId,
                    exerciseId: src.ExerciseId,
                    maxWeight: Weight.FromKilograms(src.MaxWeightKilograms),
                    maxReps: src.MaxReps,
                    maxVolume: src.MaxVolume,
                    maxTotalVolume: src.MaxTotalVolume,
                    maxWeightDate: src.MaxWeightDate,
                    maxRepsDate: src.MaxRepsDate,
                    maxVolumeDate: src.MaxVolumeDate,
                    maxTotalVolumeDate: src.MaxTotalVolumeDate,
                    totalWorkouts: src.TotalWorkouts,
                    totalSets: src.TotalSets,
                    totalReps: src.TotalReps,
                    totalLifted: src.TotalLifted,
                    lastPerformed: src.LastPerformed,
                    createdAt: src.CreatedAt,
                    updatedAt: src.UpdatedAt));

            _ = CreateMap<ExerciseRecord, ExerciseRecordEf>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MaxWeightKilograms, opt => opt.MapFrom(src => src.MaxWeight.ToKilograms()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
