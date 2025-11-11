// Domain.Tests/Entities/ExerciseTests.cs
using System;
using System.Threading;
using Xunit;
using FluentValidation;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Tests.Factories;
using FitTrackerDomain.Tests.Factories;

namespace FitTracker.Domain.Tests.Entities
{
    public class ExerciseTests
    {
        private readonly Guid _testUserId = Guid.NewGuid();

        #region Builder Tests

        [Fact]
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            // Act
            var builder = Exercise.CreateBuilder();

            // Assert
            Assert.NotNull(builder);
            Assert.IsType<Exercise.ExerciseBuilder>(builder);
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.Default();

            // Assert
            Assert.NotNull(exercise);
            Assert.Equal("Bench Press", exercise.Name);
            Assert.Equal("Standard bench press exercise", exercise.Description);
            Assert.Equal(MuscleGroup.Chest, exercise.MuscleGroup);
            Assert.Equal(Equipment.Barbell, exercise.Equipment);
            Assert.False(exercise.IsCustom);
            Assert.Null(exercise.UserId);
        }

        [Fact]
        public void Build_ShouldGenerateId()
        {
            // Arrange & Act
            var exercise = ExerciseMother.Default();

            // Assert
            Assert.NotEqual(Guid.Empty, exercise.Id);
        }

        [Fact]
        public void Build_ShouldSetTimestamps()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var exercise = ExerciseMother.Default();
            var after = DateTime.UtcNow;

