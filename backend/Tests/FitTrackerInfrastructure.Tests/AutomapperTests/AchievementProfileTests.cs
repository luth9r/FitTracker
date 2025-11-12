using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
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
            // Arrange & Act
            Action act = () => _mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            act.Should().NotThrow();
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
            result.Should().NotBeNull();
            result.UserId.Should().Be(achievement.UserId);
            result.Type.Should().Be((int)achievement.Type);
            result.Name.Should().Be(achievement.Name);
            result.Description.Should().Be(achievement.Description);
            result.Target.Should().Be(achievement.Target);
            result.Tier.Should().Be((int)achievement.Tier);
            result.Progress.Should().Be(achievement.Progress);
            result.IsUnlocked.Should().Be(achievement.IsUnlocked);
            result.UnlockedAt.Should().Be(achievement.UnlockedAt);
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
            result.Should().NotBeNull();
            result.UserId.Should().Be(achievementEf.UserId);
            result.Type.Should().Be((AchievementType)achievementEf.Type);
            result.Name.Should().Be(achievementEf.Name);
            result.Description.Should().Be(achievementEf.Description);
            result.Target.Should().Be(achievementEf.Target);
            result.Tier.Should().Be((AchievementTier)achievementEf.Tier);
            result.Progress.Should().Be(achievementEf.Progress);
            result.IsUnlocked.Should().Be(achievementEf.IsUnlocked);
            result.UnlockedAt.Should().Be(achievementEf.UnlockedAt);
        }
    }
}
