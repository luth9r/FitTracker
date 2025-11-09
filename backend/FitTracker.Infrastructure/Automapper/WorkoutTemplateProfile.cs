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
    public class WorkoutTemplateProfile : Profile
    {
        public WorkoutTemplateProfile()
        {
            CreateMap<WorkoutTemplate, WorkoutTemplateEf>()
                .ForMember(dest => dest.Exercises, opt => opt.Ignore());

            CreateMap<WorkoutTemplateEf, WorkoutTemplate>()
                .ConstructUsing(src => new WorkoutTemplate(
                    src.Id,
                    src.Name,
                    src.Description,
                    src.UsageCount,
                    src.LastUsedAt))
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });
        }
    }
}
