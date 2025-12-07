using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            _ = CreateMap<WorkoutEf, Workout>()
                .ConstructUsing(src => new Workout(
                    id: src.Id,
                    userId: src.UserId,
                    name: src.Name,
                    workoutDate: src.WorkoutDate,
                    workoutTemplateId: src.WorkoutTemplateId,
                    notes: src.Notes,
                    duration: src.Duration,
                    isCompleted: src.IsCompleted,
                    isInProgress: src.IsInProgress,
                    startedAt: src.StartedAt,
                    completedAt: src.CompletedAt,
                    totalVolumeKg: src.TotalVolumeKg,
                    createdAt: src.CreatedAt,
                    updatedAt: src.UpdatedAt));

            _ = CreateMap<Workout, WorkoutEf>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.WorkoutTemplateId, opt => opt.MapFrom(src => src.WorkoutTemplateId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.WorkoutDate, opt => opt.MapFrom(src => src.WorkoutDate))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted))
                .ForMember(dest => dest.IsInProgress, opt => opt.MapFrom(src => src.IsInProgress))
                .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.StartedAt))
                .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
                .ForMember(dest => dest.TotalVolumeKg, opt => opt.MapFrom(src => src.TotalVolumeKg))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.WorkoutTemplate, opt => opt.Ignore())
                .ForMember(dest => dest.Exercises, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
