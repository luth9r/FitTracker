using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Xunit;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class SetProfileTests
    {
        private readonly IMapper _mapper;

        public SetProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<SetProfile>();

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
        public void Should_Map_Set_To_SetEf()
        {
            // Arrange
            var workoutExerciseId = Guid.NewGuid();
            var weight = Weight.FromKilograms(75);
            var setNumber = 3;
            var reps = 12;
            var restSeconds = 60;
            var setType = SetType.Normal;
            var isCompleted = true;
            var completedAt = DateTime.UtcNow;

            var set = new Set(
                workoutExerciseId,
                setNumber,
                weight,
                reps,
                restSeconds,
                setType,
                isCompleted,
                completedAt
            );

            // Act
            var result = _mapper.Map<SetEf>(set);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutExerciseId.Should().Be(workoutExerciseId);
            result.SetNumber.Should().Be(setNumber);
            result.WeightKg.Should().Be(weight.ToKilograms());
            result.Reps.Should().Be(reps);
            result.RestSeconds.Should().Be(restSeconds);
            result.SetType.Should().Be((int)setType);
            result.IsCompleted.Should().Be(isCompleted);
            result.CompletedAt.Should().Be(completedAt);
            result.WorkoutExercise.Should().BeNull();
        }

        [Fact]
        public void Should_Map_SetEf_To_Set()
        {
            // Arrange
            var workoutExerciseId = Guid.NewGuid();
            var weightKg = 80m;
            var setNumber = 2;
            var reps = 10;
            var restSeconds = 90;
            var setType = (int)SetType.WarmUp;
            var isCompleted = false;
            var completedAt = (DateTime?)null;

            var setEf = new SetEf
            {
                WorkoutExerciseId = workoutExerciseId,
                SetNumber = setNumber,
                WeightKg = weightKg,
                Reps = reps,
                RestSeconds = restSeconds,
                SetType = setType,
                IsCompleted = isCompleted,
                CompletedAt = completedAt
            };

            // Act
            var result = _mapper.Map<Set>(setEf);

            // Assert
            result.Should().NotBeNull();
            result.WorkoutExerciseId.Should().Be(workoutExerciseId);
            result.SetNumber.Should().Be(setNumber);
            result.Weight.ToKilograms().Should().Be(weightKg);
            result.Reps.Should().Be(reps);
            result.RestSeconds.Should().Be(restSeconds);
            result.SetType.Should().Be((SetType)setType);
            result.IsCompleted.Should().Be(isCompleted);
            result.CompletedAt.Should().Be(completedAt);
        }
    }
}
