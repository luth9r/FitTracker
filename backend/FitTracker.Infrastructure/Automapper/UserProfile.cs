using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
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
                    preferredUnits: UnitSystem.FromString(src.PreferredUnits),
                    isEmailVerified: src.IsEmailVerified,
                    googleProviderId: src.GoogleProviderId,
                    createdAt: src.CreatedAt,
                    updatedAt: src.UpdatedAt));

            _ = CreateMap<User, UserEf>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.PreferredUnits, opt => opt.MapFrom(src => src.PreferredUnits.ToString()))
                .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => src.IsEmailVerified))
                .ForMember(dest => dest.GoogleProviderId, opt => opt.MapFrom(src => src.GoogleProviderId))
                .ForMember(dest => dest.Workouts, opt => opt.Ignore())
                .ForMember(dest => dest.CustomExercises, opt => opt.Ignore())
                .ForMember(dest => dest.WorkoutTemplates, opt => opt.Ignore())
                .ForMember(dest => dest.UserAchievements, opt => opt.Ignore())
                .ForMember(dest => dest.ExerciseRecords, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<string, UnitSystem>().ConvertUsing(src => UnitSystem.FromString(src));

            CreateMap<UnitSystem, string>().ConvertUsing(src => src.ToString());
        }
    }
}
