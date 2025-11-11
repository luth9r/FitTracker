using System;
using System.Collections.Generic;
using FitTracker.Domain.Entities;

namespace FitTracker.Domain.Tests.Factories
{
    /// <summary>
    /// Factory for creating Workout test data.
    /// </summary>
    public static class WorkoutFactory
    {
        /// <summary>
        /// Creates a default workout with minimal required data.
        /// </summary>
        public static Workout Default(Guid? userId = null) => Workout.CreateBuilder()
        .ForUser(userId ?? Guid.NewGuid())
        .WithName("Default Workout")
        .OnDate(DateTime.UtcNow.Date)
        .Build();


        /// <summary>
        /// Creates a completed workout with specified duration and volume.
        /// </summary>
        public static Workout CompletedWorkout(
            Guid? userId = null,
            TimeSpan? duration = null,
            decimal totalVolumeKg = 100) => new Workout(
                Guid.NewGuid(),
                userId ?? Guid.NewGuid(),
                "Completed Workout",
                DateTime.UtcNow.AddDays(-1),
                null,
                "Sample notes",
                duration ?? TimeSpan.FromMinutes(90),
                true,
                false,
                DateTime.UtcNow.AddDays(-1).AddHours(-1),
                DateTime.UtcNow.AddDays(-1),
                totalVolumeKg);

        /// <summary>
        /// Creates an in-progress workout.
        /// </summary>
        public static Workout InProgressWorkout(Guid? userId = null) => Workout.CreateBuilder()
            .ForUser(userId ?? Guid.NewGuid())
            .WithName("In Progress Workout")
            .OnDate(DateTime.UtcNow)
            .Build();

        /// <summary>
        /// Creates a workout with custom notes and template.
        /// </summary>
        public static Workout CustomWorkout(
            Guid? userId = null,
            Guid? templateId = null,
            string? notes = null) => Workout.CreateBuilder()
            .ForUser(userId ?? Guid.NewGuid())
            .WithName("Custom Workout")
            .OnDate(DateTime.UtcNow.Date)
            .FromTemplate(templateId)
            .WithNotes(notes)
            .Build();

        /// <summary>
        /// Creates a workout with maximum allowed duration.
        /// </summary>
        public static Workout MaxDurationWorkout(Guid? userId = null) => Workout.CreateBuilder()
            .ForUser(userId ?? Guid.NewGuid())
            .WithName("Max Duration Workout")
            .OnDate(DateTime.UtcNow)
            .WithNotes("Max duration")
            .Build();
    }
}