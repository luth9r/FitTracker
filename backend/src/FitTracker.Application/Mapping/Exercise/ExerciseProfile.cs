using AutoMapper;
using FitTracker.Application.Features.Exercise.Common;
using FitTracker.Application.Features.Exercise.Queries.GetExerciseById;
using FitTracker.Domain.ReadModels;
using ExerciseEntity = FitTracker.Domain.Entities.Exercise;

namespace FitTracker.Application.Mapping.Exercise;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        _ = CreateMap<ExerciseEntity, ExerciseResponse>()
            .ForCtorParam(
                "Name",
                opt => opt.MapFrom(src =>
                    src.IsCustomExercise() ? src.Name : $"Exercise.Name.{src.Name}"))
            .ForCtorParam(
                "MuscleGroup",
                opt => opt.MapFrom(src =>
                    $"Exercise.MuscleGroup.{src.MuscleGroup}"))
            .ForCtorParam(
                "Equipment",
                opt => opt.MapFrom(src => $"Exercise.Equipment.{src.Equipment}"))
            .ForCtorParam(
                "IsCustom",
                opt => opt.MapFrom(src =>
                    src.CreatedByUserId.HasValue));

        _ = CreateMap<ExerciseDetails, ExerciseDetailsResponse>()
            .ForCtorParam(
                "Name",
                opt => opt.MapFrom(src =>
                    src.IsCustom
                        ? src.Name
                        : $"Exercise.Name.{src.Name}"))
            .ForCtorParam(
                "MuscleGroup",
                opt => opt.MapFrom(src =>
                    $"Exercise.MuscleGroup.{src.MuscleGroup}"))
            .ForCtorParam(
                "Equipment",
                opt => opt.MapFrom(src =>
                    $"Exercise.Equipment.{src.Equipment}"))
            .ForCtorParam("VolumeHistory", opt => opt.MapFrom(s => s.VolumeHistory));

        CreateMap<ExerciseHistoryPoint, ExerciseHistoryPointResponse>()
            .ForCtorParam("Date", opt => opt.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));
    }
}
