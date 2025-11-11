using System;
using FitTracker.Domain.Entities;

namespace FitTracker.Tests.Factories
{
    public static class WorkoutTemplateExerciseFactory
    {
        public static WorkoutTemplateExercise Create(
            Guid? workoutTemplateId = null,
            Guid? exerciseId = null,
            int orderIndex = 1,
            string? notes = null)
        {
            return new WorkoutTemplateExercise(
                workoutTemplateId ?? Guid.NewGuid(),
                exerciseId ?? Guid.NewGuid(),
                orderIndex,
                notes);
        }
    }
}
