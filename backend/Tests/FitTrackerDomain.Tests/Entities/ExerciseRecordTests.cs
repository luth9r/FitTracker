// Domain.Tests/Entities/ExerciseRecordTests.cs
using System;
using System.Threading;
using Xunit;
using FluentValidation;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Tests.Factories;
using FitTrackerDomain.Tests.Factories;

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
            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() =>
                ExerciseRecord.Create(Guid.Empty, _exerciseId));

            Assert.Contains("User ID is required", ex.Message);
        }

        [Fact]
        public void Create_WithEmptyExerciseId_ShouldThrowValidationException()
        {
            // Act & Assert
            var ex = Assert.Throws<ValidationException>(() =>
                ExerciseRecord.Create(_userId, Guid.Empty));

            Assert.Contains("Exercise ID is required", ex.Message);
        }

        #endregion

        #region Default Creation

        [Fact]
        public void Create_Default_ShouldInitializeWithZeroes()
        {
            // Act
            var record = ExerciseRecord.Create(_userId, _exerciseId);

            // Assert
            Assert.Equal(_userId, record.UserId);
            Assert.Equal(_exerciseId, record.ExerciseId);
            Assert.Equal(0, record.MaxWeight.ToKilograms());
            Assert.Equal(0, record.MaxReps);
            Assert.Equal(0, record.MaxVolume);
            Assert.Equal(0, record.MaxTotalVolume);
            Assert.Equal(0, record.TotalWorkouts);
            Assert.Equal(0, record.TotalSets);
            Assert.Equal(0, record.TotalReps);
            Assert.Equal(0, record.TotalLifted);
            Assert.True(record.MaxWeightDate <= DateTime.UtcNow);
            Assert.True(record.LastPerformed <= DateTime.UtcNow);
        }

        #endregion

        #region UpdateRecords Tests

        [Fact]
        public void UpdateRecords_WithNewMaxWeight_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var record = ExerciseRecordMother.Default();
            var newMaxWeight = Weight.FromKilograms(50);

            // Act
            var result = record.UpdateRecords(
                maxSetWeight: newMaxWeight,
                maxSetReps: 1,
                maxSetVolume: 50m,
                workoutTotalVolume: 50m,
                workoutSets: 1,
                workoutReps: 1,
                workoutLifted: 50m);

            // Assert
            Assert.True(result);
            Assert.Equal(newMaxWeight.ToKilograms(), record.MaxWeight.ToKilograms());
            Assert.Equal(1, record.MaxReps);
            Assert.Equal(50m, record.MaxVolume);
            Assert.Equal(50m, record.MaxTotalVolume);
            Assert.Equal(1, record.TotalWorkouts);
            Assert.Equal(1, record.TotalSets);
            Assert.Equal(1, record.TotalReps);
            Assert.Equal(50m, record.TotalLifted);
            Assert.True(record.LastPerformed <= DateTime.UtcNow);
        }

        [Fact]
        public void UpdateRecords_WithLowerMaxWeight_ShouldNotChangeMaxWeight()
        {
            // Arrange
            var record = ExerciseRecordMother.WithValues(
                maxWeight: Weight.FromKilograms(100),
                maxReps: 10,
                maxVolume: 1000m,
                maxTotalVolume: 2000m,
                totalWorkouts: 5,
                totalSets: 20,
                totalReps: 200,
                totalLifted: 10000m);
            var newMaxWeight = Weight.FromKilograms(80);

            // Act
            var result = record.UpdateRecords(
                maxSetWeight: newMaxWeight,
                maxSetReps: 5,
                maxSetVolume: 900m,
                workoutTotalVolume: 1500m,
                workoutSets: 3,
                workoutReps: 50,
                workoutLifted: 3000m);

            // Assert
            Assert.False(result);
            Assert.Equal(100, record.MaxWeight.ToKilograms());
            Assert.Equal(10, record.MaxReps);
            Assert.Equal(1000m, record.MaxVolume);
            Assert.Equal(2000m, record.MaxTotalVolume);
            Assert.Equal(6, record.TotalWorkouts);
            Assert.Equal(23, record.TotalSets);
            Assert.Equal(250, record.TotalReps);
            Assert.Equal(13000m, record.TotalLifted);
            Assert.True(record.LastPerformed <= DateTime.UtcNow);
        }

        #endregion

        #region Average Calculations

        [Fact]
        public void GetAverageWeightPerSet_WithZeroTotalSets_ShouldReturnZero()
        {
            // Arrange
            var record = ExerciseRecordMother.Default();

            // Act
            var avg = record.GetAverageWeightPerSet();

            // Assert
            Assert.Equal(0, avg);
        }

        [Fact]
        public void GetAverageWeightPerSet_WithData_ShouldCalculateCorrectly()
        {
            // Arrange
            var record = ExerciseRecordMother.WithValues(
                totalSets: 5,
                totalLifted: 150);

            // Act
            var avg = record.GetAverageWeightPerSet();

            // Assert
            Assert.Equal(30, avg);
        }

        [Fact]
        public void GetAverageRepsPerSet_WithZeroTotalSets_ShouldReturnZero()
        {
            // Arrange
            var record = ExerciseRecordMother.Default();

            // Act
            var avg = record.GetAverageRepsPerSet();

            // Assert
            Assert.Equal(0, avg);
        }

        [Fact]
        public void GetAverageRepsPerSet_WithData_ShouldCalculateCorrectly()
        {
            // Arrange
            var record = ExerciseRecordMother.WithValues(
                totalSets: 10,
                totalReps: 100);

            // Act
            var avg = record.GetAverageRepsPerSet();

            // Assert
            Assert.Equal(10, avg);
        }

        #endregion
    }
}
