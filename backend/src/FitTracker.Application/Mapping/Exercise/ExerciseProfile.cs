using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Application.DTOs.Exercise;
using FitTracker.Application.Interfaces;
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

            //.ConstructUsing(src => new ExerciseDetailsResponse(
            //    Id: src.Id,
            //    Name: src.Name,
            //    MuscleGroup: src.MuscleGroup.ToString(),
            //    Equipment: src.Equipment.ToString(),
            //    Description: src.Description,
            //    ImageUrl: src.ImageUrl,
            //    VideoUrl: src.VideoUrl,
            //    IsCustom: src.IsCustom,
            //    MaxWeightKg: src.MaxWeightKg,
            //    MaxReps: src.MaxReps,
            //    MaxVolume: src.MaxVolume,
            //    MaxTotalVolume: src.MaxTotalVolume,
            //    MaxWeightDate: src.MaxWeightDate,
            //    MaxRepsDate: src.MaxRepsDate,
            //    MaxVolumeDate: src.MaxVolumeDate,
            //    MaxTotalVolumeDate: src.MaxTotalVolumeDate,
            //    TotalWorkouts: src.TotalWorkouts,
            //    TotalSets: src.TotalSets,
            //    TotalReps: src.TotalReps,
            //    TotalLifted: src.TotalLifted,
            //    AvgWeightPerSet: src.AvgWeightPerSet,
            //    AvgRepsPerSet: src.AvgRepsPerSet,
            //    LastPerformed: src.LastPerformed,
            //    VolumeHistory: src.VolumeHistory)
            //    );
        }
    }
}