            // Assert
            Assert.True(exercise.CreatedAt >= before);
            Assert.True(exercise.CreatedAt <= after);
            Assert.True(exercise.UpdatedAt >= before);
            Assert.True(exercise.UpdatedAt <= after);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Build_WithInvalidName_ShouldThrowValidationException(string invalidName)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
                Exercise.CreateBuilder()
                    .WithName(invalidName)
                    .WithMuscleGroup(MuscleGroup.Chest)
                    .WithEquipment(Equipment.Barbell)
                    .Build());

            Assert.Contains("name", exception.Message.ToLower());
        }

        [Fact]
        public void Build_WithNameTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var longName = new string('A', Exercise.NameMaxLength + 1);

            // Act & Assert
            Assert.Throws<ValidationException>(() =>
                Exercise.CreateBuilder()
                    .WithName(longName)
                    .WithMuscleGroup(MuscleGroup.Chest)
                    .WithEquipment(Equipment.Barbell)
                    .Build());
        }

        #endregion

        #region Standard Exercise Tests

        [Fact]
        public void Build_AsStandard_ShouldCreateStandardExercise()
        {
            // Arrange & Act
            var exercise = Exercise.CreateBuilder()
                .WithName("Standard Exercise")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Barbell)
                .AsStandard()
                .Build();

            // Assert
            Assert.False(exercise.IsCustom);
            Assert.Null(exercise.UserId);
        }

        [Fact]
        public void BenchPress_ShouldCreateCorrectExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.BenchPress();

            // Assert
            Assert.Equal("Barbell Bench Press", exercise.Name);
            Assert.Equal(MuscleGroup.Chest, exercise.MuscleGroup);
            Assert.Equal(Equipment.Barbell, exercise.Equipment);
            Assert.False(exercise.IsCustom);
            Assert.NotNull(exercise.ImageUrl);
            Assert.NotNull(exercise.VideoUrl);
        }

        [Fact]
        public void Deadlift_ShouldCreateCorrectExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.Deadlift();

            // Assert
            Assert.Equal("Barbell Deadlift", exercise.Name);
            Assert.Equal(MuscleGroup.Back, exercise.MuscleGroup);
            Assert.Equal(Equipment.Barbell, exercise.Equipment);
        }

        [Fact]
        public void Squat_ShouldCreateCorrectExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.Squat();

            // Assert
            Assert.Equal("Barbell Squat", exercise.Name);
            Assert.Equal(MuscleGroup.Legs, exercise.MuscleGroup);
        }

        #endregion

        #region Custom Exercise Tests

        [Fact]
        public void Build_AsCustomWithUserId_ShouldCreateCustomExercise()
        {
            // Arrange & Act
            var exercise = Exercise.CreateBuilder()
                .WithName("Custom Exercise")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Dumbbell)
                .AsCustom(_testUserId)
                .Build();

            // Assert
            Assert.True(exercise.IsCustom);
            Assert.Equal(_testUserId, exercise.UserId);
        }

        [Fact]
        public void Build_AsCustomWithoutUserId_ShouldThrowArgumentException()
        {
            var builder = Exercise.CreateBuilder()
                .WithName("Custom Exercise")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Dumbbell)
                .AsCustom(Guid.Empty);

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() => builder.Build());

            Assert.Contains("user", exception.Message.ToLower());
        }

        [Fact]
        public void CustomExercise_ShouldHaveCorrectUserId()
        {
            // Arrange & Act
            var exercise = ExerciseMother.CustomExercise(_testUserId);

            // Assert
            Assert.True(exercise.IsCustom);
            Assert.Equal(_testUserId, exercise.UserId);
        }

        [Fact]
        public void CustomWithAllFields_ShouldHaveAllProperties()
        {
            // Arrange & Act
            var exercise = ExerciseMother.CustomWithAllFields(_testUserId);

            // Assert
            Assert.True(exercise.IsCustom);
            Assert.Equal(_testUserId, exercise.UserId);
            Assert.NotNull(exercise.ImageUrl);
            Assert.NotNull(exercise.VideoUrl);
            Assert.NotNull(exercise.Description);
        }

        #endregion

        #region Muscle Group Tests

        [Theory]
        [InlineData(MuscleGroup.Chest)]
        [InlineData(MuscleGroup.Back)]
        [InlineData(MuscleGroup.Legs)]
        [InlineData(MuscleGroup.Shoulders)]
        [InlineData(MuscleGroup.Biceps)]
        [InlineData(MuscleGroup.Triceps)]
        [InlineData(MuscleGroup.Abs)]
        [InlineData(MuscleGroup.Glutes)]
        [InlineData(MuscleGroup.Calves)]
        [InlineData(MuscleGroup.Forearms)]
        public void Build_WithDifferentMuscleGroups_ShouldSetCorrectly(MuscleGroup muscleGroup)
        {
            // Arrange & Act
            var exercise = Exercise.CreateBuilder()
                .WithName("Test Exercise")
                .WithMuscleGroup(muscleGroup)
                .WithEquipment(Equipment.Barbell)
                .Build();

            // Assert
            Assert.Equal(muscleGroup, exercise.MuscleGroup);
        }

        #endregion

        #region Equipment Tests

        [Theory]
        [InlineData(Equipment.Barbell)]
        [InlineData(Equipment.Dumbbell)]
        [InlineData(Equipment.Bodyweight)]
        [InlineData(Equipment.Machine)]
        [InlineData(Equipment.Cable)]
        [InlineData(Equipment.Kettlebell)]
        [InlineData(Equipment.TRX)]
        [InlineData(Equipment.Other)]
        public void Build_WithDifferentEquipment_ShouldSetCorrectly(Equipment equipment)
        {
            // Arrange & Act
            var exercise = Exercise.CreateBuilder()
                .WithName("Test Exercise")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(equipment)
                .Build();

            // Assert
            Assert.Equal(equipment, exercise.Equipment);
        }

        [Fact]
        public void WithBarbell_ShouldUseBarbell()
        {
            // Arrange & Act
            var exercise = ExerciseMother.WithBarbell();

            // Assert
            Assert.Equal(Equipment.Barbell, exercise.Equipment);
        }

        [Fact]
        public void WithBodyweight_ShouldUseBodyweight()
        {
            // Arrange & Act
            var exercise = ExerciseMother.WithBodyweight();

            // Assert
            Assert.Equal(Equipment.Bodyweight, exercise.Equipment);
        }

        #endregion

        #region Media Tests

        [Fact]
        public void Build_WithImageUrl_ShouldSetImageUrl()
        {
            // Arrange & Act
            var exercise = Exercise.CreateBuilder()
                .WithName("Test")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Barbell)
                .WithImageUrl("https://example.com/image.jpg")
                .Build();

            // Assert
            Assert.Equal("https://example.com/image.jpg", exercise.ImageUrl);
        }

        [Fact]
        public void Build_WithVideoUrl_ShouldSetVideoUrl()
        {
            // Arrange & Act
            var exercise = Exercise.CreateBuilder()
                .WithName("Test")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Barbell)
                .WithVideoUrl("https://example.com/video.mp4")
                .Build();

            // Assert
            Assert.Equal("https://example.com/video.mp4", exercise.VideoUrl);
        }

        [Fact]
        public void WithImage_ShouldHaveImageOnly()
        {
            // Arrange & Act
            var exercise = ExerciseMother.WithImage();

            // Assert
            Assert.NotNull(exercise.ImageUrl);
            Assert.Null(exercise.VideoUrl);
        }

        [Fact]
        public void WithVideo_ShouldHaveVideoOnly()
        {
            // Arrange & Act
            var exercise = ExerciseMother.WithVideo();

            // Assert
            Assert.Null(exercise.ImageUrl);
            Assert.NotNull(exercise.VideoUrl);
        }

        [Fact]
        public void WithImageAndVideo_ShouldHaveBoth()
        {
            // Arrange & Act
            var exercise = ExerciseMother.WithImageAndVideo();

            // Assert
            Assert.NotNull(exercise.ImageUrl);
            Assert.NotNull(exercise.VideoUrl);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_WithValidData_ShouldUpdateExercise()
        {
            // Arrange
            var exercise = ExerciseMother.Default();
            var originalUpdatedAt = exercise.UpdatedAt;
            Thread.Sleep(10);

            // Act
            exercise.Update(
                "Updated Name",
                MuscleGroup.Back,
                Equipment.Dumbbell,
                "Updated description");

            // Assert
            Assert.Equal("Updated Name", exercise.Name);
            Assert.Equal(MuscleGroup.Back, exercise.MuscleGroup);
            Assert.Equal(Equipment.Dumbbell, exercise.Equipment);
            Assert.Equal("Updated description", exercise.Description);
            Assert.True(exercise.UpdatedAt > originalUpdatedAt);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Update_WithInvalidName_ShouldThrowValidationException(string invalidName)
        {
            // Arrange
            var exercise = ExerciseMother.Default();

            // Act & Assert
            var exception = Assert.Throws<ValidationException>(() =>
                exercise.Update(invalidName, MuscleGroup.Chest, Equipment.Barbell));

            Assert.Contains("name", exception.Message.ToLower());
        }

        [Fact]
        public void UpdateImageUrl_ShouldUpdateImage()
        {
            // Arrange
            var exercise = ExerciseMother.Default();
            var originalUpdatedAt = exercise.UpdatedAt;
            Thread.Sleep(10);

            // Act
            exercise.UpdateImageUrl("https://example.com/new-image.jpg");

            // Assert
            Assert.Equal("https://example.com/new-image.jpg", exercise.ImageUrl);
            Assert.True(exercise.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void UpdateImageUrl_WithNull_ShouldSetNull()
        {
            // Arrange
            var exercise = ExerciseMother.WithImage();

            // Act
            exercise.UpdateImageUrl(null);

            // Assert
            Assert.Null(exercise.ImageUrl);
        }

        [Fact]
        public void UpdateVideoUrl_ShouldUpdateVideo()
        {
            // Arrange
            var exercise = ExerciseMother.Default();
            var originalUpdatedAt = exercise.UpdatedAt;
            Thread.Sleep(10);

            // Act
            exercise.UpdateVideoUrl("https://example.com/new-video.mp4");

            // Assert
            Assert.Equal("https://example.com/new-video.mp4", exercise.VideoUrl);
            Assert.True(exercise.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void UpdateVideoUrl_WithNull_ShouldSetNull()
        {
            // Arrange
            var exercise = ExerciseMother.WithVideo();

            // Act
            exercise.UpdateVideoUrl(null);

            // Assert
            Assert.Null(exercise.VideoUrl);
        }

        #endregion

        #region Collection Tests

        [Fact]
        public void ChestExercises_ShouldContainOnlyChestExercises()
        {
            // Arrange & Act
            var exercises = ExerciseMother.ChestExercises();

            // Assert
            Assert.Equal(3, exercises.Count);
            Assert.All(exercises, e => Assert.Equal(MuscleGroup.Chest, e.MuscleGroup));
        }

        [Fact]
        public void BackExercises_ShouldContainOnlyBackExercises()
        {
            // Arrange & Act
            var exercises = ExerciseMother.BackExercises();

            // Assert
            Assert.Equal(3, exercises.Count);
            Assert.All(exercises, e => Assert.Equal(MuscleGroup.Back, e.MuscleGroup));
        }

        [Fact]
        public void BodyweightExercises_ShouldContainOnlyBodyweight()
        {
            // Arrange & Act
            var exercises = ExerciseMother.BodyweightExercises();

            // Assert
            Assert.True(exercises.Count > 0);
            Assert.All(exercises, e => Assert.Equal(Equipment.Bodyweight, e.Equipment));
        }

        [Fact]
        public void CompoundExercises_ShouldContainMajorLifts()
        {
            // Arrange & Act
            var exercises = ExerciseMother.CompoundExercises();

            // Assert
            Assert.Contains(exercises, e => e.Name.Contains("Bench Press"));
            Assert.Contains(exercises, e => e.Name.Contains("Deadlift"));
            Assert.Contains(exercises, e => e.Name.Contains("Squat"));
        }

        [Fact]
        public void AllStandardExercises_ShouldBeStandard()
        {
            // Arrange & Act
            var exercises = ExerciseMother.AllStandardExercises();

            // Assert
            Assert.True(exercises.Count > 10);
            Assert.All(exercises, e => Assert.False(e.IsCustom));
            Assert.All(exercises, e => Assert.Null(e.UserId));
        }

        [Fact]
        public void MixedCollection_ShouldContainBothStandardAndCustom()
        {
            // Arrange & Act
            var exercises = ExerciseMother.MixedCollection(_testUserId);

            // Assert
            Assert.Contains(exercises, e => !e.IsCustom);
            Assert.Contains(exercises, e => e.IsCustom);
            Assert.Contains(exercises, e => e.UserId == _testUserId);
        }

        #endregion

        #region Object Mother Tests

        [Fact]
        public void PushUp_ShouldBeBodyweightChestExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.PushUp();

            // Assert
            Assert.Equal("Push-Up", exercise.Name);
            Assert.Equal(MuscleGroup.Chest, exercise.MuscleGroup);
            Assert.Equal(Equipment.Bodyweight, exercise.Equipment);
            Assert.False(exercise.IsCustom);
        }

        [Fact]
        public void PullUp_ShouldBeBodyweightBackExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.PullUp();

            // Assert
            Assert.Equal("Pull-Up", exercise.Name);
            Assert.Equal(MuscleGroup.Back, exercise.MuscleGroup);
            Assert.Equal(Equipment.Bodyweight, exercise.Equipment);
        }

        [Fact]
        public void LegPress_ShouldBeMachineExercise()
        {
            // Arrange & Act
            var exercise = ExerciseMother.LegPress();

            // Assert
            Assert.Equal(MuscleGroup.Legs, exercise.MuscleGroup);
            Assert.Equal(Equipment.Machine, exercise.Equipment);
        }

        #endregion
    }
}
