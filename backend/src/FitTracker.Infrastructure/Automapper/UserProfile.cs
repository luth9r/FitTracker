using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        _ = CreateMap<UserEf, User>()
            .ConstructUsing(src => new User(
                id: src.Id,
                username: src.Username,
                email: src.Email,
                passwordHash: src.PasswordHash,
                firstName: src.FirstName,
                lastName: src.LastName,
                avatar: src.Avatar,
                bio: src.Bio,
                isEmailVerified: src.IsEmailVerified,
                googleProviderId: src.GoogleProviderId,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        _ = CreateMap<User, UserEf>()
            .ForMember(dest => dest.Workouts, opt => opt.Ignore())
            .ForMember(dest => dest.CustomExercises, opt => opt.Ignore())
            .ForMember(dest => dest.WorkoutTemplates, opt => opt.Ignore())
            .ForMember(dest => dest.UserAchievements, opt => opt.Ignore())
            .ForMember(dest => dest.ExerciseRecords, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (src.DomainEvents == null || src!.DomainEvents.Count == 0)
                {
                    return;
                }

                foreach (var domainEvent in src.DomainEvents)
                {
                    dest.AddDomainEvent(domainEvent);
                }
            });
    }
}
