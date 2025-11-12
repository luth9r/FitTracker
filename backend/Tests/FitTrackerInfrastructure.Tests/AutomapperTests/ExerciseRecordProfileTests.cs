using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class ExerciseRecordProfileTests
    {
        private readonly IMapper _mapper;

        public ExerciseRecordProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ExerciseRecordProfile>();

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
        public void Should_Map_ExerciseRecord_To_ExerciseRecordEf()
        {
            var userId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var weight = Weight.FromKilograms(100);
            var maxWeightDate = DateTime.UtcNow.AddDays(-10);
            var maxRepsDate = DateTime.UtcNow.AddDays(-8);
            var maxVolumeDate = DateTime.UtcNow.AddDays(-5);
            var maxTotalVolumeDate = DateTime.UtcNow.AddDays(-3);
            var lastPerformed = DateTime.UtcNow.AddDays(-1);

            var exerciseRecord = new ExerciseRecord(
                userId,
                exerciseId,
                weight,
                10,
                1000,
                5000,
                maxWeightDate,
                maxRepsDate,
                maxVolumeDate,
                maxTotalVolumeDate,
                50,
                200,
                2000,
                100000,
                lastPerformed
            );

            var result = _mapper.Map<ExerciseRecordEf>(exerciseRecord);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.ExerciseId.Should().Be(exerciseId);
            result.MaxWeightKilograms.Should().Be(weight.ToKilograms());
            result.MaxReps.Should().Be(10);
            result.MaxVolume.Should().Be(1000);
            result.MaxTotalVolume.Should().Be(5000);
            result.MaxWeightDate.Should().Be(maxWeightDate);
            result.MaxRepsDate.Should().Be(maxRepsDate);
            result.MaxVolumeDate.Should().Be(maxVolumeDate);
            result.MaxTotalVolumeDate.Should().Be(maxTotalVolumeDate);
            result.TotalWorkouts.Should().Be(50);
            result.TotalSets.Should().Be(200);
            result.TotalReps.Should().Be(2000);
            result.TotalLifted.Should().Be(100000);
            result.LastPerformed.Should().Be(lastPerformed);
            result.User.Should().BeNull();
            result.Exercise.Should().BeNull();
        }

        [Fact]
        public void Should_Map_ExerciseRecordEf_To_ExerciseRecord()
        {
            var userId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var maxWeightDate = DateTime.UtcNow.AddDays(-10);
            var maxRepsDate = DateTime.UtcNow.AddDays(-8);
            var maxVolumeDate = DateTime.UtcNow.AddDays(-5);
            var maxTotalVolumeDate = DateTime.UtcNow.AddDays(-3);
            var lastPerformed = DateTime.UtcNow.AddDays(-1);

            var exerciseRecordEf = new ExerciseRecordEf
            {
                UserId = userId,
                ExerciseId = exerciseId,
                MaxWeightKilograms = 150.5m,
                MaxReps = 15,
                MaxVolume = 1500,
                MaxTotalVolume = 7500,
                MaxWeightDate = maxWeightDate,
                MaxRepsDate = maxRepsDate,
                MaxVolumeDate = maxVolumeDate,
                MaxTotalVolumeDate = maxTotalVolumeDate,
                TotalWorkouts = 75,
                TotalSets = 300,
                TotalReps = 3000,
                TotalLifted = 150000,
                LastPerformed = lastPerformed
            };

            var result = _mapper.Map<ExerciseRecord>(exerciseRecordEf);

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.ExerciseId.Should().Be(exerciseId);
            result.MaxWeight.ToKilograms().Should().Be(150.5m);
            result.MaxReps.Should().Be(15);
            result.MaxVolume.Should().Be(1500);
            result.MaxTotalVolume.Should().Be(7500);
            result.MaxWeightDate.Should().Be(maxWeightDate);
            result.MaxRepsDate.Should().Be(maxRepsDate);
            result.MaxVolumeDate.Should().Be(maxVolumeDate);
            result.MaxTotalVolumeDate.Should().Be(maxTotalVolumeDate);
            result.TotalWorkouts.Should().Be(75);
            result.TotalSets.Should().Be(300);
            result.TotalReps.Should().Be(3000);
            result.TotalLifted.Should().Be(150000);
            result.LastPerformed.Should().Be(lastPerformed);
        }
    }
}
