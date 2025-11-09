using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<Workout, WorkoutEf>();

            CreateMap<WorkoutEf, Workout>()
                .ConstructUsing(src => new Workout(
                    src.Id,
                    src.UserId,
                    src.Name,
                    src.WorkoutDate,
                    src.WorkoutTemplateId,
                    src.Notes,
                    src.Duration,
                    src.IsCompleted,
                    src.IsInProgress,
                    src.StartedAt,
                    src.CompletedAt,
                    src.TotalVolumeKg));
        }
    }
}
