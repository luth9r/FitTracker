using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Xunit;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class WorkoutProfileTests
    {
        private readonly IMapper _mapper;

        public WorkoutProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<WorkoutProfile>();

            var config = new MapperConfiguration(
                configExpression,
                NullLoggerFactory.Instance
            );
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Configuration_Should_BeValid()
        {
            Action act = () => _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Map_Workout_To_WorkoutEf()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "Chest Day";
            var workoutDate = DateTime.UtcNow;
            var workoutTemplateId = Guid.NewGuid();
            var notes = "Warm-up first";
            var duration = TimeSpan.FromSeconds(60);
            var isCompleted = false;
            var isInProgress = true;
            var startedAt = DateTime.UtcNow.AddMinutes(-10);
            var completedAt = (DateTime?)null;
            var totalVolumeKg = 123.45m;

            var workout = new Workout(
                id,
                userId,
                name,
                workoutDate,
                workoutTemplateId,
                notes,
                duration,
                isCompleted,
                isInProgress,
                startedAt,
                completedAt,
                totalVolumeKg
            );

            // Act
            var result = _mapper.Map<WorkoutEf>(workout);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.UserId.Should().Be(userId);
            result.Name.Should().Be(name);
            result.WorkoutDate.Should().Be(workoutDate);
            result.WorkoutTemplateId.Should().Be(workoutTemplateId);
            result.Notes.Should().Be(notes);
            result.Duration.Should().Be(duration);
            result.IsCompleted.Should().Be(isCompleted);
            result.IsInProgress.Should().Be(isInProgress);
            result.StartedAt.Should().Be(startedAt);
            result.CompletedAt.Should().Be(completedAt);
            result.TotalVolumeKg.Should().Be(totalVolumeKg);
            result.User.Should().BeNull();
            result.WorkoutTemplate.Should().BeNull();
            result.Exercises.Should().BeEmpty();
        }

        [Fact]
        public void Should_Map_WorkoutEf_To_Workout()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "Leg Day";
            var workoutDate = DateTime.UtcNow;
            var workoutTemplateId = Guid.NewGuid();
            var notes = "Light warm-up";
            var duration = TimeSpan.FromSeconds(45);
            var isCompleted = true;
            var isInProgress = false;
            var startedAt = DateTime.UtcNow.AddMinutes(-50);
            var completedAt = DateTime.UtcNow;

            var workoutEf = new WorkoutEf
            {
                UserId = userId,
                Name = name,
                WorkoutDate = workoutDate,
                WorkoutTemplateId = workoutTemplateId,
                Notes = notes,
                Duration = duration,
                IsCompleted = isCompleted,
                IsInProgress = isInProgress,
                StartedAt = startedAt,
                CompletedAt = completedAt,
                TotalVolumeKg = 222.33m
            };

            typeof(BaseEntityEf).GetProperty("Id")!.SetValue(workoutEf, id);

            // Act
            var result = _mapper.Map<Workout>(workoutEf);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Name.Should().Be(name);
            result.WorkoutDate.Should().Be(workoutDate);
            result.WorkoutTemplateId.Should().Be(workoutTemplateId);
            result.Notes.Should().Be(notes);
            result.Duration.Should().Be(duration);
            result.IsCompleted.Should().Be(isCompleted);
            result.IsInProgress.Should().Be(isInProgress);
            result.StartedAt.Should().Be(startedAt);
            result.CompletedAt.Should().Be(completedAt);
            result.TotalVolumeKg.Should().Be(222.33m);
        }
    }
}
