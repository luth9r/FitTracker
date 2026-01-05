using AutoMapper;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.Repositories;

public sealed class WorkoutReadRepositoryTests
{
    private static FitTrackerDbContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<FitTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FitTrackerDbContext(options);
    }

    private static IMapper BuildMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<WorkoutProfile>(), NullLoggerFactory.Instance);

        return config.CreateMapper();
    }

    [Fact]
    public async Task GetCompletedByUserIdAsync_ShouldReturnOnlyCompletedWorkouts()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        var workouts = new[]
        {
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Completed Workout 1",
                WorkoutDate = DateTime.UtcNow.AddDays(-2),
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "In Progress Workout",
                WorkoutDate = DateTime.UtcNow.AddDays(-1),
                Duration = TimeSpan.Zero,
                IsCompleted = false,
                IsInProgress = true,
                TotalVolumeKg = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Completed Workout 2",
                WorkoutDate = DateTime.UtcNow.AddDays(-3),
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 800,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCompletedByUserIdAsync(userId, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Select(w => w.Name).Should().BeEquivalentTo("Completed Workout 1", "Completed Workout 2");
    }

    [Fact]
    public async Task GetCompletedByUserIdAsync_ShouldReturnOrderedByWorkoutDate()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        var workouts = new[]
        {
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Newest",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Oldest",
                WorkoutDate = DateTime.UtcNow.AddDays(-5),
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 800,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Middle",
                WorkoutDate = DateTime.UtcNow.AddDays(-2),
                Duration = TimeSpan.FromMinutes(50),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 900,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCompletedByUserIdAsync(userId, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Oldest");
        result[1].Name.Should().Be("Middle");
        result[2].Name.Should().Be("Newest");
    }

    [Fact]
    public async Task GetCompletedByUserIdAsync_ShouldReturnOnlyUserWorkouts()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        var workouts = new[]
        {
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId1,
                Name = "User1 Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                Name = "User2 Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 800,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetCompletedByUserIdAsync(userId1, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Single().Name.Should().Be("User1 Workout");
    }

    [Fact]
    public async Task GetCompletedByUserIdAsync_WhenNoCompletedWorkouts_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        // Act
        var result = await repository.GetCompletedByUserIdAsync(userId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecentByUserIdAsync_ShouldReturnSpecifiedNumberOfWorkouts()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        var workouts = new[]
        {
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Workout 1",
                WorkoutDate = DateTime.UtcNow.AddDays(-1),
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Workout 2",
                WorkoutDate = DateTime.UtcNow.AddDays(-2),
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 800,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Workout 3",
                WorkoutDate = DateTime.UtcNow.AddDays(-3),
                Duration = TimeSpan.FromMinutes(50),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 900,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetRecentByUserIdAsync(userId, 2, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetRecentByUserIdAsync_ShouldReturnOrderedByWorkoutDateDescending()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        var workouts = new[]
        {
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Oldest",
                WorkoutDate = DateTime.UtcNow.AddDays(-5),
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Newest",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 800,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = "Middle",
                WorkoutDate = DateTime.UtcNow.AddDays(-2),
                Duration = TimeSpan.FromMinutes(50),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 900,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetRecentByUserIdAsync(userId, 3, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Newest");
        result[1].Name.Should().Be("Middle");
        result[2].Name.Should().Be("Oldest");
    }

    [Fact]
    public async Task GetRecentByUserIdAsync_ShouldReturnOnlyUserWorkouts()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        var workouts = new[]
        {
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId1,
                Name = "User1 Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(60),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 1000,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new WorkoutEf
            {
                Id = Guid.NewGuid(),
                UserId = userId2,
                Name = "User2 Workout",
                WorkoutDate = DateTime.UtcNow,
                Duration = TimeSpan.FromMinutes(45),
                IsCompleted = true,
                IsInProgress = false,
                TotalVolumeKg = 800,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };

        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetRecentByUserIdAsync(userId1, 10, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Single().Name.Should().Be("User1 Workout");
    }

    [Fact]
    public async Task GetRecentByUserIdAsync_WhenNoWorkouts_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        // Act
        var result = await repository.GetRecentByUserIdAsync(userId, 5, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetRecentByUserIdAsync_WhenTakeIsZero_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new WorkoutReadRepository(context, mapper);

        var userId = Guid.NewGuid();

        var workout = new WorkoutEf
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = "Workout",
            WorkoutDate = DateTime.UtcNow,
            Duration = TimeSpan.FromMinutes(60),
            IsCompleted = true,
            IsInProgress = false,
            TotalVolumeKg = 1000,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        context.Workouts.Add(workout);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetRecentByUserIdAsync(userId, 0, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
