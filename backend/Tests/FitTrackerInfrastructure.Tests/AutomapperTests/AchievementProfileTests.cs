using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class AchievementProfileTests
    {
        private readonly IMapper _mapper;

        public AchievementProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<AchievementProfile>();

            var config = new MapperConfiguration(
                configExpression,
                NullLoggerFactory.Instance
            );
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Configuration_Should_BeValid()
        {
            // Arrange & Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_Achievement_To_AchievementEf()
        {
            // Arrange
            var achievement = new Achievement(
                userId: Guid.NewGuid(),
                type: AchievementType.FirstWorkout,
                name: "Test Achievement",
                description: "Test Description",
                target: 100,
                tier: AchievementTier.Bronze,
                progress: 50,
                isUnlocked: false,
                unlockedAt: null
            );

            // Act
            var result = _mapper.Map<AchievementEf>(achievement);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(achievement.UserId, result.UserId);
            Assert.Equal((int)achievement.Type, result.Type);
            Assert.Equal(achievement.Name, result.Name);
            Assert.Equal(achievement.Description, result.Description);
            Assert.Equal(achievement.Target, result.Target);
            Assert.Equal((int)achievement.Tier, result.Tier);
            Assert.Equal(achievement.Progress, result.Progress);
            Assert.Equal(achievement.IsUnlocked, result.IsUnlocked);
            Assert.Equal(achievement.UnlockedAt, result.UnlockedAt);
        }

        [Fact]
        public void Should_Map_AchievementEf_To_Achievement()
        {
            // Arrange
            var achievementEf = new AchievementEf
            {
                UserId = Guid.NewGuid(),
                Type = (int)AchievementType.PerfectForm,
                Name = "Test Achievement",
                Description = "Test Description",
                Target = 200,
                Tier = (int)AchievementTier.Silver,
                Progress = 90,
                IsUnlocked = false,
                UnlockedAt = DateTime.UtcNow
            };

            // Act
            var result = _mapper.Map<Achievement>(achievementEf);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(achievementEf.UserId, result.UserId);
            Assert.Equal((AchievementType)achievementEf.Type, result.Type);
            Assert.Equal(achievementEf.Name, result.Name);
            Assert.Equal(achievementEf.Description, result.Description);
            Assert.Equal(achievementEf.Target, result.Target);
            Assert.Equal((AchievementTier)achievementEf.Tier, result.Tier);
            Assert.Equal(achievementEf.Progress, result.Progress);
            Assert.Equal(achievementEf.IsUnlocked, result.IsUnlocked);
            Assert.Equal(achievementEf.UnlockedAt, result.UnlockedAt);
        }
    }
}
