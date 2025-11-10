// Domain.Tests/Entities/SetTests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using FluentValidation;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Tests.Factories;
using FitTrackerDomain.Tests.Factories;

namespace FitTracker.Domain.Tests.Entities
{
    public class SetTests
    {
        private readonly Guid _workoutExerciseId = Guid.NewGuid();

        #region Constructor Validation

        [Fact]
        public void Constructor_WithEmptyWorkoutExerciseId_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                Set.CreateBuilder()
                    .WithWorkoutExercise(Guid.Empty)
                    .WithSetNumber(1)
                    .WithWeightKg(50)
                    .WithReps(10)
                    .Build());

            Assert.Contains("workout exercise id", ex.Message.ToLower());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidSetNumber_ShouldThrowArgumentException(int invalidSetNumber)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                Set.CreateBuilder()
                    .WithWorkoutExercise(_workoutExerciseId)
                    .WithSetNumber(invalidSetNumber)
                    .WithWeightKg(50)
                    .WithReps(10)
                    .Build());

            Assert.Contains("set number", ex.Message.ToLower());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidReps_ShouldThrowArgumentException(int invalidReps)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                Set.CreateBuilder()
                    .WithWorkoutExercise(_workoutExerciseId)
                    .WithSetNumber(1)
                    .WithWeightKg(50)
                    .WithReps(invalidReps)
                    .Build());

            Assert.Contains("reps", ex.Message.ToLower());
        }

        [Fact]
        public void Constructor_WithRepsExceedingMax_ShouldThrowArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                Set.CreateBuilder()
                    .WithWorkoutExercise(_workoutExerciseId)
                    .WithSetNumber(1)
                    .WithWeightKg(50)
                    .WithReps(Set.MaxReps + 1)
                    .Build());

            Assert.Contains("reps", ex.Message.ToLower());
        }

        //[Fact]
        //public void Constructor_WithNullWeight_ShouldThrowArgumentNullException()
        //{
        //    Assert.Throws<ArgumentNullException>(() =>
        //        new Set(_workoutExerciseId, 1, null!, 10));
        //}

        #endregion

        #region Update Methods

        [Fact]
        public void UpdateSetNumber_WithValidNumber_ShouldChangeSetNumber()
        {
            var set = SetMother.Default();

            set.UpdateSetNumber(2);

            Assert.Equal(2, set.SetNumber);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void UpdateSetNumber_WithInvalidNumber_ShouldThrowArgumentException(int invalidNumber)
        {
            var set = SetMother.Default();

            var ex = Assert.Throws<ArgumentException>(() => set.UpdateSetNumber(invalidNumber));

            Assert.Contains("set number", ex.Message.ToLower());
        }

        [Fact]
        public void UpdateWeight_WithValidWeight_ShouldUpdateWeight()
        {
            var set = SetMother.Default();
            var newWeight = Weight.FromKilograms(60);

            set.UpdateWeight(newWeight);

            Assert.Equal(60m, set.Weight.ToKilograms());
        }

        [Fact]
        public void UpdateWeight_WithNull_ShouldThrowArgumentNullException()
        {
            var set = SetMother.Default();
            Assert.Throws<ArgumentNullException>(() => set.UpdateWeight(null!));
        }

        [Fact]
        public void IncreaseWeightKg_WithPositiveAmount_ShouldIncreaseWeight()
        {
            var set = SetMother.Default();
            var initialWeight = set.Weight.ToKilograms();

            set.IncreaseWeightKg(10);

            Assert.Equal(initialWeight + 10, set.Weight.ToKilograms());
        }

        [Fact]
        public void IncreaseWeightKg_WithNegativeAmount_ShouldThrowArgumentException()
        {
            var set = SetMother.Default();
            var ex = Assert.Throws<ArgumentException>(() => set.IncreaseWeightKg(-5));
            Assert.Contains("amount", ex.Message.ToLower());
        }

        [Fact]
        public void DecreaseWeightKg_WithPositiveAmount_ShouldDecreaseWeight()
        {
            var set = SetMother.Default();
            set.UpdateWeight(Weight.FromKilograms(100));

            set.DecreaseWeightKg(10);

            Assert.Equal(90m, set.Weight.ToKilograms());
        }

        [Fact]
        public void DecreaseWeightKg_WithNegativeAmount_ShouldThrowArgumentException()
        {
            var set = SetMother.Default();
            var ex = Assert.Throws<ArgumentException>(() => set.DecreaseWeightKg(-5));
            Assert.Contains("amount", ex.Message.ToLower());
        }

        [Fact]
        public void DecreaseWeightKg_BelowZero_ShouldThrowInvalidOperationException()
        {
            var set = SetMother.Default();

            var ex = Assert.Throws<InvalidOperationException>(() => set.DecreaseWeightKg(set.Weight.ToKilograms() + 1));

            Assert.Contains("weight cannot be negative", ex.Message.ToLower());
        }

        [Fact]
        public void UpdateReps_WithValidValue_ShouldChangeReps()
        {
            var set = SetMother.Default();

            set.UpdateReps(12);

            Assert.Equal(12, set.Reps);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void UpdateReps_WithNonPositiveValue_ShouldThrowArgumentException(int invalidReps)
        {
            var set = SetMother.Default();

            var ex = Assert.Throws<ArgumentException>(() => set.UpdateReps(invalidReps));

            Assert.Contains("reps", ex.Message.ToLower());
        }

        [Fact]
        public void UpdateReps_ExceedingMax_ShouldThrowArgumentException()
        {
            var set = SetMother.Default();

            var ex = Assert.Throws<ArgumentException>(() => set.UpdateReps(Set.MaxReps + 1));

            Assert.Contains("reps", ex.Message.ToLower());
        }

        [Fact]
        public void UpdateRest_WithValidValue_ShouldUpdateRestSeconds()
        {
            var set = SetMother.Default();

            set.UpdateRest(60);

            Assert.Equal(60, set.RestSeconds);
        }

        [Fact]
        public void UpdateRest_WithNull_ShouldSetRestSecondsNull()
        {
            var set = SetMother.Default();
            set.UpdateRest(60);

            set.UpdateRest(null);

            Assert.Null(set.RestSeconds);
        }

        [Fact]
        public void UpdateRest_WithNegativeValue_ShouldThrowArgumentException()
        {
            var set = SetMother.Default();
            var ex = Assert.Throws<ArgumentException>(() => set.UpdateRest(-5));

            Assert.Contains("rest seconds cannot be negative", ex.Message.ToLower());
        }

        [Fact]
        public void UpdateRest_ExceedingMax_ShouldThrowArgumentException()
        {
            var set = SetMother.Default();
            var ex = Assert.Throws<ArgumentException>(() => set.UpdateRest(Set.MaxRestSeconds + 1));

            Assert.Contains("rest cannot exceed", ex.Message.ToLower());
        }

        [Fact]
        public void ChangeSetType_ShouldUpdateSetType()
        {
            var set = SetMother.Default();

            set.ChangeSetType(SetType.Dropset);

            Assert.Equal(SetType.Dropset, set.SetType);
        }

        [Fact]
        public void Complete_ShouldMarkCompleted()
        {
            var set = SetMother.Default();

            set.Complete();

            Assert.True(set.IsCompleted);
            Assert.NotNull(set.CompletedAt);
        }

        [Fact]
        public void Uncomplete_ShouldMarkNotCompleted()
        {
            var set = SetMother.CompletedSet();

            set.Uncomplete();

            Assert.False(set.IsCompleted);
            Assert.Null(set.CompletedAt);
        }

        #endregion

        #region Calculations

        [Fact]
        public void CalculateVolume_ShouldReturnCorrectWeightTimesReps()
        {
            var set = SetMother.Default();

            var expected = set.Weight.ToKilograms() * set.Reps;

            Assert.Equal(expected, set.CalculateVolume());
        }

        [Fact]
        public void CalculateVolumeLbs_ShouldReturnCorrectWeightTimesReps()
        {
            var set = SetMother.Default();

            var expected = set.Weight.ToPounds() * set.Reps;

            Assert.Equal(expected, set.CalculateVolumeLbs());
        }

        #endregion

        #region IsPR Tests

        [Fact]
        public void IsPR_WithNoPreviousSets_ShouldReturnTrue()
        {
            var set = SetMother.Default();

            var result = set.IsPR(new List<Set>());

            Assert.True(result);
        }

        [Fact]
        public void IsPR_WithHigherWeight_ShouldReturnTrue()
        {
            var set = SetMother.Default();
            var previousSets = new List<Set>
            {
                SetMother.Default()
            };
            previousSets[0].UpdateWeight(Weight.FromKilograms(40));

            var result = set.IsPR(previousSets);

            Assert.True(result);
        }

        [Fact]
        public void IsPR_WithLowerWeight_ShouldReturnFalse()
        {
            var set = SetMother.Default();
            var previousSets = new List<Set>
            {
                SetMother.Default()
            };
            previousSets[0].UpdateWeight(Weight.FromKilograms(60));

            var result = set.IsPR(previousSets);

            Assert.False(result);
        }

        #endregion
    }
}
