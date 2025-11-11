using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTracker.Domain.Tests.Factories
{
    /// <summary>
    /// Factory for creating TemplateSet test data.
    /// </summary>
    public static class TemplateSetFactory
    {
        /// <summary>
        /// Creates a default template set with minimal data.
        /// </summary>
        public static TemplateSet Default() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(20m)
            .WithPlannedReps(10)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        #region Standard Sets

        /// <summary>
        /// Creates a light warm-up set.
        /// </summary>
        public static TemplateSet WarmupSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(10m)
            .WithPlannedReps(15)
            .WithRest(30)
            .WithSetType(SetType.Warmup)
            .Build();

        /// <summary>
        /// Creates a standard working set.
        /// </summary>
        public static TemplateSet WorkingSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(2)
            .WithPlannedWeight(60m)
            .WithPlannedReps(8)
            .WithRest(90)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a heavy working set.
        /// </summary>
        public static TemplateSet HeavySet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(100m)
            .WithPlannedReps(5)
            .WithRest(180)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a high-rep endurance set.
        /// </summary>
        public static TemplateSet EnduranceSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(20m)
            .WithPlannedReps(20)
            .WithRest(45)
            .WithSetType(SetType.Normal)
            .Build();

        #endregion

        #region Advanced Set Types

        /// <summary>
        /// Creates a dropset.
        /// </summary>
        public static TemplateSet DropSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(50m)
            .WithPlannedReps(10)
            .WithRest(60)
            .WithSetType(SetType.Dropset)
            .Build();

        /// <summary>
        /// Creates a superset.
        /// </summary>
        public static TemplateSet SuperSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(40m)
            .WithPlannedReps(12)
            .WithRest(0)
            .WithSetType(SetType.Superset)
            .Build();

        /// <summary>
        /// Creates a pyramid set.
        /// </summary>
        public static TemplateSet PyramidSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(70m)
            .WithPlannedReps(6)
            .WithRest(120)
            .WithSetType(SetType.Pyramid)
            .Build();

        /// <summary>
        /// Creates a rest-pause set.
        /// </summary>
        public static TemplateSet RestPauseSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(80m)
            .WithPlannedReps(8)
            .WithRest(15)
            .WithSetType(SetType.RestPause)
            .Build();

        #endregion

        #region Specific Weight Ranges

        /// <summary>
        /// Creates a bodyweight set (0kg).
        /// </summary>
        public static TemplateSet BodyweightSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(0m)
            .WithPlannedReps(15)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a light set (10-30kg).
        /// </summary>
        public static TemplateSet LightSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(15m)
            .WithPlannedReps(15)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a moderate set (30-70kg).
        /// </summary>
        public static TemplateSet ModerateSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(50m)
            .WithPlannedReps(10)
            .WithRest(90)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a maximum weight set.
        /// </summary>
        public static TemplateSet MaxWeightSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(200m)
            .WithPlannedReps(1)
            .WithRest(300)
            .WithSetType(SetType.Normal)
            .Build();

        #endregion

        #region Rest Period Variations

        /// <summary>
        /// Creates a set with no rest.
        /// </summary>
        public static TemplateSet NoRestSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(30m)
            .WithPlannedReps(12)
            .WithRest(0)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a set with short rest (30s).
        /// </summary>
        public static TemplateSet ShortRestSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(40m)
            .WithPlannedReps(12)
            .WithRest(30)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a set with medium rest (90s).
        /// </summary>
        public static TemplateSet MediumRestSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(60m)
            .WithPlannedReps(8)
            .WithRest(90)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a set with long rest (3-5 minutes).
        /// </summary>
        public static TemplateSet LongRestSet() => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(100m)
            .WithPlannedReps(5)
            .WithRest(300)
            .WithSetType(SetType.Normal)
            .Build();

        #endregion

        #region Custom Configurations

        /// <summary>
        /// Creates a template set for a specific exercise.
        /// </summary>
        public static TemplateSet WithTemplateExercise(Guid templateExerciseId) => TemplateSet.CreateBuilder()
            .WithTemplateExercise(templateExerciseId)
            .WithSetNumber(1)
            .WithPlannedWeight(50m)
            .WithPlannedReps(10)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a template set with a specific set number.
        /// </summary>
        public static TemplateSet WithSetNumber(int setNumber) => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(setNumber)
            .WithPlannedWeight(50m)
            .WithPlannedReps(10)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a template set with specific weight.
        /// </summary>
        public static TemplateSet WithWeight(decimal weight) => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(weight)
            .WithPlannedReps(10)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a template set with specific reps.
        /// </summary>
        public static TemplateSet WithReps(int reps) => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(50m)
            .WithPlannedReps(reps)
            .WithRest(60)
            .WithSetType(SetType.Normal)
            .Build();

        /// <summary>
        /// Creates a template set with a specific type.
        /// </summary>
        public static TemplateSet WithType(SetType setType) => TemplateSet.CreateBuilder()
            .WithTemplateExercise(Guid.NewGuid())
            .WithSetNumber(1)
            .WithPlannedWeight(50m)
            .WithPlannedReps(10)
            .WithRest(60)
            .WithSetType(setType)
            .Build();

        #endregion

        #region Collections

        /// <summary>
        /// Creates a progressive overload sequence (3 sets, increasing weight).
        /// </summary>
        public static List<TemplateSet> ProgressiveOverloadSequence(Guid templateExerciseId) => new List<TemplateSet>
        {
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(1).WithPlannedWeight(40m).WithPlannedReps(10).WithRest(90).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(2).WithPlannedWeight(50m).WithPlannedReps(8).WithRest(90).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(3).WithPlannedWeight(60m).WithPlannedReps(6).WithRest(120).Build()
        };

        /// <summary>
        /// Creates a warmup + working sets sequence.
        /// </summary>
        public static List<TemplateSet> WarmupAndWorkingSets(Guid templateExerciseId) => new List<TemplateSet>
        {
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(1).WithPlannedWeight(20m).WithPlannedReps(15).WithRest(30).WithSetType(SetType.Warmup).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(2).WithPlannedWeight(60m).WithPlannedReps(8).WithRest(90).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(3).WithPlannedWeight(60m).WithPlannedReps(8).WithRest(90).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(4).WithPlannedWeight(60m).WithPlannedReps(8).WithRest(90).Build()
        };

        /// <summary>
        /// Creates a pyramid training sequence.
        /// </summary>
        public static List<TemplateSet> PyramidSequence(Guid templateExerciseId) => new List<TemplateSet>
        {
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(1).WithPlannedWeight(40m).WithPlannedReps(12).WithRest(60).WithSetType(SetType.Pyramid).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(2).WithPlannedWeight(60m).WithPlannedReps(8).WithRest(90).WithSetType(SetType.Pyramid).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(3).WithPlannedWeight(80m).WithPlannedReps(4).WithRest(120).WithSetType(SetType.Pyramid).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(4).WithPlannedWeight(60m).WithPlannedReps(8).WithRest(90).WithSetType(SetType.Pyramid).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(5).WithPlannedWeight(40m).WithPlannedReps(12).WithRest(60).WithSetType(SetType.Pyramid).Build()
        };

        /// <summary>
        /// Creates a standard 3x10 template.
        /// </summary>
        public static List<TemplateSet> Standard3x10(Guid templateExerciseId) => new List<TemplateSet>
        {
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(1).WithPlannedWeight(50m).WithPlannedReps(10).WithRest(90).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(2).WithPlannedWeight(50m).WithPlannedReps(10).WithRest(90).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(3).WithPlannedWeight(50m).WithPlannedReps(10).WithRest(90).Build()
        };

        /// <summary>
        /// Creates a standard 5x5 strength template.
        /// </summary>
        public static List<TemplateSet> Standard5x5(Guid templateExerciseId) => new List<TemplateSet>
        {
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(1).WithPlannedWeight(80m).WithPlannedReps(5).WithRest(180).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(2).WithPlannedWeight(80m).WithPlannedReps(5).WithRest(180).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(3).WithPlannedWeight(80m).WithPlannedReps(5).WithRest(180).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(4).WithPlannedWeight(80m).WithPlannedReps(5).WithRest(180).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(5).WithPlannedWeight(80m).WithPlannedReps(5).WithRest(180).Build()
        };

        /// <summary>
        /// Creates a dropset sequence.
        /// </summary>
        public static List<TemplateSet> DropsetSequence(Guid templateExerciseId) => new List<TemplateSet>
        {
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(1).WithPlannedWeight(60m).WithPlannedReps(8).WithRest(0).WithSetType(SetType.Dropset).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(2).WithPlannedWeight(40m).WithPlannedReps(10).WithRest(0).WithSetType(SetType.Dropset).Build(),
            TemplateSet.CreateBuilder().WithTemplateExercise(templateExerciseId).WithSetNumber(3).WithPlannedWeight(20m).WithPlannedReps(15).WithRest(120).WithSetType(SetType.Dropset).Build()
        };

        #endregion
    }
}
