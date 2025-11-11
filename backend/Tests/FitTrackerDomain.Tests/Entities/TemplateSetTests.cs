using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Tests.Factories;
using FluentAssertions;

namespace FitTracker.Domain.Tests.Entities
{
    public class TemplateSetTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidData_ShouldCreateTemplateSet()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();
            var setNumber = 1;
            var plannedWeight = 50m;
            var plannedReps = 10;
            var restSeconds = 90;
            var setType = SetType.Normal;

            // Act
            var templateSet = new TemplateSet(
                templateExerciseId,
                setNumber,
                plannedWeight,
                plannedReps,
                restSeconds,
                setType);

            // Assert
            templateSet.Should().NotBeNull();
            templateSet.WorkoutTemplateExerciseId.Should().Be(templateExerciseId);
            templateSet.SetNumber.Should().Be(setNumber);
            templateSet.PlannedWeight.ToKilograms().Should().Be(plannedWeight);
            templateSet.PlannedReps.Should().Be(plannedReps);
            templateSet.RestSeconds.Should().Be(restSeconds);
            templateSet.SetType.Should().Be(setType);
        }

        [Fact]
        public void Constructor_WithNullRestSeconds_ShouldCreateTemplateSet()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var templateSet = new TemplateSet(
                templateExerciseId,
                1,
                50m,
                10,
                null,
                SetType.Normal);

            // Assert
            templateSet.RestSeconds.Should().BeNull();
        }

        #endregion

        #region Builder Tests

        [Fact]
        public void Builder_WithValidData_ShouldBuildTemplateSet()
        {
            // Arrange & Act
            var templateSet = TemplateSet.CreateBuilder()
                .WithTemplateExercise(Guid.NewGuid())
                .WithSetNumber(2)
                .WithPlannedWeight(60m)
                .WithPlannedReps(8)
                .WithRest(120)
                .WithSetType(SetType.Dropset)
                .Build();

            // Assert
            templateSet.Should().NotBeNull();
            templateSet.SetNumber.Should().Be(2);
            templateSet.PlannedWeight.ToKilograms().Should().Be(60m);
            templateSet.PlannedReps.Should().Be(8);
            templateSet.RestSeconds.Should().Be(120);
            templateSet.SetType.Should().Be(SetType.Dropset);
        }

        [Fact]
        public void Builder_WithMinimalData_ShouldBuildTemplateSet()
        {
            // Arrange & Act
            var templateSet = TemplateSet.CreateBuilder()
                .WithTemplateExercise(Guid.NewGuid())
                .WithSetNumber(1)
                .WithPlannedWeight(20m)
                .WithPlannedReps(10)
                .Build();

            // Assert
            templateSet.Should().NotBeNull();
            templateSet.SetType.Should().Be(SetType.Normal);
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_WithValidData_ShouldUpdateTemplateSet()
        {
            // Arrange
            var templateSet = TemplateSetFactory.Default();
            var newWeight = 100m;
            var newReps = 5;
            var newRest = 180;

            // Act
            templateSet.Update(newWeight, newReps, newRest);

            // Assert
            templateSet.PlannedWeight.ToKilograms().Should().Be(newWeight);
            templateSet.PlannedReps.Should().Be(newReps);
            templateSet.RestSeconds.Should().Be(newRest);
            templateSet.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Update_WithNullRestSeconds_ShouldUpdateTemplateSet()
        {
            // Arrange
            var templateSet = TemplateSetFactory.Default();

            // Act
            templateSet.Update(50m, 10, null);

            // Assert
            templateSet.RestSeconds.Should().BeNull();
        }

        #endregion

        #region Object Mother Tests

        [Fact]
        public void TemplateSetMother_Default_ShouldCreateValidSet()
        {
            // Act
            var templateSet = TemplateSetFactory.Default();

            // Assert
            templateSet.Should().NotBeNull();
            templateSet.PlannedWeight.ToKilograms().Should().Be(20m);
            templateSet.PlannedReps.Should().Be(10);
            templateSet.RestSeconds.Should().Be(60);
        }

        [Fact]
        public void TemplateSetMother_WarmupSet_ShouldCreateWarmupSet()
        {
            // Act
            var templateSet = TemplateSetFactory.WarmupSet();

            // Assert
            templateSet.SetType.Should().Be(SetType.Warmup);
            templateSet.PlannedWeight.ToKilograms().Should().Be(10m);
            templateSet.PlannedReps.Should().Be(15);
        }

        [Fact]
        public void TemplateSetMother_HeavySet_ShouldCreateHeavySet()
        {
            // Act
            var templateSet = TemplateSetFactory.HeavySet();

            // Assert
            templateSet.PlannedWeight.ToKilograms().Should().Be(100m);
            templateSet.PlannedReps.Should().Be(5);
            templateSet.RestSeconds.Should().Be(180);
        }

        [Fact]
        public void TemplateSetMother_DropSet_ShouldCreateDropSet()
        {
            // Act
            var templateSet = TemplateSetFactory.DropSet();

            // Assert
            templateSet.SetType.Should().Be(SetType.Dropset);
        }

        [Fact]
        public void TemplateSetMother_BodyweightSet_ShouldCreateZeroWeightSet()
        {
            // Act
            var templateSet = TemplateSetFactory.BodyweightSet();

            // Assert
            templateSet.PlannedWeight.ToKilograms().Should().Be(0m);
        }

        [Fact]
        public void TemplateSetMother_NoRestSet_ShouldCreateSetWithZeroRest()
        {
            // Act
            var templateSet = TemplateSetFactory.NoRestSet();

            // Assert
            templateSet.RestSeconds.Should().Be(0);
        }

        #endregion

        #region Collection Tests

        [Fact]
        public void TemplateSetMother_ProgressiveOverloadSequence_ShouldCreateThreeSetsWithIncreasingWeight()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var sets = TemplateSetFactory.ProgressiveOverloadSequence(templateExerciseId);

            // Assert
            sets.Should().HaveCount(3);
            sets[0].PlannedWeight.ToKilograms().Should().Be(40m);
            sets[1].PlannedWeight.ToKilograms().Should().Be(50m);
            sets[2].PlannedWeight.ToKilograms().Should().Be(60m);
            sets.Should().OnlyContain(s => s.WorkoutTemplateExerciseId == templateExerciseId);
        }

        [Fact]
        public void TemplateSetMother_WarmupAndWorkingSets_ShouldCreateWarmupFollowedByWorkingSets()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var sets = TemplateSetFactory.WarmupAndWorkingSets(templateExerciseId);

            // Assert
            sets.Should().HaveCount(4);
            sets[0].SetType.Should().Be(SetType.Warmup);
            sets.Skip(1).Should().OnlyContain(s => s.SetType == SetType.Normal);
        }

        [Fact]
        public void TemplateSetMother_PyramidSequence_ShouldCreatePyramidPattern()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var sets = TemplateSetFactory.PyramidSequence(templateExerciseId);

            // Assert
            sets.Should().HaveCount(5);
            sets.Should().OnlyContain(s => s.SetType == SetType.Pyramid);
            sets[2].PlannedWeight.ToKilograms().Should().BeGreaterThan(sets[0].PlannedWeight.ToKilograms());
            sets[2].PlannedWeight.ToKilograms().Should().BeGreaterThan(sets[4].PlannedWeight.ToKilograms());
        }

        [Fact]
        public void TemplateSetMother_Standard3x10_ShouldCreateThreeSetsOfTenReps()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var sets = TemplateSetFactory.Standard3x10(templateExerciseId);

            // Assert
            sets.Should().HaveCount(3);
            sets.Should().OnlyContain(s => s.PlannedReps == 10);
            sets.Should().OnlyContain(s => s.PlannedWeight.ToKilograms() == 50m);
        }

        [Fact]
        public void TemplateSetMother_Standard5x5_ShouldCreateFiveSetsOfFiveReps()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var sets = TemplateSetFactory.Standard5x5(templateExerciseId);

            // Assert
            sets.Should().HaveCount(5);
            sets.Should().OnlyContain(s => s.PlannedReps == 5);
            sets.Should().OnlyContain(s => s.PlannedWeight.ToKilograms() == 80m);
        }

        [Fact]
        public void TemplateSetMother_DropsetSequence_ShouldCreateDropsetWithDecreasingWeight()
        {
            // Arrange
            var templateExerciseId = Guid.NewGuid();

            // Act
            var sets = TemplateSetFactory.DropsetSequence(templateExerciseId);

            // Assert
            sets.Should().HaveCount(3);
            sets.Should().OnlyContain(s => s.SetType == SetType.Dropset);
            sets[0].PlannedWeight.ToKilograms().Should().BeGreaterThan(sets[1].PlannedWeight.ToKilograms());
            sets[1].PlannedWeight.ToKilograms().Should().BeGreaterThan(sets[2].PlannedWeight.ToKilograms());
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void TemplateSet_WithMaxWeight_ShouldCreateSuccessfully()
        {
            // Act
            var templateSet = TemplateSetFactory.MaxWeightSet();

            // Assert
            templateSet.PlannedWeight.ToKilograms().Should().Be(200m);
        }

        [Fact]
        public void TemplateSet_WithLongRest_ShouldCreateSuccessfully()
        {
            // Act
            var templateSet = TemplateSetFactory.LongRestSet();

            // Assert
            templateSet.RestSeconds.Should().Be(300);
        }

        #endregion
    }
}
