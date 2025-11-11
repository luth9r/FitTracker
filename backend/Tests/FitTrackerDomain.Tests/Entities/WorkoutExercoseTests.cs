using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Tests.Factories;

namespace FitTracker.Domain.Tests.Entities
{
    public class WorkoutExerciseTests
    {
        [Fact]
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            var builder = WorkoutExercise.CreateBuilder();

            builder.Should().NotBeNull();
            builder.Should().BeOfType<WorkoutExercise.WorkoutExerciseBuilder>();
        }

        [Fact]
        public void Constructor_WithValidData_ShouldCreateWorkoutExercise()
        {
            var workoutExercise = WorkoutExerciseFactory.Default();

            workoutExercise.Should().NotBeNull();
            workoutExercise.OrderIndex.Should().Be(1);
            workoutExercise.Notes.Should().Be("Default notes");
            workoutExercise.WorkoutId.Should().NotBeEmpty();
            workoutExercise.ExerciseId.Should().NotBeEmpty();
        }

        [Fact]
        public void UpdateOrder_WithValidOrder_ShouldUpdateOrder()
        {
            var workoutExercise = WorkoutExerciseFactory.Default();

            workoutExercise.UpdateOrder(5);

            workoutExercise.OrderIndex.Should().Be(5);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(WorkoutExercise.MaxOrderIndex + 1)]
        public void UpdateOrder_WithInvalidOrder_ShouldThrowArgumentException(int invalidOrder)
        {
            var workoutExercise = WorkoutExerciseFactory.Default();

            Action act = () => workoutExercise.UpdateOrder(invalidOrder);

            act.Should().Throw<ArgumentException>().WithMessage("*order*");
        }

        [Fact]
        public void UpdateNotes_ShouldUpdateNotes()
        {
            var workoutExercise = WorkoutExerciseFactory.Default();

            workoutExercise.UpdateNotes("Updated notes");

            workoutExercise.Notes.Should().Be("Updated notes");
        }

        [Fact]
        public void UpdateNotes_WithNull_ShouldSetNotesNull()
        {
            var workoutExercise = WorkoutExerciseFactory.Default();

            workoutExercise.UpdateNotes(null);

            workoutExercise.Notes.Should().BeNull();
        }
    }
}