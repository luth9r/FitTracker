using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Application.DTOs.Exercise;
using FitTracker.Application.Interfaces;
using ExerciseEntity = FitTracker.Domain.Entities.Exercise;

namespace FitTracker.Application.Mapping.Exercise
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            _ = CreateMap<ExerciseEntity, ExerciseResponse>()
                .ConstructUsing(src => new ExerciseResponse(
                    src.Name,
                    src.Description,
                    src.ImageUrl,
                    src.VideoUrl,
                    string.Empty,
                    string.Empty,
                    src.CreatedByUserId.HasValue));
        }
    }
}
