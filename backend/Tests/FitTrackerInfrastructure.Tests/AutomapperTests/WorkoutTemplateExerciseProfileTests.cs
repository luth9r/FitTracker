using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class WorkoutTemplateExerciseProfileTests
    {
        private readonly IMapper _mapper;

        public WorkoutTemplateExerciseProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<WorkoutTemplateExerciseProfile>();

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
        public void Should_Map_WorkoutTemplateExercise_To_WorkoutTemplateExerciseEf()
        {
            // Arrange
            var workoutTemplateId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var orderIndex = 3;
            var notes = "Focus on form";

            var workoutTemplateExercise = new WorkoutTemplateExercise(
                workoutTemplateId,
                exerciseId,
                orderIndex,
                notes
            );

            // Act
            var result = _mapper.Map<WorkoutTemplateExerciseEf>(workoutTemplateExercise);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutTemplateId.Should().Be(workoutTemplateId);
            result.ExerciseId.Should().Be(exerciseId);
            result.OrderIndex.Should().Be(orderIndex);
            result.Notes.Should().Be(notes);
            result.WorkoutTemplate.Should().BeNull();
            result.Exercise.Should().BeNull();
            result.PlannedSets.Should().BeEmpty();
        }

        [Fact]
        public void Should_Map_WorkoutTemplateExerciseEf_To_WorkoutTemplateExercise()
        {
            // Arrange
            var workoutTemplateId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var orderIndex = 5;
            var notes = "Add extra weight";

            var workoutTemplateExerciseEf = new WorkoutTemplateExerciseEf
            {
                WorkoutTemplateId = workoutTemplateId,
                ExerciseId = exerciseId,
                OrderIndex = orderIndex,
                Notes = notes
            };

            // Act
            var result = _mapper.Map<WorkoutTemplateExercise>(workoutTemplateExerciseEf);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutTemplateId.Should().Be(workoutTemplateId);
            result.ExerciseId.Should().Be(exerciseId);
            result.OrderIndex.Should().Be(orderIndex);
            result.Notes.Should().Be(notes);
        }
    }
}
