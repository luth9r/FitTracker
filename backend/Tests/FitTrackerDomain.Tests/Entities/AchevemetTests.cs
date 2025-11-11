using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Tests.Factories;
using FluentAssertions;
using FluentValidation;

namespace FitTracker.Domain.Tests.Entities
{
    public class AchievementTests
    {
        private readonly Guid _testUserId = Guid.NewGuid();

        #region Builder Tests

        [Fact]
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            // Act
            var builder = Achievement.CreateBuilder();

            // Assert
            builder.Should().NotBeNull();
            builder.Should().BeOfType<Achievement.AchievementBuilder>();
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateAchievement()
        {
            // Arrange & Act
            var achievement = AchievementFactory.Default();

            // Assert
            achievement.Should().NotBeNull();
            achievement.Name.Should().Be("Test Achievement");
            achievement.Description.Should().Be("Test description");
            achievement.Target.Should().Be(100);
            achievement.Tier.Should().Be(AchievementTier.Bronze);
            achievement.Progress.Should().Be(0);
            achievement.IsUnlocked.Should().BeFalse();
        }

        [Fact]
        public void Build_ShouldGenerateId()
        {
            // Arrange & Act
            var achievement = AchievementFactory.Default();

            // Assert
            achievement.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void Build_ShouldSetTimestamps()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var achievement = AchievementFactory.Default();
            var after = DateTime.UtcNow;

            // Assert
            achievement.CreatedAt.Should().BeOnOrAfter(before);
            achievement.CreatedAt.Should().BeOnOrBefore(after);
            achievement.UpdatedAt.Should().BeOnOrAfter(before);
            achievement.UpdatedAt.Should().BeOnOrBefore(after);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Build_WithInvalidName_ShouldThrowValidationException(string invalidName)
        {
            // Arrange & Act
            Action act = () => Achievement.CreateBuilder()
                .WithType(AchievementType.FirstWorkout)
                .WithName(invalidName)
                .WithDescription("Description")
                .WithTarget(100)
                .Build();

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Build_WithNameTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var longName = new string('A', Achievement.NameMaxLength + 1);

            // Act
            Action act = () => Achievement.CreateBuilder()
                .WithType(AchievementType.FirstWorkout)
                .WithName(longName)
                .WithDescription("Description")
                .WithTarget(100)
                .Build();

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Build_WithInvalidDescription_ShouldThrowValidationException(string invalidDescription)
        {
            // Arrange & Act
            Action act = () => Achievement.CreateBuilder()
                .WithType(AchievementType.FirstWorkout)
                .WithName("Name")
                .WithDescription(invalidDescription)
                .WithTarget(100)
                .Build();

            // Assert
            act.Should().Throw<ValidationException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Build_WithInvalidTarget_ShouldThrowValidationException(int invalidTarget)
        {
            // Arrange & Act
            Action act = () => Achievement.CreateBuilder()
                .WithType(AchievementType.FirstWorkout)
                .WithName("Name")
                .WithDescription("Description")
                .WithTarget(invalidTarget)
                .Build();

            // Assert
            act.Should().Throw<ValidationException>();
        }

        #endregion

        #region Achievement Type Tests

        [Theory]
        [InlineData(AchievementType.FirstWorkout)]
        [InlineData(AchievementType.WorkoutStreak)]
        [InlineData(AchievementType.TotalWorkouts)]
        [InlineData(AchievementType.TotalVolume)]
        [InlineData(AchievementType.MaxWeight)]
        [InlineData(AchievementType.ConsecutiveDays)]
        [InlineData(AchievementType.ExerciseVariety)]
        [InlineData(AchievementType.PerfectForm)]
        [InlineData(AchievementType.EarlyBird)]
        [InlineData(AchievementType.NightOwl)]
        [InlineData(AchievementType.WeightMilestone)]
        [InlineData(AchievementType.RepsMilestone)]
        [InlineData(AchievementType.PowerLifter)]
        public void Build_WithDifferentTypes_ShouldSetCorrectType(AchievementType type)
        {
            // Arrange & Act
            var achievement = Achievement.CreateBuilder()
                .WithType(type)
                .WithName("Test")
                .WithDescription("Test1")
                .WithTarget(100)
                .Build();

            // Assert
            achievement.Type.Should().Be(type);
        }

        [Fact]
        public void Build_ShouldGenerateCorrectIconUrl()
        {
            // Arrange & Act
            var achievement = AchievementFactory.FirstWorkout();

            // Assert
            achievement.IconUrl.ToLower().Should().Contain("/icons/achievement_");
            achievement.IconUrl.ToLower().Should().Contain("firstworkout");
        }

        #endregion

        #region Achievement Tier Tests

        [Theory]
        [InlineData(AchievementTier.Bronze)]
        [InlineData(AchievementTier.Silver)]
        [InlineData(AchievementTier.Gold)]
        [InlineData(AchievementTier.Platinum)]
        [InlineData(AchievementTier.Diamond)]
        [InlineData(AchievementTier.Emerald)]
        [InlineData(AchievementTier.Titan)]
        public void Build_WithDifferentTiers_ShouldSetCorrectTier(AchievementTier tier)
        {
            // Arrange & Act
            var achievement = Achievement.CreateBuilder()
                .WithType(AchievementType.FirstWorkout)
                .WithName("Test")
                .WithDescription("Testing")
                .WithTarget(100)
                .WithTier(tier)
                .Build();

            // Assert
            achievement.Tier.Should().Be(tier);
        }

        [Fact]
        public void Build_WithBronzeTier_ShouldSetBronze()
        {
            // Arrange & Act
            var achievement = Achievement.CreateBuilder()
                .WithName("Bronze")
                .WithDescription("Bronze tier")
                .WithTarget(10)
                .WithTier(AchievementTier.Bronze)
                .Build();

            // Assert
            achievement.Tier.Should().Be(AchievementTier.Bronze);
        }

        [Fact]
        public void Build_WithSilverTier_ShouldSetSilver()
        {
            // Arrange & Act
            var achievement = Achievement.CreateBuilder()
                .WithName("Silver")
                .WithDescription("Silver tier")
                .WithTarget(50)
                .WithTier(AchievementTier.Silver)
                .Build();

            // Assert
            achievement.Tier.Should().Be(AchievementTier.Silver);
        }

        [Fact]
        public void Build_WithGoldTier_ShouldSetGold()
        {
            // Arrange & Act
            var achievement = Achievement.CreateBuilder()
                .WithName("Gold")
                .WithDescription("Gold tier")
                .WithTarget(100)
                .WithTier(AchievementTier.Gold)
                .Build();

            // Assert
            achievement.Tier.Should().Be(AchievementTier.Gold);
        }

        #endregion

        #region UpdateProgress Tests

        [Fact]
        public void UpdateProgress_WithValidProgress_ShouldUpdateProgress()
        {
            // Arrange
            var achievement = AchievementFactory.WorkoutStreakSilver();
            var initialUpdatedAt = achievement.UpdatedAt;
            Thread.Sleep(10);

            // Act
            var result = achievement.UpdateProgress(3);

            // Assert
            achievement.Progress.Should().Be(3);
            result.Should().BeFalse(); // Not unlocked yet
            achievement.IsUnlocked.Should().BeFalse();
            achievement.UpdatedAt.Should().BeAfter(initialUpdatedAt);
        }

        [Fact]
        public void UpdateProgress_ReachingTarget_ShouldUnlockAchievement()
        {
            // Arrange
            var achievement = AchievementFactory.FirstWorkout();

            // Act
            var result = achievement.UpdateProgress(1);

            // Assert
            result.Should().BeTrue();
            achievement.IsUnlocked.Should().BeTrue();
            achievement.UnlockedAt.Should().NotBeNull();
            achievement.Progress.Should().Be(1);
        }

        [Fact]
        public void UpdateProgress_ExceedingTarget_ShouldUnlockAchievement()
        {
            // Arrange
            var achievement = AchievementFactory.WorkoutStreakBronze();

            // Act
            var result = achievement.UpdateProgress(10);

            // Assert
            result.Should().BeTrue();
            achievement.IsUnlocked.Should().BeTrue();
            achievement.UnlockedAt.Should().NotBeNull();
            achievement.Progress.Should().Be(10);
        }

        [Fact]
        public void UpdateProgress_AlreadyUnlocked_ShouldNotReturnTrue()
        {
            // Arrange
            var achievement = AchievementFactory.FirstWorkout();
            achievement.UpdateProgress(1); // First unlock
            var unlockedAt = achievement.UnlockedAt;
            Thread.Sleep(10);

            // Act
            var result = achievement.UpdateProgress(2);

            // Assert
            result.Should().BeFalse(); // Already unlocked
            achievement.IsUnlocked.Should().BeTrue();
            achievement.UnlockedAt.Should().Be(unlockedAt);
        }

        [Fact]
        public void UpdateProgress_FromZeroToTarget_ShouldUnlockInOneStep()
        {
            // Arrange
            var achievement = AchievementFactory.TotalWorkoutsBronze();

            // Act
            var result = achievement.UpdateProgress(10);

            // Assert
            result.Should().BeTrue();
            achievement.IsUnlocked.Should().BeTrue();
            achievement.Progress.Should().Be(10);
        }

        [Fact]
        public void UpdateProgress_MultipleIncrements_ShouldTrackCorrectly()
        {
            // Arrange
            var achievement = AchievementFactory.WorkoutStreakSilver();

            // Act
            achievement.UpdateProgress(2);
            achievement.UpdateProgress(4);
            var result = achievement.UpdateProgress(7);

            // Assert
            result.Should().BeTrue(); // Now unlocked
            achievement.Progress.Should().Be(7);
            achievement.IsUnlocked.Should().BeTrue();
        }

        [Fact]
        public void UpdateProgress_ShouldAllowDecrement()
        {
            // Arrange
            var achievement = AchievementFactory.Default();
            achievement.UpdateProgress(50);

            // Act
            achievement.UpdateProgress(30);

            // Assert
            achievement.Progress.Should().Be(30);
            achievement.IsUnlocked.Should().BeFalse();
        }

        #endregion

        #region GetProgressPercentage Tests

        [Theory]
        [InlineData(0, 100, 0)]
        [InlineData(25, 100, 25)]
        [InlineData(50, 100, 50)]
        [InlineData(75, 100, 75)]
        [InlineData(100, 100, 100)]
        [InlineData(150, 100, 150)]
        public void GetProgressPercentage_WithVariousValues_ShouldCalculateCorrectly(
            int progress, int target, int expectedPercentage)
        {
            // Arrange
            var achievement = Achievement.CreateBuilder()
                .WithTarget(target)
                .Build();
            achievement.UpdateProgress(progress);

            // Act
            var percentage = achievement.GetProgressPercentage();

            // Assert
            percentage.Should().Be(expectedPercentage);
        }

        [Fact]
        public void GetProgressPercentage_WithZeroProgress_ShouldReturnZero()
        {
            // Arrange
            var achievement = AchievementFactory.Default();

            // Act
            var percentage = achievement.GetProgressPercentage();

            // Assert
            percentage.Should().Be(0);
        }

        [Fact]
        public void GetProgressPercentage_WhenCompleted_ShouldReturn100()
        {
            // Arrange
            var achievement = AchievementFactory.FirstWorkout();
            achievement.UpdateProgress(1);

            // Act
            var percentage = achievement.GetProgressPercentage();

            // Assert
            percentage.Should().Be(100);
        }

        #endregion

        #region Object Mother Tests

        [Fact]
        public void FirstWorkout_ShouldCreateCorrectAchievement()
        {
            // Arrange & Act
            var achievement = AchievementFactory.FirstWorkout();

            // Assert
            achievement.Name.Should().Be("First Steps");
            achievement.Target.Should().Be(1);
            achievement.Type.Should().Be(AchievementType.FirstWorkout);
            achievement.Tier.Should().Be(AchievementTier.Bronze);
        }

        [Fact]
        public void WorkoutStreakGold_ShouldCreateGoldAchievement()
        {
            // Arrange & Act
            var achievement = AchievementFactory.WorkoutStreakGold();

            // Assert
            achievement.Name.Should().Be("30-Day Streak");
            achievement.Target.Should().Be(30);
            achievement.Tier.Should().Be(AchievementTier.Gold);
        }

        [Fact]
        public void TotalWorkoutsTitan_ShouldCreateTitanAchievement()
        {
            // Arrange & Act
            var achievement = AchievementFactory.TotalWorkoutsTitan();

            // Assert
            achievement.Type.Should().Be(AchievementType.TotalWorkouts);
            achievement.Target.Should().Be(1000);
            achievement.Tier.Should().Be(AchievementTier.Titan);
        }

        [Fact]
        public void MaxWeightEmerald_ShouldCreateEmeraldAchievement()
        {
            // Arrange & Act
            var achievement = AchievementFactory.MaxWeightEmerald();

            // Assert
            achievement.Type.Should().Be(AchievementType.MaxWeight);
            achievement.Target.Should().Be(200);
            achievement.Tier.Should().Be(AchievementTier.Emerald);
        }

        [Fact]
        public void Unlocked_ShouldReturnUnlockedAchievement()
        {
            // Arrange & Act
            var achievement = AchievementFactory.Unlocked();

            // Assert
            achievement.IsUnlocked.Should().BeTrue();
            achievement.UnlockedAt.Should().NotBeNull();
            achievement.Progress.Should().Be(achievement.Target);
        }

        [Fact]
        public void HalfwayComplete_ShouldHave50PercentProgress()
        {
            // Arrange & Act
            var achievement = AchievementFactory.HalfwayComplete();

            // Assert
            achievement.GetProgressPercentage().Should().Be(50);
            achievement.IsUnlocked.Should().BeFalse();
        }

        [Fact]
        public void AlmostComplete_ShouldHave99PercentProgress()
        {
            // Arrange & Act
            var achievement = AchievementFactory.AlmostComplete();

            // Assert
            achievement.GetProgressPercentage().Should().Be(99);
            achievement.IsUnlocked.Should().BeFalse();
        }

        #endregion

        #region Collection Tests

        [Fact]
        public void BronzeCollection_ShouldContainOnlyBronze()
        {
            // Arrange & Act
            var achievements = AchievementFactory.BronzeCollection();

            // Assert
            achievements.Should().HaveCount(8);
            achievements.Should().OnlyContain(a => a.Tier == AchievementTier.Bronze);
        }

        [Fact]
        public void AllTiersCollection_ShouldContainAllTiers()
        {
            // Arrange & Act
            var achievements = AchievementFactory.AllTiersCollection();

            // Assert
            achievements.Should().Contain(a => a.Tier == AchievementTier.Bronze);
            achievements.Should().Contain(a => a.Tier == AchievementTier.Silver);
            achievements.Should().Contain(a => a.Tier == AchievementTier.Gold);
            achievements.Should().Contain(a => a.Tier == AchievementTier.Platinum);
            achievements.Should().Contain(a => a.Tier == AchievementTier.Diamond);
            achievements.Should().Contain(a => a.Tier == AchievementTier.Emerald);
            achievements.Should().Contain(a => a.Tier == AchievementTier.Titan);
        }

        [Fact]
        public void WorkoutProgressionPath_ShouldHaveIncreasingTargets()
        {
            // Arrange & Act
            var achievements = AchievementFactory.WorkoutProgressionPath();

            // Assert
            achievements.Should().HaveCount(4);
            achievements[0].Target.Should().Be(1);
            achievements[1].Target.Should().Be(10);
            achievements[2].Target.Should().Be(100);
            achievements[3].Target.Should().Be(1000);
        }

        [Fact]
        public void StreakProgressionPath_ShouldHaveIncreasingDifficulty()
        {
            // Arrange & Act
            var achievements = AchievementFactory.StreakProgressionPath();

            // Assert
            achievements.Should().HaveCount(3);
            achievements.Should().OnlyContain(a => a.Type == AchievementType.WorkoutStreak);
            achievements[0].Target.Should().BeLessThan(achievements[1].Target);
            achievements[1].Target.Should().BeLessThan(achievements[2].Target);
        }

        [Fact]
        public void MixedProgressCollection_ShouldHaveVariedProgress()
        {
            // Arrange & Act
            var achievements = AchievementFactory.MixedProgressCollection();

            // Assert
            achievements.Should().Contain(a => a.IsUnlocked);
            achievements.Should().Contain(a => !a.IsUnlocked && a.Progress > 0);
            achievements.Should().Contain(a => a.Progress == 0);
        }

        [Fact]
        public void CompleteAchievementSystem_ShouldContainAllTypes()
        {
            // Arrange & Act
            var achievements = AchievementFactory.CompleteAchievementSystem();

            // Assert
            achievements.Should().Contain(a => a.Type == AchievementType.FirstWorkout);
            achievements.Should().Contain(a => a.Type == AchievementType.WorkoutStreak);
            achievements.Should().Contain(a => a.Type == AchievementType.TotalWorkouts);
            achievements.Should().Contain(a => a.Type == AchievementType.TotalVolume);
            achievements.Should().Contain(a => a.Type == AchievementType.MaxWeight);
            achievements.Should().Contain(a => a.Type == AchievementType.ConsecutiveDays);
            achievements.Should().Contain(a => a.Type == AchievementType.ExerciseVariety);
            achievements.Should().Contain(a => a.Type == AchievementType.PerfectForm);
            achievements.Should().Contain(a => a.Type == AchievementType.EarlyBird);
            achievements.Should().Contain(a => a.Type == AchievementType.NightOwl);
        }

        #endregion

        #region UserId Tests

        [Fact]
        public void WithUserId_ShouldSetCorrectUserId()
        {
            // Arrange & Act
            var achievement = AchievementFactory.WithUserId(_testUserId);

            // Assert
            achievement.UserId.Should().Be(_testUserId);
        }

        [Fact]
        public void ForUser_ShouldCreateMultipleAchievementsForUser()
        {
            // Arrange & Act
            var achievements = AchievementFactory.ForUser(_testUserId);

            // Assert
            achievements.Should().OnlyContain(a => a.UserId == _testUserId);
            achievements.Should().NotBeEmpty();
        }

        #endregion
    }
}
