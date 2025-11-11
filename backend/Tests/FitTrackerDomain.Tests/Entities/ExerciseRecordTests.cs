using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTrackerDomain.Tests.Factories;
using FluentAssertions;
using FluentValidation;

namespace FitTracker.Domain.Tests.Entities
{
    public class ExerciseRecordTests
    {
        private readonly Guid _userId = Guid.NewGuid();
        private readonly Guid _exerciseId = Guid.NewGuid();

        #region Constructor Validation

        [Fact]
        public void Create_WithEmptyUserId_ShouldThrowValidationException()
        {
            Action act = () => ExerciseRecord.Create(Guid.Empty, _exerciseId);

            var ex = act.Should().Throw<ValidationException>().Which;
            ex.Message.Should().Contain("User ID is required");
        }

        [Fact]
        public void Create_WithEmptyExerciseId_ShouldThrowValidationException()
        {
            Action act = () => ExerciseRecord.Create(_userId, Guid.Empty);

            var ex = act.Should().Throw<ValidationException>().Which;
            ex.Message.Should().Contain("Exercise ID is required");
        }

        #endregion

        #region Default Creation

        [Fact]
        public void Create_Default_ShouldInitializeWithZeroes()
        {
            var record = ExerciseRecord.Create(_userId, _exerciseId);

            record.UserId.Should().Be(_userId);
            record.ExerciseId.Should().Be(_exerciseId);
            record.MaxWeight.ToKilograms().Should().Be(0);
            record.MaxReps.Should().Be(0);
            record.MaxVolume.Should().Be(0);
            record.MaxTotalVolume.Should().Be(0);
            record.TotalWorkouts.Should().Be(0);
            record.TotalSets.Should().Be(0);
            record.TotalReps.Should().Be(0);
            record.TotalLifted.Should().Be(0);
            record.MaxWeightDate.Should().BeOnOrBefore(DateTime.UtcNow);
            record.LastPerformed.Should().BeOnOrBefore(DateTime.UtcNow);
        }

        #endregion

        #region UpdateRecords Tests

        [Fact]
        public void UpdateRecords_WithNewMaxWeight_ShouldUpdateAndReturnTrue()
        {
            var record = ExerciseRecordFactory.Default();
            var newMaxWeight = Weight.FromKilograms(50);

            var result = record.UpdateRecords(
                maxSetWeight: newMaxWeight,
                maxSetReps: 1,
                maxSetVolume: 50m,
                workoutTotalVolume: 50m,
                workoutSets: 1,
                workoutReps: 1,
                workoutLifted: 50m);

            result.Should().BeTrue();
            record.MaxWeight.ToKilograms().Should().Be(newMaxWeight.ToKilograms());
            record.MaxReps.Should().Be(1);
            record.MaxVolume.Should().Be(50m);
            record.MaxTotalVolume.Should().Be(50m);
            record.TotalWorkouts.Should().Be(1);
            record.TotalSets.Should().Be(1);
            record.TotalReps.Should().Be(1);
            record.TotalLifted.Should().Be(50m);
            record.LastPerformed.Should().BeOnOrBefore(DateTime.UtcNow);
        }

        [Fact]
        public void UpdateRecords_WithLowerMaxWeight_ShouldNotChangeMaxWeight()
        {
            var record = ExerciseRecordFactory.WithValues(
                maxWeight: Weight.FromKilograms(100),
                maxReps: 10,
                maxVolume: 1000m,
                maxTotalVolume: 2000m,
                totalWorkouts: 5,
                totalSets: 20,
                totalReps: 200,
                totalLifted: 10000m);
            var newMaxWeight = Weight.FromKilograms(80);

            var result = record.UpdateRecords(
                maxSetWeight: newMaxWeight,
                maxSetReps: 5,
                maxSetVolume: 900m,
                workoutTotalVolume: 1500m,
                workoutSets: 3,
                workoutReps: 50,
                workoutLifted: 3000m);

            result.Should().BeFalse();
            record.MaxWeight.ToKilograms().Should().Be(100);
            record.MaxReps.Should().Be(10);
            record.MaxVolume.Should().Be(1000m);
            record.MaxTotalVolume.Should().Be(2000m);
            record.TotalWorkouts.Should().Be(6);
            record.TotalSets.Should().Be(23);
            record.TotalReps.Should().Be(250);
            record.TotalLifted.Should().Be(13000m);
            record.LastPerformed.Should().BeOnOrBefore(DateTime.UtcNow);
        }

        #endregion

        #region Average Calculations

        [Fact]
        public void GetAverageWeightPerSet_WithZeroTotalSets_ShouldReturnZero()
        {
            var record = ExerciseRecordFactory.Default();

            var avg = record.GetAverageWeightPerSet();

            avg.Should().Be(0);
        }

        [Fact]
        public void GetAverageWeightPerSet_WithData_ShouldCalculateCorrectly()
        {
            var record = ExerciseRecordFactory.WithValues(
                totalSets: 5,
                totalLifted: 150);

            var avg = record.GetAverageWeightPerSet();

            avg.Should().Be(30);
        }

        [Fact]
        public void GetAverageRepsPerSet_WithZeroTotalSets_ShouldReturnZero()
        {
            var record = ExerciseRecordFactory.Default();

            var avg = record.GetAverageRepsPerSet();

            avg.Should().Be(0);
        }

        [Fact]
        public void GetAverageRepsPerSet_WithData_ShouldCalculateCorrectly()
        {
            var record = ExerciseRecordFactory.WithValues(
                totalSets: 10,
                totalReps: 100);

            var avg = record.GetAverageRepsPerSet();

            avg.Should().Be(10);
        }

        #endregion
    }
}
