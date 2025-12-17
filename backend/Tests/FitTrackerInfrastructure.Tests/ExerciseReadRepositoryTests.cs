using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FitTracker.Infrastructure.Tests.Repositories
{
    public sealed class ExerciseReadRepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock = new();
        private IExerciseReadRepository _repository = null!;

        private static FitTrackerDbContext BuildContext()
        {
            var options = new DbContextOptionsBuilder<FitTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FitTrackerDbContext(options);
        }

        [Fact]
        public async Task GetExerciseDetailsAsync_ShouldReturnAggregatesAndVolumeHistory()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();

            await using var context = BuildContext();

            _repository = new ExerciseReadRepository(context, _mapperMock.Object);

            var exerciseEf = new ExerciseEf
            {
                Id = exerciseId,
                Name = "Bench Press",
                Description = "Chest exercise",
                ImageUrl = "http://image",
                VideoUrl = "http://video",
                MuscleGroup = (int)MuscleGroup.Chest,
                Equipment = (int)Equipment.Barbell,
                CreatedByUserId = null,
            };
            context.Exercises.Add(exerciseEf);

            var recordEf = new ExerciseRecordEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ExerciseId = exerciseId,
                MaxWeightKg = 100,
                MaxReps = 8,
                MaxVolumeKg = 800,
                MaxTotalVolumeKg = 3000,
                MaxWeightDate = new DateTime(2025, 12, 1),
                MaxRepsDate = new DateTime(2025, 12, 2),
                MaxVolumeDate = new DateTime(2025, 12, 3),
                MaxTotalVolumeDate = new DateTime(2025, 12, 4),
                TotalWorkouts = 2,
                TotalSets = 4,
                TotalReps = 32,
                TotalLiftedKg = 3200,
                LastPerformed = new DateTime(2025, 12, 4),
            };
            context.ExerciseRecords.Add(recordEf);

            var workout1 = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                Name = "Workout 1",
                UserId = userId,
                WorkoutDate = new DateTime(2025, 12, 1),
            };
            var workout2 = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                Name = "Workout 2",
                UserId = userId,
                WorkoutDate = new DateTime(2025, 12, 2),
            };
            context.Workouts.AddRange(workout1, workout2);

            var we1 = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout1.Id,
                ExerciseId = exerciseId,
                OrderIndex = 1,
            };
            var we2 = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout2.Id,
                ExerciseId = exerciseId,
                OrderIndex = 1,
            };
            context.WorkoutExercises.AddRange(we1, we2);

            var set1 = new SetEf
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = we1.Id,
                WeightKg = 80,
                Reps = 8,
                IsCompleted = true,
            };
            var set2 = new SetEf
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = we2.Id,
                WeightKg = 90,
                Reps = 8,
                IsCompleted = true,
            };
            context.Sets.AddRange(set1, set2);

            await context.SaveChangesAsync();

            // Act
            var result = await _repository.GetExerciseDetailsAsync(exerciseId, userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(exerciseId);
            result.Name.Should().Be("Bench Press");
            result.MuscleGroup.Should().Be(MuscleGroup.Chest);
            result.Equipment.Should().Be(Equipment.Barbell);
            result.IsCustom.Should().BeFalse();

            result.TotalWorkouts.Should().Be(2);
            result.TotalSets.Should().Be(4);
            result.TotalReps.Should().Be(32);
            result.TotalLifted.Should().Be(3200);
            result.MaxWeightKg.Should().Be(100);
            result.MaxReps.Should().Be(8);
            result.MaxVolume.Should().Be(800);
            result.MaxTotalVolume.Should().Be(3000);
            result.LastPerformed.Should().Be(new DateTime(2025, 12, 4));

            result.VolumeHistory.Should().HaveCount(2);

            result.VolumeHistory.Select(h => h.Date)
                .Should().BeEquivalentTo(
                    new[]
                    {
                        new DateOnly(2025, 12, 1),
                        new DateOnly(2025, 12, 2),
                    });

            var firstPoint = result.VolumeHistory.Single(h => h.Date == new DateOnly(2025, 12, 1));
            firstPoint.Value.Should().Be(80 * 8);

            var secondPoint = result.VolumeHistory.Single(h => h.Date == new DateOnly(2025, 12, 2));
            secondPoint.Value.Should().Be(90 * 8);
        }
    }
}
