using FitTracker.Domain.Entities.TemplateAggregate;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FitTrackerInfrastructure.Tests.Repositories;

public class TemplateWorkoutWriteRepositoryTests : RepositoryTestBase
{
    private readonly TemplateWorkoutWriteRepository _repository;

    public TemplateWorkoutWriteRepositoryTests()
    {
        _repository = new TemplateWorkoutWriteRepository(context, mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistTemplate_WithExercisesAndSets()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var exerciseId = Guid.NewGuid();

        var template = TemplateWorkout.Create(
            userId,
            "Push Day",
            "Chest and Triceps");

        var setsData = new List<TemplateSetData>
        {
            new(SetNumber: 1, Weight: 100, Reps: 5, Rest: 120, Type: SetType.Normal),
            new(SetNumber: 2, Weight: 90, Reps: 8, Rest: 90, Type: SetType.Dropset)
        };

        template.AddExercise(
            exerciseId,
            orderIndex: 1,
            notes: "Focus on form",
            setsData);

        // Act
        await _repository.AddAsync(template, CancellationToken.None);

        await context.SaveChangesAsync();

        // Assert
        var dbTemplate = await context.WorkoutTemplates
            .Include(t => t.Exercises)
            .ThenInclude(e => e.PlannedSets)
            .FirstOrDefaultAsync(t => t.Id == template.Id);

        dbTemplate.Should().NotBeNull();
        dbTemplate!.Name.Should().Be("Push Day");
        dbTemplate.UserId.Should().Be(userId);
        dbTemplate.Exercises.Should().HaveCount(1);

        var dbExercise = dbTemplate.Exercises.First();
        dbExercise.ExerciseId.Should().Be(exerciseId);
        dbExercise.OrderIndex.Should().Be(1);
        dbExercise.Notes.Should().Be("Focus on form");
        dbExercise.PlannedSets.Should().HaveCount(2);

        var set1 = dbExercise.PlannedSets.Single(s => s.SetNumber == 1);
        set1.PlannedWeightKg.Should().Be(100);
        set1.PlannedReps.Should().Be(5);
        set1.SetType.Should().Be((int)SetType.Normal);

        var set2 = dbExercise.PlannedSets.Single(s => s.SetNumber == 2);
        set2.PlannedWeightKg.Should().Be(90);
        set2.SetType.Should().Be((int)SetType.Dropset);
    }
}
