using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper.Entities;

public class AchievementProfile : Profile
{
    public AchievementProfile()
    {
        _ = CreateMap<AchievementEf, Achievement>()
            .ConstructUsing(src => new Achievement(
                id: src.Id,
                type: (AchievementType)src.Type,
                name: src.Name,
                description: src.Description,
                iconUrl: src.IconUrl,
                target: src.Target,
                tier: (AchievementTier)src.Tier,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt));

        _ = CreateMap<Achievement, AchievementEf>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(dest => dest.Tier, opt => opt.MapFrom(src => (int)src.Tier))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        _ = CreateMap<UserAchievementEf, UserAchievement>()
            .ConstructUsing(src => new UserAchievement(
                id: src.Id,
                achievementId: src.AchievementId,
                userId: src.UserId,
                progress: src.Progress,
                isUnlocked: src.IsUnlocked,
                unlockedAt: src.UnlockedAt,
                createdAt: src.CreatedAt,
                updatedAt: src.UpdatedAt));

        _ = CreateMap<UserAchievement, UserAchievementEf>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AchievementId, opt => opt.MapFrom(src => src.AchievementId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}