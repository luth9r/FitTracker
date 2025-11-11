using System;
using FluentAssertions;
using FitTracker.Domain.Entities;
using FitTracker.Tests.Factories;
using Xunit;

namespace FitTracker.Tests.Domain.Entities
{
    public class WorkoutTemplateExerciseTests
    {
        [Fact]
        public void Constructor_Should_Initialize_Properties_Correctly()
        {
            var templateId = Guid.NewGuid();
            var exerciseId = Guid.NewGuid();
            var orderIndex = 3;
            var notes = "Test notes";

            var exercise = WorkoutTemplateExerciseFactory.Create(templateId, exerciseId, orderIndex, notes);

            exercise.WorkoutTemplateId.Should().Be(templateId);
            exercise.ExerciseId.Should().Be(exerciseId);
            exercise.OrderIndex.Should().Be(orderIndex);
            exercise.Notes.Should().Be(notes);
        }

        [Fact]
        public void UpdateOrder_Should_Change_OrderIndex_And_Update_UpdatedAt()
        {
            var exercise = WorkoutTemplateExerciseFactory.Create(orderIndex: 2);
            var newOrder = 5;

            exercise.UpdateOrder(newOrder);

            exercise.OrderIndex.Should().Be(newOrder);
            exercise.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateOrder_Should_Throw_When_Order_Less_Than_One()
        {
            var exercise = WorkoutTemplateExerciseFactory.Create();

            Action act = () => exercise.UpdateOrder(0);

            act.Should().Throw<ArgumentException>().WithMessage("Order must be at least 1*");
        }

        [Fact]
        public void UpdateNotes_Should_Change_Notes_And_Update_UpdatedAt()
        {
            var exercise = WorkoutTemplateExerciseFactory.Create(notes: "Initial notes");
            var newNotes = "Updated notes";

            exercise.UpdateNotes(newNotes);

            exercise.Notes.Should().Be(newNotes);
            exercise.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateNotes_Should_Allow_Null_Notes()
        {
            var exercise = WorkoutTemplateExerciseFactory.Create(notes: "Some notes");

            exercise.UpdateNotes(null);

            exercise.Notes.Should().BeNull();
            exercise.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
