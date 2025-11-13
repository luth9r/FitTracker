using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class TemplateSetProfileTests
    {
        private readonly IMapper _mapper;

        public TemplateSetProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<TemplateSetProfile>();

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
        public void Should_Map_TemplateSet_To_TemplateSetEf()
        {
            // Arrange
            var workoutTemplateExerciseId = Guid.NewGuid();
            var plannedWeight = Weight.FromKilograms(85);
            var setNumber = 4;
            var plannedReps = 8;
            var restSeconds = 90;
            var setType = SetType.Normal;

            var templateSet = new TemplateSet(
                workoutTemplateExerciseId,
                setNumber,
                plannedWeight,
                plannedReps,
                restSeconds,
                setType
            );

            // Act
            var result = _mapper.Map<TemplateSetEf>(templateSet);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutTemplateExerciseId.Should().Be(workoutTemplateExerciseId);
            result.SetNumber.Should().Be(setNumber);
            result.PlannedWeight.Should().Be(plannedWeight.ToKilograms());
            result.PlannedReps.Should().Be(plannedReps);
            result.RestSeconds.Should().Be(restSeconds);
            result.SetType.Should().Be((int)setType);
            result.WorkoutTemplateExercise.Should().BeNull();
        }

        [Fact]
        public void Should_Map_TemplateSetEf_To_TemplateSet()
        {
            // Arrange
            var workoutTemplateExerciseId = Guid.NewGuid();
            var plannedWeightKg = 90m;
            var setNumber = 5;
            var plannedReps = 10;
            var restSeconds = 120;
            var setType = (int)SetType.WarmUp;

            var templateSetEf = new TemplateSetEf
            {
                WorkoutTemplateExerciseId = workoutTemplateExerciseId,
                SetNumber = setNumber,
                PlannedWeight = plannedWeightKg,
                PlannedReps = plannedReps,
                RestSeconds = restSeconds,
                SetType = setType
            };

            // Act
            var result = _mapper.Map<TemplateSet>(templateSetEf);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutTemplateExerciseId.Should().Be(workoutTemplateExerciseId);
            result.SetNumber.Should().Be(setNumber);
            result.PlannedWeight.ToKilograms().Should().Be(plannedWeightKg);
            result.PlannedReps.Should().Be(plannedReps);
            result.RestSeconds.Should().Be(restSeconds);
            result.SetType.Should().Be((SetType)setType);
        }
    }
}
