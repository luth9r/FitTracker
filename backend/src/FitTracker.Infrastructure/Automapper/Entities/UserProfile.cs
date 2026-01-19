using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Automapper.Extensions;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper.Entities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        _ = CreateMap<UserEf, User>()
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        _ = CreateMap<User, UserEf>()
            .ForMember(dest => dest.Workouts, opt => opt.Ignore())
            .ForMember(dest => dest.CustomExercises, opt => opt.Ignore())
            .ForMember(dest => dest.WorkoutTemplates, opt => opt.Ignore())
            .ForMember(dest => dest.UserAchievements, opt => opt.Ignore())
            .ForMember(dest => dest.ExerciseRecords, opt => opt.Ignore())
            .IgnoreDomainEventsAndAudit();
    }
}
