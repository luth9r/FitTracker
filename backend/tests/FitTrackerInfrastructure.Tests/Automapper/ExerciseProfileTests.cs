using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTrackerInfrastructure.Tests.TestDoubles;
using FluentAssertions;

namespace FitTrackerInfrastructure.Tests.Automapper;

public class ExerciseProfileTests
{
    private readonly IMapper _mapper;

    public ExerciseProfileTests()
    {
        var config = MapperConfigurationHelper.Create<ExerciseProfile>();

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        // Act & Assert
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_ExerciseEf_To_Exercise()
    {
        // Arrange
        var exerciseEf = new ExerciseEf
        {
            Id = Guid.NewGuid(),
            Name = "Bench Press",
            Description = "A chest exercise",
            ImageUrl = "https://example.com/bench.jpg",
            VideoUrl = "https://example.com/bench.mp4",
            MuscleGroup = 0, // Chest
            Equipment = 1, // Dumbbell
            CreatedByUserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var exercise = _mapper.Map<Exercise>(exerciseEf);

        // Assert
        exercise.Should().NotBeNull();
        exercise.Id.Should().Be(exerciseEf.Id);
        exercise.Name.Should().Be(exerciseEf.Name);
        exercise.Description.Should().Be(exerciseEf.Description);
        exercise.ImageUrl.Should().Be(exerciseEf.ImageUrl);
        exercise.VideoUrl.Should().Be(exerciseEf.VideoUrl);
        exercise.MuscleGroup.Should().Be(MuscleGroup.Chest);
        exercise.Equipment.Should().Be(Equipment.Dumbbell);
        exercise.CreatedByUserId.Should().Be(exerciseEf.CreatedByUserId);
        exercise.CreatedAt.Should().Be(exerciseEf.CreatedAt);
        exercise.UpdatedAt.Should().Be(exerciseEf.UpdatedAt);
    }

    [Fact]
    public void Should_Map_Exercise_To_ExerciseEf()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var exercise = Exercise.CreateCustom(
            userId,
            "Squat",
            MuscleGroup.Legs,
            Equipment.Barbell,
            "A leg exercise",
            "https://example.com/squat.jpg",
            "https://example.com/squat.mp4");

        // Act
        var exerciseEf = _mapper.Map<ExerciseEf>(exercise);

        // Assert
        exerciseEf.Should().NotBeNull();
        exerciseEf.Id.Should().Be(exercise.Id);
        exerciseEf.Name.Should().Be(exercise.Name);
        exerciseEf.Description.Should().Be(exercise.Description);
        exerciseEf.ImageUrl.Should().Be(exercise.ImageUrl);
        exerciseEf.VideoUrl.Should().Be(exercise.VideoUrl);
        exerciseEf.MuscleGroup.Should().Be((int)MuscleGroup.Legs);
        exerciseEf.Equipment.Should().Be((int)Equipment.Barbell);
        exerciseEf.CreatedByUserId.Should().Be(userId);
    }

    [Fact]
    public void Should_Map_Exercise_With_Null_Optional_Fields_To_ExerciseEf()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var exercise = Exercise.CreateCustom(
            userId,
            "Push-up",
            MuscleGroup.Chest,
            Equipment.None);

        // Act
        var exerciseEf = _mapper.Map<ExerciseEf>(exercise);

        // Assert
        exerciseEf.Should().NotBeNull();
        exerciseEf.Name.Should().Be("Push-up");
        exerciseEf.Description.Should().BeNull();
        exerciseEf.ImageUrl.Should().BeNull();
        exerciseEf.VideoUrl.Should().BeNull();
    }

    [Fact]
    public void Should_Transfer_DomainEvents_When_Mapping_Exercise_To_ExerciseEf()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var exercise = Exercise.CreateCustom(
            userId,
            "Deadlift",
            MuscleGroup.Back,
            Equipment.Barbell);

        // Act
        var exerciseEf = _mapper.Map<ExerciseEf>(exercise);

        // Assert
        exerciseEf.DomainEvents.Should().HaveCount(exercise.DomainEvents.Count);
    }

    [Theory]
    [InlineData(MuscleGroup.Chest, 0)]
    [InlineData(MuscleGroup.Back, 1)]
    [InlineData(MuscleGroup.Shoulders, 2)]
    [InlineData(MuscleGroup.Biceps, 3)]
    [InlineData(MuscleGroup.Triceps, 4)]
    [InlineData(MuscleGroup.Forearms, 5)]
    public void Should_Map_MuscleGroup_Enum_Correctly(MuscleGroup muscleGroup, int expectedValue)
    {
        // Arrange
        var exercise = Exercise.CreateCustom(
            Guid.NewGuid(),
            "Test Exercise",
            muscleGroup,
            Equipment.None);

        // Act
        var exerciseEf = _mapper.Map<ExerciseEf>(exercise);

        // Assert
        exerciseEf.MuscleGroup.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(Equipment.Barbell, 0)]
    [InlineData(Equipment.Dumbbell, 1)]
    [InlineData(Equipment.Machine, 2)]
    [InlineData(Equipment.Cable, 3)]
    [InlineData(Equipment.Bodyweight, 4)]
    public void Should_Map_Equipment_Enum_Correctly(Equipment equipment, int expectedValue)
    {
        // Arrange
        var exercise = Exercise.CreateCustom(
            Guid.NewGuid(),
            "Test Exercise",
            MuscleGroup.Chest,
            equipment);

        // Act
        var exerciseEf = _mapper.Map<ExerciseEf>(exercise);

        // Assert
        exerciseEf.Equipment.Should().Be(expectedValue);
    }
}
