using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Application.DTOs.Users;
using FitTracker.Domain.ReadModels;

namespace FitTracker.Application.Mapping.User
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<WorkoutSummary, RecentWorkoutResponse>()
                .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("WorkoutDate", opt => opt.MapFrom(src => src.WorkoutDate))
                .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("IsCompleted", opt => opt.MapFrom(src => src.IsCompleted))
                .ForCtorParam("DurationMinutes", opt => opt.MapFrom(src => src.DurationMinutes))
                .ForCtorParam("TotalVolumeKg", opt => opt.MapFrom(src => src.TotalVolumeKg));
        }
    }
}
