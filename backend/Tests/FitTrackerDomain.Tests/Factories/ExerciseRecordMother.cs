using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;

namespace FitTrackerDomain.Tests.Factories
{
    public static class ExerciseRecordMother
    {
        private static readonly Guid DefaultUserId = Guid.NewGuid();
        private static readonly Guid DefaultExerciseId = Guid.NewGuid();

        public static ExerciseRecord Default() =>
            ExerciseRecord.Create(DefaultUserId, DefaultExerciseId);

        public static ExerciseRecord WithValues(
            Guid? userId = null,
            Guid? exerciseId = null,
            Weight? maxWeight = null,
            int maxReps = 0,
            decimal maxVolume = 0,
            decimal maxTotalVolume = 0,
            DateTime? maxWeightDate = null,
            DateTime? maxRepsDate = null,
            DateTime? maxVolumeDate = null,
            DateTime? maxTotalVolumeDate = null,
            int totalWorkouts = 0,
            int totalSets = 0,
            int totalReps = 0,
            decimal totalLifted = 0,
            DateTime? lastPerformed = null)
        {
            return new ExerciseRecord(
                userId ?? DefaultUserId,
                exerciseId ?? DefaultExerciseId,
                maxWeight ?? Weight.FromKilograms(0),
                maxReps,
                maxVolume,
                maxTotalVolume,
                maxWeightDate ?? DateTime.UtcNow,
                maxRepsDate ?? DateTime.UtcNow,
                maxVolumeDate ?? DateTime.UtcNow,
                maxTotalVolumeDate ?? DateTime.UtcNow,
                totalWorkouts,
                totalSets,
                totalReps,
                totalLifted,
                lastPerformed ?? DateTime.UtcNow);
        }
    }
}
