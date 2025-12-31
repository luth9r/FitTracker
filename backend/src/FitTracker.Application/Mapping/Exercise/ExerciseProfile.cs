using AutoMapper;
using FitTracker.Application.DTOs.Exercise;
using FitTracker.Domain.ReadModels;
using ExerciseEntity = FitTracker.Domain.Entities.Exercise;

namespace FitTracker.Application.Mapping.Exercise
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            _ = CreateMap<ExerciseEntity, ExerciseResponse>()
                .ConstructUsing(src => new ExerciseResponse(
                    src.Id,
                    src.Name,
                    src.Description,
                    src.ImageUrl,
                    src.VideoUrl,
                    string.Empty,
                    string.Empty,
                    src.CreatedByUserId.HasValue));

            _ = CreateMap<ExerciseDetails, ExerciseDetailsResponse>()
                .ForCtorParam("MuscleGroup", opt => opt.MapFrom(s => s.MuscleGroup.ToString()))
                .ForCtorParam("Equipment", opt => opt.MapFrom(s => s.Equipment.ToString()))
                .ForCtorParam("VolumeHistory", opt => opt.MapFrom(s => s.VolumeHistory));

            CreateMap<ExerciseHistoryPoint, ExerciseHistoryPointResponse>()
                .ForCtorParam("Date", opt => opt.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));
        }
    }
}
