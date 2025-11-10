// Domain.Tests/Entities/AchievementTests.cs
using System;
using System.Threading;
using Xunit;
using FluentValidation;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Tests.Factories;

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
            Assert.NotNull(builder);
            Assert.IsType<Achievement.AchievementBuilder>(builder);
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateAchievement()
        {
            // Arrange & Act
            var achievement = AchievementMother.Default();

            // Assert
            Assert.NotNull(achievement);
            Assert.Equal("Test Achievement", achievement.Name);
            Assert.Equal("Test description", achievement.Description);
            Assert.Equal(100, achievement.Target);
            Assert.Equal(AchievementTier.Bronze, achievement.Tier);
            Assert.Equal(0, achievement.Progress);
            Assert.False(achievement.IsUnlocked);
        }

        [Fact]
        public void Build_ShouldGenerateId()
        {
            // Arrange & Act
            var achievement = AchievementMother.Default();

            // Assert
            Assert.NotEqual(Guid.Empty, achievement.Id);
        }

        [Fact]
        public void Build_ShouldSetTimestamps()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var achievement = AchievementMother.Default();
            var after = DateTime.UtcNow;

            // Assert
            Assert.True(achievement.CreatedAt >= before);
            Assert.True(achievement.CreatedAt <= after);
            Assert.True(achievement.UpdatedAt >= before);
            Assert.True(achievement.UpdatedAt <= after);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Build_WithInvalidName_ShouldThrowValidationException(string invalidName)
        {
            // Arrange & Act & Assert
            Assert.Throws<ValidationException>(() =>
                Achievement.CreateBuilder()
                    .WithType(AchievementType.FirstWorkout)
                    .WithName(invalidName)
                    .WithDescription("Description")
                    .WithTarget(100)
                    .Build());
        }

        [Fact]
        public void Build_WithNameTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var longName = new string('A', Achievement.NameMaxLength + 1);

            // Act & Assert
            Assert.Throws<ValidationException>(() =>
                Achievement.CreateBuilder()
                    .WithType(AchievementType.FirstWorkout)
                    .WithName(longName)
                    .WithDescription("Description")
                    .WithTarget(100)
                    .Build());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Build_WithInvalidDescription_ShouldThrowValidationException(string invalidDescription)
        {
            // Arrange & Act & Assert
            Assert.Throws<ValidationException>(() =>
                Achievement.CreateBuilder()
                    .WithType(AchievementType.FirstWorkout)
                    .WithName("Name")
                    .WithDescription(invalidDescription)
                    .WithTarget(100)
                    .Build());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Build_WithInvalidTarget_ShouldThrowValidationException(int invalidTarget)
        {
            // Arrange & Act & Assert
            Assert.Throws<ValidationException>(() =>
                Achievement.CreateBuilder()
                    .WithType(AchievementType.FirstWorkout)
                    .WithName("Name")
                    .WithDescription("Description")
                    .WithTarget(invalidTarget)
                    .Build());
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
            Assert.Equal(type, achievement.Type);
        }

        [Fact]
        public void Build_ShouldGenerateCorrectIconUrl()
        {
            // Arrange & Act
            var achievement = AchievementMother.FirstWorkout();

            // Assert
            Assert.Contains("/icons/achievement_", achievement.IconUrl.ToLower());
            Assert.Contains("firstworkout", achievement.IconUrl.ToLower());
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
            Assert.Equal(tier, achievement.Tier);
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
            Assert.Equal(AchievementTier.Bronze, achievement.Tier);
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
            Assert.Equal(AchievementTier.Silver, achievement.Tier);
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
            Assert.Equal(AchievementTier.Gold, achievement.Tier);
        }

        #endregion

        #region UpdateProgress Tests

        [Fact]
        public void UpdateProgress_WithValidProgress_ShouldUpdateProgress()
        {
            // Arrange
            var achievement = AchievementMother.WorkoutStreakSilver();
            var initialUpdatedAt = achievement.UpdatedAt;
            Thread.Sleep(10);

            // Act
            var result = achievement.UpdateProgress(3);

            // Assert
            Assert.Equal(3, achievement.Progress);
            Assert.False(result); // Not unlocked yet
            Assert.False(achievement.IsUnlocked);
            Assert.True(achievement.UpdatedAt > initialUpdatedAt);
        }

        [Fact]
        public void UpdateProgress_ReachingTarget_ShouldUnlockAchievement()
        {
            // Arrange
            var achievement = AchievementMother.FirstWorkout();

            // Act
            var result = achievement.UpdateProgress(1);

            // Assert
            Assert.True(result);
            Assert.True(achievement.IsUnlocked);
            Assert.NotNull(achievement.UnlockedAt);
            Assert.Equal(1, achievement.Progress);
        }

        [Fact]
        public void UpdateProgress_ExceedingTarget_ShouldUnlockAchievement()
        {
            // Arrange
            var achievement = AchievementMother.WorkoutStreakBronze();

            // Act
            var result = achievement.UpdateProgress(10);

            // Assert
            Assert.True(result);
            Assert.True(achievement.IsUnlocked);
            Assert.NotNull(achievement.UnlockedAt);
            Assert.Equal(10, achievement.Progress);
        }

        [Fact]
        public void UpdateProgress_AlreadyUnlocked_ShouldNotReturnTrue()
        {
            // Arrange
            var achievement = AchievementMother.FirstWorkout();
            achievement.UpdateProgress(1); // First unlock
            var unlockedAt = achievement.UnlockedAt;
            Thread.Sleep(10);

            // Act
            var result = achievement.UpdateProgress(2);

            // Assert
            Assert.False(result); // Already unlocked
            Assert.True(achievement.IsUnlocked);
            Assert.Equal(unlockedAt, achievement.UnlockedAt);
        }

        [Fact]
        public void UpdateProgress_FromZeroToTarget_ShouldUnlockInOneStep()
        {
            // Arrange
            var achievement = AchievementMother.TotalWorkoutsBronze();

            // Act
            var result = achievement.UpdateProgress(10);

            // Assert
            Assert.True(result);
            Assert.True(achievement.IsUnlocked);
            Assert.Equal(10, achievement.Progress);
        }

        [Fact]
        public void UpdateProgress_MultipleIncrements_ShouldTrackCorrectly()
        {
            // Arrange
            var achievement = AchievementMother.WorkoutStreakSilver();

            // Act
            achievement.UpdateProgress(2);
            achievement.UpdateProgress(4);
            var result = achievement.UpdateProgress(7);

            // Assert
            Assert.True(result); // Now unlocked
            Assert.Equal(7, achievement.Progress);
            Assert.True(achievement.IsUnlocked);
        }

        [Fact]
        public void UpdateProgress_ShouldAllowDecrement()
        {
            // Arrange
            var achievement = AchievementMother.Default();
            achievement.UpdateProgress(50);

            // Act
            achievement.UpdateProgress(30);

            // Assert
            Assert.Equal(30, achievement.Progress);
            Assert.False(achievement.IsUnlocked);
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
            Assert.Equal(expectedPercentage, percentage);
        }

        [Fact]
        public void GetProgressPercentage_WithZeroProgress_ShouldReturnZero()
        {
            // Arrange
            var achievement = AchievementMother.Default();

            // Act
            var percentage = achievement.GetProgressPercentage();

            // Assert
            Assert.Equal(0, percentage);
        }

        [Fact]
        public void GetProgressPercentage_WhenCompleted_ShouldReturn100()
        {
            // Arrange
            var achievement = AchievementMother.FirstWorkout();
            achievement.UpdateProgress(1);

            // Act
            var percentage = achievement.GetProgressPercentage();

            // Assert
            Assert.Equal(100, percentage);
        }

        #endregion

        #region Object Mother Tests

        [Fact]
        public void FirstWorkout_ShouldCreateCorrectAchievement()
        {
            // Arrange & Act
            var achievement = AchievementMother.FirstWorkout();

            // Assert
            Assert.Equal("First Steps", achievement.Name);
            Assert.Equal(1, achievement.Target);
            Assert.Equal(AchievementType.FirstWorkout, achievement.Type);
            Assert.Equal(AchievementTier.Bronze, achievement.Tier);
        }

        [Fact]
        public void WorkoutStreakGold_ShouldCreateGoldAchievement()
        {
            // Arrange & Act
            var achievement = AchievementMother.WorkoutStreakGold();

            // Assert
            Assert.Equal("30-Day Streak", achievement.Name);
            Assert.Equal(30, achievement.Target);
            Assert.Equal(AchievementTier.Gold, achievement.Tier);
        }

        [Fact]
        public void TotalWorkoutsTitan_ShouldCreateTitanAchievement()
        {
            // Arrange & Act
            var achievement = AchievementMother.TotalWorkoutsTitan();

            // Assert
            Assert.Equal(AchievementType.TotalWorkouts, achievement.Type);
            Assert.Equal(1000, achievement.Target);
            Assert.Equal(AchievementTier.Titan, achievement.Tier);
        }

        [Fact]
        public void MaxWeightEmerald_ShouldCreateEmeraldAchievement()
        {
            // Arrange & Act
            var achievement = AchievementMother.MaxWeightEmerald();

            // Assert
            Assert.Equal(AchievementType.MaxWeight, achievement.Type);
            Assert.Equal(200, achievement.Target);
            Assert.Equal(AchievementTier.Emerald, achievement.Tier);
        }

        [Fact]
        public void Unlocked_ShouldReturnUnlockedAchievement()
        {
            // Arrange & Act
            var achievement = AchievementMother.Unlocked();

            // Assert
            Assert.True(achievement.IsUnlocked);
            Assert.NotNull(achievement.UnlockedAt);
            Assert.Equal(achievement.Target, achievement.Progress);
        }

        [Fact]
        public void HalfwayComplete_ShouldHave50PercentProgress()
        {
            // Arrange & Act
            var achievement = AchievementMother.HalfwayComplete();

            // Assert
            Assert.Equal(50, achievement.GetProgressPercentage());
            Assert.False(achievement.IsUnlocked);
        }

        [Fact]
        public void AlmostComplete_ShouldHave99PercentProgress()
        {
            // Arrange & Act
            var achievement = AchievementMother.AlmostComplete();

            // Assert
            Assert.Equal(99, achievement.GetProgressPercentage());
            Assert.False(achievement.IsUnlocked);
        }

        #endregion

        #region Collection Tests

        [Fact]
        public void BronzeCollection_ShouldContainOnlyBronze()
        {
            // Arrange & Act
            var achievements = AchievementMother.BronzeCollection();

            // Assert
            Assert.Equal(8, achievements.Count);
            Assert.All(achievements, a => Assert.Equal(AchievementTier.Bronze, a.Tier));
        }

        [Fact]
        public void AllTiersCollection_ShouldContainAllTiers()
        {
            // Arrange & Act
            var achievements = AchievementMother.AllTiersCollection();

            // Assert
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Bronze);
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Silver);
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Gold);
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Platinum);
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Diamond);
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Emerald);
            Assert.Contains(achievements, a => a.Tier == AchievementTier.Titan);
        }

        [Fact]
        public void WorkoutProgressionPath_ShouldHaveIncreasingTargets()
        {
            // Arrange & Act
            var achievements = AchievementMother.WorkoutProgressionPath();

            // Assert
            Assert.Equal(4, achievements.Count);
            Assert.Equal(1, achievements[0].Target);
            Assert.Equal(10, achievements[1].Target);
            Assert.Equal(100, achievements[2].Target);
            Assert.Equal(1000, achievements[3].Target);
        }

        [Fact]
        public void StreakProgressionPath_ShouldHaveIncreasingDifficulty()
        {
            // Arrange & Act
            var achievements = AchievementMother.StreakProgressionPath();

            // Assert
            Assert.Equal(3, achievements.Count);
            Assert.All(achievements, a => Assert.Equal(AchievementType.WorkoutStreak, a.Type));
            Assert.True(achievements[0].Target < achievements[1].Target);
            Assert.True(achievements[1].Target < achievements[2].Target);
        }

        [Fact]
        public void MixedProgressCollection_ShouldHaveVariedProgress()
        {
            // Arrange & Act
            var achievements = AchievementMother.MixedProgressCollection();

            // Assert
            Assert.Contains(achievements, a => a.IsUnlocked);
            Assert.Contains(achievements, a => !a.IsUnlocked && a.Progress > 0);
            Assert.Contains(achievements, a => a.Progress == 0);
        }

        [Fact]
        public void CompleteAchievementSystem_ShouldContainAllTypes()
        {
            // Arrange & Act
            var achievements = AchievementMother.CompleteAchievementSystem();

            // Assert
            Assert.True(achievements.Count >= 13); // At least one of each type
            Assert.Contains(achievements, a => a.Type == AchievementType.FirstWorkout);
            Assert.Contains(achievements, a => a.Type == AchievementType.WorkoutStreak);
            Assert.Contains(achievements, a => a.Type == AchievementType.TotalWorkouts);
            Assert.Contains(achievements, a => a.Type == AchievementType.TotalVolume);
            Assert.Contains(achievements, a => a.Type == AchievementType.MaxWeight);
            Assert.Contains(achievements, a => a.Type == AchievementType.ConsecutiveDays);
            Assert.Contains(achievements, a => a.Type == AchievementType.ExerciseVariety);
            Assert.Contains(achievements, a => a.Type == AchievementType.PerfectForm);
            Assert.Contains(achievements, a => a.Type == AchievementType.EarlyBird);
            Assert.Contains(achievements, a => a.Type == AchievementType.NightOwl);
        }

        #endregion

        #region UserId Tests

        [Fact]
        public void WithUserId_ShouldSetCorrectUserId()
        {
            // Arrange & Act
            var achievement = AchievementMother.WithUserId(_testUserId);

            // Assert
            Assert.Equal(_testUserId, achievement.UserId);
        }

        [Fact]
        public void ForUser_ShouldCreateMultipleAchievementsForUser()
        {
            // Arrange & Act
            var achievements = AchievementMother.ForUser(_testUserId);

            // Assert
            Assert.All(achievements, a => Assert.Equal(_testUserId, a.UserId));
            Assert.True(achievements.Count > 0);
        }

        #endregion
    }
}
