using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTrackerDomain.Tests.Factories;
using FluentAssertions;
using FluentValidation;

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
            builder.Should().NotBeNull();
            builder.Should().BeOfType<Exercise.ExerciseBuilder>();
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.Default();

            // Assert
            exercise.Should().NotBeNull();
            exercise.Name.Should().Be("Bench Press");
            exercise.Description.Should().Be("Standard bench press exercise");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Chest);
            exercise.Equipment.Should().Be(Equipment.Barbell);
            exercise.IsCustom.Should().BeFalse();
            exercise.UserId.Should().BeNull();
        }

        [Fact]
        public void Build_ShouldGenerateId()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.Default();

            // Assert
            exercise.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void Build_ShouldSetTimestamps()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var exercise = ExerciseFactory.Default();
            var after = DateTime.UtcNow;

            // Assert
            exercise.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            exercise.UpdatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Build_WithInvalidName_ShouldThrowValidationException(string invalidName)
        {
            // Arrange & Act
            Action act = () => Exercise.CreateBuilder()
                .WithName(invalidName)
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Barbell)
                .Build();

            // Assert
            act.Should().Throw<ValidationException>().WithMessage("*name*");
        }

        [Fact]
        public void Build_WithNameTooLong_ShouldThrowValidationException()
        {
            // Arrange
            var longName = new string('A', Exercise.NameMaxLength + 1);

            // Act
            Action act = () => Exercise.CreateBuilder()
                .WithName(longName)
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Barbell)
                .Build();

            // Assert
            act.Should().Throw<ValidationException>();
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
            exercise.IsCustom.Should().BeFalse();
            exercise.UserId.Should().BeNull();
        }

        [Fact]
        public void BenchPress_ShouldCreateCorrectExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.BenchPress();

            // Assert
            exercise.Name.Should().Be("Barbell Bench Press");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Chest);
            exercise.Equipment.Should().Be(Equipment.Barbell);
            exercise.IsCustom.Should().BeFalse();
            exercise.ImageUrl.Should().NotBeNullOrEmpty();
            exercise.VideoUrl.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Deadlift_ShouldCreateCorrectExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.Deadlift();

            // Assert
            exercise.Name.Should().Be("Barbell Deadlift");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Back);
            exercise.Equipment.Should().Be(Equipment.Barbell);
        }

        [Fact]
        public void Squat_ShouldCreateCorrectExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.Squat();

            // Assert
            exercise.Name.Should().Be("Barbell Squat");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Legs);
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
            exercise.IsCustom.Should().BeTrue();
            exercise.UserId.Should().Be(_testUserId);
        }

        [Fact]
        public void Build_AsCustomWithoutUserId_ShouldThrowValidationException()
        {
            var builder = Exercise.CreateBuilder()
                .WithName("Custom Exercise")
                .WithMuscleGroup(MuscleGroup.Chest)
                .WithEquipment(Equipment.Dumbbell)
                .AsCustom(Guid.Empty);

            // Act
            Action act = () => builder.Build();

            // Assert
            act.Should().Throw<ValidationException>().WithMessage("*user*");
        }

        [Fact]
        public void CustomExercise_ShouldHaveCorrectUserId()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.CustomExercise(_testUserId);

            // Assert
            exercise.IsCustom.Should().BeTrue();
            exercise.UserId.Should().Be(_testUserId);
        }

        [Fact]
        public void CustomWithAllFields_ShouldHaveAllProperties()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.CustomWithAllFields(_testUserId);

            // Assert
            exercise.IsCustom.Should().BeTrue();
            exercise.UserId.Should().Be(_testUserId);
            exercise.ImageUrl.Should().NotBeNullOrEmpty();
            exercise.VideoUrl.Should().NotBeNullOrEmpty();
            exercise.Description.Should().NotBeNullOrEmpty();
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
            exercise.MuscleGroup.Should().Be(muscleGroup);
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
            exercise.Equipment.Should().Be(equipment);
        }

        [Fact]
        public void WithBarbell_ShouldUseBarbell()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.WithBarbell();

            // Assert
            exercise.Equipment.Should().Be(Equipment.Barbell);
        }

        [Fact]
        public void WithBodyweight_ShouldUseBodyweight()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.WithBodyweight();

            // Assert
            exercise.Equipment.Should().Be(Equipment.Bodyweight);
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
            exercise.ImageUrl.Should().Be("https://example.com/image.jpg");
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
            exercise.VideoUrl.Should().Be("https://example.com/video.mp4");
        }

        [Fact]
        public void WithImage_ShouldHaveImageOnly()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.WithImage();

            // Assert
            exercise.ImageUrl.Should().NotBeNullOrEmpty();
            exercise.VideoUrl.Should().BeNull();
        }

        [Fact]
        public void WithVideo_ShouldHaveVideoOnly()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.WithVideo();

            // Assert
            exercise.ImageUrl.Should().BeNull();
            exercise.VideoUrl.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void WithImageAndVideo_ShouldHaveBoth()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.WithImageAndVideo();

            // Assert
            exercise.ImageUrl.Should().NotBeNullOrEmpty();
            exercise.VideoUrl.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_WithValidData_ShouldUpdateExercise()
        {
            // Arrange
            var exercise = ExerciseFactory.Default();
            var originalUpdatedAt = exercise.UpdatedAt;
            Thread.Sleep(10);

            // Act
            exercise.Update(
                "Updated Name",
                MuscleGroup.Back,
                Equipment.Dumbbell,
                "Updated description");

            // Assert
            exercise.Name.Should().Be("Updated Name");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Back);
            exercise.Equipment.Should().Be(Equipment.Dumbbell);
            exercise.Description.Should().Be("Updated description");
            exercise.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Update_WithInvalidName_ShouldThrowValidationException(string invalidName)
        {
            // Arrange
            var exercise = ExerciseFactory.Default();

            // Act
            Action act = () => exercise.Update(invalidName, MuscleGroup.Chest, Equipment.Barbell);

            // Assert
            act.Should().Throw<ValidationException>().WithMessage("*name*");
        }

        [Fact]
        public void UpdateImageUrl_ShouldUpdateImage()
        {
            // Arrange
            var exercise = ExerciseFactory.Default();
            var originalUpdatedAt = exercise.UpdatedAt;
            Thread.Sleep(10);

            // Act
            exercise.UpdateImageUrl("https://example.com/new-image.jpg");

            // Assert
            exercise.ImageUrl.Should().Be("https://example.com/new-image.jpg");
            exercise.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void UpdateImageUrl_WithNull_ShouldSetNull()
        {
            // Arrange
            var exercise = ExerciseFactory.WithImage();

            // Act
            exercise.UpdateImageUrl(null);

            // Assert
            exercise.ImageUrl.Should().BeNull();
        }

        [Fact]
        public void UpdateVideoUrl_ShouldUpdateVideo()
        {
            // Arrange
            var exercise = ExerciseFactory.Default();
            var originalUpdatedAt = exercise.UpdatedAt;
            Thread.Sleep(10);

            // Act
            exercise.UpdateVideoUrl("https://example.com/new-video.mp4");

            // Assert
            exercise.VideoUrl.Should().Be("https://example.com/new-video.mp4");
            exercise.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void UpdateVideoUrl_WithNull_ShouldSetNull()
        {
            // Arrange
            var exercise = ExerciseFactory.WithVideo();

            // Act
            exercise.UpdateVideoUrl(null);

            // Assert
            exercise.VideoUrl.Should().BeNull();
        }

        #endregion

        #region Collection Tests

        [Fact]
        public void ChestExercises_ShouldContainOnlyChestExercises()
        {
            // Arrange & Act
            var exercises = ExerciseFactory.ChestExercises();

            // Assert
            exercises.Count.Should().Be(3);
            exercises.Should().OnlyContain(e => e.MuscleGroup == MuscleGroup.Chest);
        }

        [Fact]
        public void BackExercises_ShouldContainOnlyBackExercises()
        {
            // Arrange & Act
            var exercises = ExerciseFactory.BackExercises();

            // Assert
            exercises.Count.Should().Be(3);
            exercises.Should().OnlyContain(e => e.MuscleGroup == MuscleGroup.Back);
        }

        [Fact]
        public void BodyweightExercises_ShouldContainOnlyBodyweight()
        {
            // Arrange & Act
            var exercises = ExerciseFactory.BodyweightExercises();

            // Assert
            exercises.Count.Should().BeGreaterThan(0);
            exercises.Should().OnlyContain(e => e.Equipment == Equipment.Bodyweight);
        }

        [Fact]
        public void CompoundExercises_ShouldContainMajorLifts()
        {
            // Arrange & Act
            var exercises = ExerciseFactory.CompoundExercises();

            // Assert
            exercises.Should().Contain(e => e.Name.Contains("Bench Press"));
            exercises.Should().Contain(e => e.Name.Contains("Deadlift"));
            exercises.Should().Contain(e => e.Name.Contains("Squat"));
        }

        [Fact]
        public void AllStandardExercises_ShouldBeStandard()
        {
            // Arrange & Act
            var exercises = ExerciseFactory.AllStandardExercises();

            // Assert
            exercises.Count.Should().BeGreaterThan(10);
            exercises.Should().OnlyContain(e => !e.IsCustom);
            exercises.Should().OnlyContain(e => e.UserId == null);
        }

        [Fact]
        public void MixedCollection_ShouldContainBothStandardAndCustom()
        {
            // Arrange & Act
            var exercises = ExerciseFactory.MixedCollection(_testUserId);

            // Assert
            exercises.Should().Contain(e => !e.IsCustom);
            exercises.Should().Contain(e => e.IsCustom && e.UserId == _testUserId);
        }

        #endregion

        #region Object Mother Tests

        [Fact]
        public void PushUp_ShouldBeBodyweightChestExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.PushUp();

            // Assert
            exercise.Name.Should().Be("Push-Up");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Chest);
            exercise.Equipment.Should().Be(Equipment.Bodyweight);
            exercise.IsCustom.Should().BeFalse();
        }

        [Fact]
        public void PullUp_ShouldBeBodyweightBackExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.PullUp();

            // Assert
            exercise.Name.Should().Be("Pull-Up");
            exercise.MuscleGroup.Should().Be(MuscleGroup.Back);
            exercise.Equipment.Should().Be(Equipment.Bodyweight);
        }

        [Fact]
        public void LegPress_ShouldBeMachineExercise()
        {
            // Arrange & Act
            var exercise = ExerciseFactory.LegPress();

            // Assert
            exercise.MuscleGroup.Should().Be(MuscleGroup.Legs);
            exercise.Equipment.Should().Be(Equipment.Machine);
        }

        #endregion
    }
}
