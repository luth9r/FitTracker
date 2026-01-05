using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper;

public class ExerciseRecordProfile : Profile
{
    public ExerciseRecordProfile()
    {
        _ = CreateMap<ExerciseRecordEf, ExerciseRecord>()
            .ConstructUsing(src => new ExerciseRecord(
                id: src.Id,
                userId: src.UserId,
                exerciseId: src.ExerciseId,
                maxWeight: src.MaxWeightKg,
                maxReps: src.MaxReps,
                maxVolume: src.MaxVolumeKg,
                maxTotalVolume: src.MaxTotalVolumeKg,
                maxWeightDate: src.MaxWeightDate,
                maxRepsDate: src.MaxRepsDate,
                maxVolumeDate: src.MaxVolumeDate,
                maxTotalVolumeDate: src.MaxTotalVolumeDate,
                totalWorkouts: src.TotalWorkouts,
                totalSets: src.TotalSets,
                totalReps: src.TotalReps,
                totalLifted: src.TotalLiftedKg,
                lastPerformed: src.LastPerformed,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt));

        _ = CreateMap<ExerciseRecord, ExerciseRecordEf>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MaxWeightKg, opt => opt.MapFrom(src => src.MaxWeightKg))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ExerciseId, opt => opt.MapFrom(src => src.ExerciseId))
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Exercise, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}
