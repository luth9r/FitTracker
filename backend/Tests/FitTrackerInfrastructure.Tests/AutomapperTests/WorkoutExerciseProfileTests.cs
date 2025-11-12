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
    public class WorkoutExerciseProfileTests
    {
        private readonly IMapper _mapper;

        public WorkoutExerciseProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<WorkoutExerciseProfile>();

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
        public void Should_Map_WorkoutExercise_To_WorkoutExerciseEf()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var orderIndex = 1;
            var notes = "Sample notes";

            var workoutExercise = new WorkoutExercise(
                workoutId,
                exerciseId,
                orderIndex,
                notes
            );

            // Act
            var result = _mapper.Map<WorkoutExerciseEf>(workoutExercise);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutId.Should().Be(workoutId);
            result.ExerciseId.Should().Be(exerciseId);
            result.OrderIndex.Should().Be(orderIndex);
            result.Notes.Should().Be(notes);
            result.Workout.Should().BeNull();
            result.Exercise.Should().BeNull();
            result.Sets.Should().BeEmpty();
        }

        [Fact]
        public void Should_Map_WorkoutExerciseEf_To_WorkoutExercise()
        {
            // Arrange
            var workoutId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var orderIndex = 2;
            var notes = "Another notes";

            var workoutExerciseEf = new WorkoutExerciseEf
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseId,
                OrderIndex = orderIndex,
                Notes = notes
            };

            // Act
            var result = _mapper.Map<WorkoutExercise>(workoutExerciseEf);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutId.Should().Be(workoutId);
            result.ExerciseId.Should().Be(exerciseId);
            result.OrderIndex.Should().Be(orderIndex);
            result.Notes.Should().Be(notes);
        }
    }
}
