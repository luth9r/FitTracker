using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FitTrackerInfrastructure.Tests.Repositories
{
    public sealed class SetReadRepositoryTests
    {
        private readonly Mock<IMapper> _mapperMock = new();

        private static FitTrackerDbContext BuildContext()
        {
            var options = new DbContextOptionsBuilder<FitTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FitTrackerDbContext(options);
        }

        private static IMapper BuildMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<SetEf, Set>(), NullLoggerFactory.Instance);

            return config.CreateMapper();
        }

        [Fact]
        public async Task GetTotalWeightLiftedAsync_ShouldCalculateCorrectTotal()
        {
            // Arrange
            await using var context = BuildContext();
            var repository = new SetReadRepository(context, _mapperMock.Object);

            var userId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();

            var workout = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Test Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 0,
            };
            context.Workouts.Add(workout);

            var workoutExercise = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout.Id,
                ExerciseId = exerciseId,
                OrderIndex = 1,
            };
            context.WorkoutExercises.Add(workoutExercise);

            var sets = new[]
            {
                new SetEf
                {
                    Id = Guid.NewGuid(),
                    WorkoutExerciseId = workoutExercise.Id,
                    SetNumber = 1,
                    WeightKg = 100,
                    Reps = 10,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow,
                },
                new SetEf
                {
                    Id = Guid.NewGuid(),
                    WorkoutExerciseId = workoutExercise.Id,
                    SetNumber = 2,
                    WeightKg = 80,
                    Reps = 12,
                    SetType = 0,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow,
                },
            };
            context.Sets.AddRange(sets);

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTotalWeightLiftedAsync(userId, CancellationToken.None);

            // Assert
            result.Should().Be(1960); // (100 * 10) + (80 * 12)
        }

        [Fact]
        public async Task GetTotalWeightLiftedAsync_ShouldReturnZeroForUserWithoutSets()
        {
            // Arrange
            await using var context = BuildContext();
            var repository = new SetReadRepository(context, _mapperMock.Object);

            var userId = Guid.NewGuid();

            // Act
            var result = await repository.GetTotalWeightLiftedAsync(userId, CancellationToken.None);

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public async Task GetTotalWeightLiftedAsync_ShouldOnlyCountSpecificUserSets()
        {
            // Arrange
            await using var context = BuildContext();
            var repository = new SetReadRepository(context, _mapperMock.Object);

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();

            // User 1 workout
            var workout1 = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId1,
                Name = "User 1 Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 500,
            };
            context.Workouts.Add(workout1);

            var workoutExercise1 = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout1.Id,
                ExerciseId = exerciseId,
                OrderIndex = 1,
            };
            context.WorkoutExercises.Add(workoutExercise1);

            context.Sets.Add(new SetEf
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = workoutExercise1.Id,
                SetNumber = 1,
                WeightKg = 100,
                Reps = 5,
                SetType = 0,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow,
            });

            // User 2 workout
            var workout2 = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                Name = "User 2 Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 2000,
            };
            context.Workouts.Add(workout2);

            var workoutExercise2 = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout2.Id,
                ExerciseId = exerciseId,
                OrderIndex = 1,
            };
            context.WorkoutExercises.Add(workoutExercise2);

            context.Sets.Add(new SetEf
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = workoutExercise2.Id,
                SetNumber = 1,
                WeightKg = 200,
                Reps = 10,
                SetType = 0,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow,
            });

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTotalWeightLiftedAsync(userId1, CancellationToken.None);

            // Assert
            result.Should().Be(500); // Only user1's sets: 100 * 5
        }

        [Fact]
        public async Task GetTotalWeightLiftedAsync_ShouldCountMultipleWorkouts()
        {
            // Arrange
            await using var context = BuildContext();
            var repository = new SetReadRepository(context, _mapperMock.Object);

            var userId = Guid.NewGuid();
            var exerciseId1 = Guid.NewGuid();
            var exerciseId2 = Guid.NewGuid();

            // First workout
            var workout1 = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Workout 1",
                WorkoutDate = DateTime.UtcNow.AddDays(-1),
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 500,
            };
            context.Workouts.Add(workout1);

            var workoutExercise1 = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout1.Id,
                ExerciseId = exerciseId1,
                OrderIndex = 1,
            };
            context.WorkoutExercises.Add(workoutExercise1);

            context.Sets.Add(new SetEf
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = workoutExercise1.Id,
                SetNumber = 1,
                WeightKg = 50,
                Reps = 10,
                SetType = 0,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow.AddDays(-1),
            });

            // Second workout
            var workout2 = new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Workout 2",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 750,
            };
            context.Workouts.Add(workout2);

            var workoutExercise2 = new WorkoutExerciseEf
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout2.Id,
                ExerciseId = exerciseId2,
                OrderIndex = 1,
            };
            context.WorkoutExercises.Add(workoutExercise2);

            context.Sets.Add(new SetEf
            {
                Id = Guid.NewGuid(),
                WorkoutExerciseId = workoutExercise2.Id,
                SetNumber = 1,
                WeightKg = 75,
                Reps = 10,
                SetType = 0,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow,
            });

            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetTotalWeightLiftedAsync(userId, CancellationToken.None);

            // Assert
            result.Should().Be(1250); // (50 * 10) + (75 * 10)
        }
    }
}
