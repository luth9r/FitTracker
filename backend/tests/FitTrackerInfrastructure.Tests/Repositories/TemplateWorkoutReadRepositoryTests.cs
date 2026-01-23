using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities.TemplateAggregate;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;

namespace FitTrackerInfrastructure.Tests.Repositories;

public class TemplateWorkoutReadRepositoryTests : RepositoryTestBase
{
    private readonly IWorkoutTemplateReadRepository _repository;

    public TemplateWorkoutReadRepositoryTests()
    {
        _repository = new TemplateWorkoutReadRepository(context, mapper);
    }

    [Fact]
    public async Task GetTemplateByNameAsync_WhenExists_ShouldReturnTemplateWithHierarchy()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var exerciseId = Guid.NewGuid();
        var templateName = "Leg Day";

        var exerciseEf = new ExerciseEf
        {
            Id = exerciseId,
            Name = "Squat",
            CreatedByUserId = null
        };
        context.Exercises.Add(exerciseEf);

        // 2. Create Template
        var templateEf = new TemplateWorkoutEf
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = templateName,
            Description = "Heavy legs",
            UsageCount = 5,
            LastUsedAt = DateTime.UtcNow.AddDays(-1)
        };
        context.WorkoutTemplates.Add(templateEf);

        // 3. Create TemplateWorkoutExercise
        var templateExerciseEf = new TemplateWorkoutExerciseEf
        {
            Id = Guid.NewGuid(),
            WorkoutTemplateId = templateEf.Id,
            ExerciseId = exerciseId,
            OrderIndex = 1,
            Notes = "Deep depth"
        };
        context.WorkoutTemplateExercises.Add(templateExerciseEf);

        // 4. Create Sets
        var set1 = new TemplateSetEf
        {
            Id = Guid.NewGuid(),
            WorkoutTemplateExerciseId = templateExerciseEf.Id,
            SetNumber = 1,
            PlannedWeightKg = 100,
            PlannedReps = 5,
            RestSeconds = 180,
            SetType = (int)SetType.Normal
        };
        var set2 = new TemplateSetEf
        {
            Id = Guid.NewGuid(),
            WorkoutTemplateExerciseId = templateExerciseEf.Id,
            SetNumber = 2,
            PlannedWeightKg = 60,
            PlannedReps = 12,
            RestSeconds = 90,
            SetType = (int)SetType.Warmup
        };
        context.TemplateSets.AddRange(set1, set2);

        await context.SaveChangesAsync();

        // Act
        var result = await _repository.FindTemplateByNameReadonlyAsync(templateName, userId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(templateEf.Id);
        result.Name.Should().Be(templateName);
        result.Description.Should().Be("Heavy legs");

        // Verify Exercises were included
        result.Exercises.Should().HaveCount(1);
        var exerciseResult = result.Exercises.First();
        exerciseResult.ExerciseId.Should().Be(exerciseId);
        exerciseResult.Notes.Should().Be("Deep depth");

        // Verify Sets were included (Grandchildren)
        exerciseResult.Sets.Should().HaveCount(2);

        var firstSet = exerciseResult.Sets.First(s => s.SetNumber == 1);
        firstSet.PlannedWeightKg.Should().Be(100);
        firstSet.SetType.Should().Be(SetType.Normal);

        var secondSet = exerciseResult.Sets.First(s => s.SetNumber == 2);
        secondSet.PlannedWeightKg.Should().Be(60);
        secondSet.SetType.Should().Be(SetType.Warmup);
    }

    [Fact]
    public async Task GetTemplateByNameAsync_WrongName_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var templateEf = new TemplateWorkoutEf
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = "Pull Day",
            UsageCount = 0
        };
        context.WorkoutTemplates.Add(templateEf);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.FindTemplateByNameReadonlyAsync("Push Day", userId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTemplateByNameAsync_WrongUser_ShouldReturnNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var templateName = "Full Body";

        var templateEf = new TemplateWorkoutEf
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = templateName,
            UsageCount = 0
        };
        context.WorkoutTemplates.Add(templateEf);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.FindTemplateByNameReadonlyAsync(templateName, otherUserId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTemplateByNameAsync_WhenEmptyDatabase_ShouldReturnNull()
    {
        // Arrange
        // Database is empty

        // Act
        var result = await _repository.FindTemplateByNameReadonlyAsync("Any", Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
