using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;

namespace FitTracker.Infrastructure.Automapper
{
    public class AchievementProfile : Profile
    {
        public AchievementProfile()
        {
            CreateMap<Achievement, AchievementEf>();

            CreateMap<AchievementEf, Achievement>()
                .ConstructUsing(src => new Achievement(
                    src.UserId,
                    (AchievementType)src.Type,
                    src.Name,
                    src.Description,
                    src.Target,
                    (AchievementTier)src.Tier,
                    src.Progress,
                    src.IsUnlocked,
                    src.UnlockedAt));
        }
    }
}
