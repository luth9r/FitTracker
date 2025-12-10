using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Application.DTOs.Users;
using FitTracker.Domain.ReadModels;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Application.Mapping.User
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            _ = CreateMap<WorkoutSummary, RecentWorkoutResponse>()
                .ForMember(
                dest => dest.TotalVolume,
                opt => opt.MapFrom((src, dest, destMember, ctx) =>
                {
                    var unitsStr = (string?)ctx.Items["preferredUnits"] ?? "metric";
                    var units = UnitSystem.FromString(unitsStr);
                    return (double)UnitSystem.ConvertFromMetric(src.TotalVolumeKg, units);
                }));
        }
    }
}
