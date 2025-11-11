using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTrackerDomain.Tests.Factories;
using FluentAssertions;
using FluentValidation;

namespace FitTracker.Domain.Tests.Entities
{
    public class SetTests
    {
        private readonly Guid _workoutExerciseId = Guid.NewGuid();


        [Fact]
        public void Build_Should_Create_Set_With_Provided_Properties()
        {
            // Arrange
            var workoutExerciseId = Guid.NewGuid();
            var setNumber = 1;
            var weight = Weight.FromKilograms(50);
            var reps = 10;
            var restSeconds = 60;
            var setType = SetType.Dropset;

            // Act
            var set = Set.CreateBuilder()
                         .WithWorkoutExercise(workoutExerciseId)
                         .WithSetNumber(setNumber)
                         .WithWeight(weight)
                         .WithReps(reps)
                         .WithRest(restSeconds)
                         .WithSetType(setType)
                         .Build();

            // Assert
            set.Should().NotBeNull();
            set.WorkoutExerciseId.Should().Be(workoutExerciseId);
            set.SetNumber.Should().Be(setNumber);
            set.Weight.Should().Be(weight);
            set.Reps.Should().Be(reps);
            set.RestSeconds.Should().Be(restSeconds);
            set.SetType.Should().Be(setType);
            set.IsCompleted.Should().BeFalse();
            set.CompletedAt.Should().BeNull();
        }

        [Fact]
        public void Build_Should_Default_RestSeconds_To_Null_If_Not_Provided()
        {
            var set = Set.CreateBuilder()
                         .WithWorkoutExercise(Guid.NewGuid())
                         .WithSetNumber(1)
                         .WithWeightKg(20)
                         .WithReps(8)
                         .Build();

            set.RestSeconds.Should().BeNull();
        }

        [Fact]
        public void WithWeightKg_Should_Set_Weight_Correctly()
        {
            var weightKg = 75m;

            var set = Set.CreateBuilder()
                         .WithWorkoutExercise(Guid.NewGuid())
                         .WithSetNumber(2)
                         .WithWeightKg(weightKg)
                         .WithReps(5)
                         .Build();

            set.Weight.ToKilograms().Should().Be(weightKg);
        }

        [Fact]
        public void WithWeightLbs_Should_Set_Weight_Correctly()
        {
            var weightLbs = 220m;
            var expectedKg = Weight.FromPounds(weightLbs).ToKilograms();

            var set = Set.CreateBuilder()
                         .WithWorkoutExercise(Guid.NewGuid())
                         .WithSetNumber(3)
                         .WithWeightLbs(weightLbs)
                         .WithReps(5)
                         .Build();

            set.Weight.ToKilograms().Should().BeApproximately(expectedKg, 0.001m);
        }

        [Fact]
        public void WithRest_Should_Throw_When_Negative_Value_Provided()
        {
            var builder = Set.CreateBuilder()
                             .WithWorkoutExercise(Guid.NewGuid())
                             .WithSetNumber(1)
                             .WithWeightKg(10)
                             .WithReps(5);

            Action act = () => builder.WithRest(-10);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Rest seconds cannot be negative");
        }

        [Fact]
        public void Build_Should_Throw_If_Weight_Is_Null()
        {
            var builder = new Set.SetBuilder()
                .WithWorkoutExercise(Guid.NewGuid())
                .WithSetNumber(1)
                .WithReps(5);

            Action act = () => builder.Build();

            act.Should().Throw<ValidationException>();
        }

        #region Constructor Validation

