using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;

namespace FitTrackerDomain.Tests.Factories
{
    public static class SetFactory
    {
        private static readonly Guid DefaultWorkoutExerciseId = Guid.NewGuid();

        public static Set Default() =>
            Set.CreateBuilder()
                .WithWorkoutExercise(DefaultWorkoutExerciseId)
                .WithSetNumber(1)
                .WithWeightKg(50)
                .WithReps(10)
                .WithSetType(SetType.Normal)
                .Build();

        public static Set WarmupSet() =>
            Set.CreateBuilder()
                .WithWorkoutExercise(DefaultWorkoutExerciseId)
                .WithSetNumber(1)
                .WithWeightKg(20)
                .WithReps(15)
                .WithSetType(SetType.Normal)
                .Build();

        public static Set Dropset() =>
            Set.CreateBuilder()
                .WithWorkoutExercise(DefaultWorkoutExerciseId)
                .WithSetNumber(2)
                .WithWeightKg(40)
                .WithReps(8)
                .WithSetType(SetType.Dropset)
                .Build();

        public static Set Superset() =>
            Set.CreateBuilder()
                .WithWorkoutExercise(DefaultWorkoutExerciseId)
                .WithSetNumber(3)
                .WithWeightKg(30)
                .WithReps(12)
                .WithSetType(SetType.Superset)
                .Build();

        public static Set CompletedSet() =>
            Set.CreateBuilder()
                .WithWorkoutExercise(DefaultWorkoutExerciseId)
                .WithSetNumber(1)
                .WithWeightKg(60)
                .WithReps(10)
                .WithSetType(SetType.Normal)
                .Build();
    }
}
