using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<Workout, WorkoutEf>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.WorkoutTemplate, opt => opt.Ignore())
                .ForMember(dest => dest.Exercises, opt => opt.Ignore());

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
                    src.TotalVolumeKg))
                .AfterMap((src, dest) =>
                {
                    dest.SetDatabaseFields(src.Id, src.CreatedAt, src.UpdatedAt);
                });
        }
    }
}