        [Fact]
        public void Constructor_WithEmptyWorkoutExerciseId_ShouldThrowValidationException()
        {
            Action act = () => Set.CreateBuilder()
                .WithWorkoutExercise(Guid.Empty)
                .WithSetNumber(1)
                .WithWeightKg(50)
                .WithReps(10)
                .Build();

            act.Should().Throw<ValidationException>().WithMessage("*workout exercise id*");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidSetNumber_ShouldThrowValidationException(int invalidSetNumber)
        {
            Action act = () => Set.CreateBuilder()
                .WithWorkoutExercise(_workoutExerciseId)
                .WithSetNumber(invalidSetNumber)
                .WithWeightKg(50)
                .WithReps(10)
                .Build();

            act.Should().Throw<ValidationException>().WithMessage("*set number*");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_WithInvalidReps_ShouldThrowValidationException(int invalidReps)
        {
            Action act = () => Set.CreateBuilder()
                .WithWorkoutExercise(_workoutExerciseId)
                .WithSetNumber(1)
                .WithWeightKg(50)
                .WithReps(invalidReps)
                .Build();

            act.Should().Throw<ValidationException>().WithMessage("*reps*");
        }

        [Fact]
        public void Constructor_WithRepsExceedingMax_ShouldThrowValidationException()
        {
            Action act = () => Set.CreateBuilder()
                .WithWorkoutExercise(_workoutExerciseId)
                .WithSetNumber(1)
                .WithWeightKg(50)
                .WithReps(Set.MaxReps + 1)
                .Build();

            act.Should().Throw<ValidationException>().WithMessage("*reps*");
        }

        #endregion

        #region Update Methods

        [Fact]
        public void UpdateSetNumber_WithValidNumber_ShouldChangeSetNumber()
        {
            var set = SetFactory.Default();

            set.UpdateSetNumber(2);

            set.SetNumber.Should().Be(2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void UpdateSetNumber_WithInvalidNumber_ShouldThrowArgumentException(int invalidNumber)
        {
            var set = SetFactory.Default();

            Action act = () => set.UpdateSetNumber(invalidNumber);

            act.Should().Throw<ArgumentException>().WithMessage("*set number*");
        }

        [Fact]
        public void UpdateWeight_WithValidWeight_ShouldUpdateWeight()
        {
            var set = SetFactory.Default();
            var newWeight = Weight.FromKilograms(60);

            set.UpdateWeight(newWeight);

            set.Weight.ToKilograms().Should().Be(60m);
        }

        [Fact]
        public void UpdateWeight_WithNull_ShouldThrowArgumentNullException()
        {
            var set = SetFactory.Default();

            Action act = () => set.UpdateWeight(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void IncreaseWeightKg_WithPositiveAmount_ShouldIncreaseWeight()
        {
            var set = SetFactory.Default();
            var initialWeight = set.Weight.ToKilograms();

            set.IncreaseWeightKg(10);

            set.Weight.ToKilograms().Should().Be(initialWeight + 10);
        }

        [Fact]
        public void IncreaseWeightKg_WithNegativeAmount_ShouldThrowArgumentException()
        {
            var set = SetFactory.Default();

            Action act = () => set.IncreaseWeightKg(-5);

            act.Should().Throw<ArgumentException>().WithMessage("*amount*");
        }

        [Fact]
        public void DecreaseWeightKg_WithPositiveAmount_ShouldDecreaseWeight()
        {
            var set = SetFactory.Default();
            set.UpdateWeight(Weight.FromKilograms(100));

            set.DecreaseWeightKg(10);

            set.Weight.ToKilograms().Should().Be(90m);
        }

        [Fact]
        public void DecreaseWeightKg_WithNegativeAmount_ShouldThrowArgumentException()
        {
            var set = SetFactory.Default();

            Action act = () => set.DecreaseWeightKg(-5);

            act.Should().Throw<ArgumentException>().WithMessage("*amount*");
        }

        [Fact]
        public void DecreaseWeightKg_BelowZero_ShouldThrowInvalidOperationException()
        {
            var set = SetFactory.Default();

            Action act = () => set.DecreaseWeightKg(set.Weight.ToKilograms() + 1);

            act.Should().Throw<InvalidOperationException>().WithMessage("*weight cannot be negative*");
        }

        [Fact]
        public void UpdateReps_WithValidValue_ShouldChangeReps()
        {
            var set = SetFactory.Default();

            set.UpdateReps(12);

            set.Reps.Should().Be(12);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void UpdateReps_WithNonPositiveValue_ShouldThrowArgumentException(int invalidReps)
        {
            var set = SetFactory.Default();

            Action act = () => set.UpdateReps(invalidReps);

            act.Should().Throw<ArgumentException>().WithMessage("*reps*");
        }

        [Fact]
        public void UpdateReps_ExceedingMax_ShouldThrowArgumentException()
        {
            var set = SetFactory.Default();

            Action act = () => set.UpdateReps(Set.MaxReps + 1);

            act.Should().Throw<ArgumentException>().WithMessage("*reps*");
        }

        [Fact]
        public void UpdateRest_WithValidValue_ShouldUpdateRestSeconds()
        {
            var set = SetFactory.Default();

            set.UpdateRest(60);

            set.RestSeconds.Should().Be(60);
        }

        [Fact]
        public void UpdateRest_WithNull_ShouldSetRestSecondsNull()
        {
            var set = SetFactory.Default();
            set.UpdateRest(60);

            set.UpdateRest(null);

            set.RestSeconds.Should().BeNull();
        }

        [Fact]
        public void UpdateRest_WithNegativeValue_ShouldThrowArgumentException()
        {
            var set = SetFactory.Default();

            Action act = () => set.UpdateRest(-5);

            act.Should().Throw<ArgumentException>().WithMessage("*rest seconds cannot be negative*");
        }

        [Fact]
        public void UpdateRest_ExceedingMax_ShouldThrowArgumentException()
        {
            var set = SetFactory.Default();

            Action act = () => set.UpdateRest(Set.MaxRestSeconds + 1);

            act.Should().Throw<ArgumentException>().WithMessage("*rest cannot exceed*");
        }

        [Fact]
        public void ChangeSetType_ShouldUpdateSetType()
        {
            var set = SetFactory.Default();

            set.ChangeSetType(SetType.Dropset);

            set.SetType.Should().Be(SetType.Dropset);
        }

        [Fact]
        public void Complete_ShouldMarkCompleted()
        {
            var set = SetFactory.Default();

            set.Complete();

            set.IsCompleted.Should().BeTrue();
            set.CompletedAt.Should().NotBeNull();
        }

        [Fact]
        public void Uncomplete_ShouldMarkNotCompleted()
        {
            var set = SetFactory.CompletedSet();

            set.Uncomplete();

            set.IsCompleted.Should().BeFalse();
            set.CompletedAt.Should().BeNull();
        }

        #endregion

        #region Calculations

        [Fact]
        public void CalculateVolume_ShouldReturnCorrectWeightTimesReps()
        {
            var set = SetFactory.Default();

            var expected = set.Weight.ToKilograms() * set.Reps;

            set.CalculateVolume().Should().Be(expected);
        }

        [Fact]
        public void CalculateVolumeLbs_ShouldReturnCorrectWeightTimesReps()
        {
            var set = SetFactory.Default();

            var expected = set.Weight.ToPounds() * set.Reps;

            set.CalculateVolumeLbs().Should().Be(expected);
        }

        #endregion

        #region IsPR Tests

        [Fact]
        public void IsPR_WithNoPreviousSets_ShouldReturnTrue()
        {
            var set = SetFactory.Default();

            var result = set.IsPR(new List<Set>());

            result.Should().BeTrue();
        }

        [Fact]
        public void IsPR_WithHigherWeight_ShouldReturnTrue()
        {
            var set = SetFactory.Default();
            var previousSets = new List<Set>
            {
                SetFactory.Default()
            };
            previousSets[0].UpdateWeight(Weight.FromKilograms(40));

            var result = set.IsPR(previousSets);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsPR_WithLowerWeight_ShouldReturnFalse()
        {
            var set = SetFactory.Default();
            var previousSets = new List<Set>
            {
                SetFactory.Default()
            };
            previousSets[0].UpdateWeight(Weight.FromKilograms(60));

            var result = set.IsPR(previousSets);

            result.Should().BeFalse();
        }

        #endregion
    }
}
