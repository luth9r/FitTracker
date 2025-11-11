using System;
using System.Collections.Generic;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Tests.Factories;
using FluentAssertions;

namespace FitTracker.Domain.Tests.Factories
{
    /// <summary>
    /// Factory for creating WorkoutExercise test objects.
    /// </summary>
    public static class WorkoutExerciseFactory
    {
        public static WorkoutExercise Default(Guid? workoutId = null, Guid? exerciseId = null) =>
            WorkoutExercise.CreateBuilder()
                .WithWorkout(workoutId ?? Guid.NewGuid())
                .WithExercise(exerciseId ?? Guid.NewGuid())
                .WithOrder(1)
                .WithNotes("Default notes")
                .Build();

        public static WorkoutExercise WithOrder(int order, Guid? workoutId = null, Guid? exerciseId = null) =>
            WorkoutExercise.CreateBuilder()
                .WithWorkout(workoutId ?? Guid.NewGuid())
                .WithExercise(exerciseId ?? Guid.NewGuid())
                .WithOrder(order)
                .Build();

        public static WorkoutExercise WithoutNotes(Guid? workoutId = null, Guid? exerciseId = null) =>
            WorkoutExercise.CreateBuilder()
                .WithWorkout(workoutId ?? Guid.NewGuid())
                .WithExercise(exerciseId ?? Guid.NewGuid())
                .WithOrder(1)
                .WithNotes(null)
                .Build();

        public static List<WorkoutExercise> SampleSet(Guid? workoutId = null) => new List<WorkoutExercise>
        {
            WorkoutExercise.CreateBuilder()
                .WithWorkout(workoutId ?? Guid.NewGuid())
                .WithExercise(Guid.NewGuid())
                .WithOrder(1)
                .WithNotes("Warmup set")
                .Build(),
            WorkoutExercise.CreateBuilder()
                .WithWorkout(workoutId ?? Guid.NewGuid())
                .WithExercise(Guid.NewGuid())
                .WithOrder(2)
                .WithNotes("Working set")
                .Build(),
            WorkoutExercise.CreateBuilder()
                .WithWorkout(workoutId ?? Guid.NewGuid())
                .WithExercise(Guid.NewGuid())
                .WithOrder(3)
                .WithNotes("Cooldown set")
                .Build()
        };
    }
}
