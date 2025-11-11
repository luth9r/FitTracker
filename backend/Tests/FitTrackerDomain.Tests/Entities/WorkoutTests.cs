using System;
using System.Threading;
using Xunit;
using FluentAssertions;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Tests.Factories;

namespace FitTracker.Domain.Tests.Entities
{
    public class WorkoutTests
    {
        [Fact]
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            var builder = Workout.CreateBuilder();

            builder.Should().NotBeNull();
            builder.Should().BeOfType<Workout.WorkoutBuilder>();
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateWorkout()
        {
            var userId = Guid.NewGuid();

            var workout = WorkoutFactory.Default(userId);

            workout.Should().NotBeNull();
            workout.UserId.Should().Be(userId);
            workout.Name.Should().Be("Default Workout");
            workout.WorkoutDate.Should().BeCloseTo(DateTime.UtcNow.Date, TimeSpan.FromSeconds(1));
            workout.IsCompleted.Should().BeFalse();
            workout.IsInProgress.Should().BeFalse();
            workout.TotalVolumeKg.Should().Be(0);
        }

        [Fact]
        public void Start_ShouldSetInProgressAndStartAt()
        {
            var workout = WorkoutFactory.Default();

            workout.Start();

            workout.IsInProgress.Should().BeTrue();
            workout.StartedAt.Should().NotBeNull();
            workout.Duration.Should().BeCloseTo(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Pause_ShouldSetNotInProgressAndUpdateDuration()
        {
            var workout = WorkoutFactory.Default();
            workout.Start();

            Thread.Sleep(10);

            workout.Pause();

            workout.IsInProgress.Should().BeFalse();
            workout.Duration.Should().BeGreaterThan(TimeSpan.Zero);
        }

        [Fact]
        public void Resume_ShouldSetInProgressAndAdjustStartedAt()
        {
            var workout = WorkoutFactory.Default();
            workout.Start();
            workout.Pause();

            var oldDuration = workout.Duration;

            workout.Resume();

            workout.IsInProgress.Should().BeTrue();
            workout.StartedAt.Should().NotBeNull();
            workout.Duration.Should().BeCloseTo(oldDuration, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Complete_ShouldMarkCompletedAndSetCompletedAt()
        {
            var workout = WorkoutFactory.Default();
            workout.Start();

            Thread.Sleep(10);

            workout.Complete();

            workout.IsCompleted.Should().BeTrue();
            workout.IsInProgress.Should().BeFalse();
            workout.CompletedAt.Should().NotBeNull();
            workout.Duration.Should().BeGreaterThan(TimeSpan.Zero);
        }

        [Fact]
        public void Uncomplete_ShouldResetCompletedFlagAndCompletedAt()
        {
            var workout = WorkoutFactory.CompletedWorkout();

            workout.Uncomplete();

            workout.IsCompleted.Should().BeFalse();
            workout.CompletedAt.Should().BeNull();
        }

        [Fact]
        public void SetDuration_WithValidDuration_ShouldSetDuration()
        {
            var workout = WorkoutFactory.Default();
            workout.Start();

            workout.Pause();

            var newDuration = TimeSpan.FromMinutes(30);
            workout.SetDuration(newDuration);

            workout.Duration.Should().Be(newDuration);
        }

        [Fact]
        public void SetDuration_WithDurationTooLong_ShouldThrow()
        {
            var workout = WorkoutFactory.Default();

            var duration = TimeSpan.FromHours(Workout.MaxDurationHours + 1);

            Action act = () => workout.SetDuration(duration);

            act.Should().Throw<ArgumentException>().WithMessage($"Duration cannot exceed {Workout.MaxDurationHours} hours");
        }

        [Fact]
        public void SetDuration_WhenInProgress_ShouldThrow()
        {
            var workout = WorkoutFactory.Default();
            workout.Start();

            var duration = TimeSpan.FromMinutes(10);

            Action act = () => workout.SetDuration(duration);

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot manually set duration while workout is in progress");
        }

        [Fact]
        public void Update_WithValidData_ShouldUpdateProperties()
        {
            var workout = WorkoutFactory.Default();

            var newName = "Updated Workout";
            var newDate = DateTime.UtcNow.AddDays(1);
            var newNotes = "Updated notes";

            workout.Update(newName, newDate, newNotes);

            workout.Name.Should().Be(newName);
            workout.WorkoutDate.Should().Be(newDate);
            workout.Notes.Should().Be(newNotes);
        }

        [Fact]
        public void IsToday_ShouldReturnTrueForToday()
        {
            var workout = WorkoutFactory.Default();

            workout.IsToday().Should().BeTrue();
        }

        [Fact]
        public void IsPast_ShouldReturnTrueForPastDate()
        {
            var workout = WorkoutFactory.Default();
            workout.Update(workout.Name, DateTime.UtcNow.AddDays(-1));

            workout.IsPast().Should().BeTrue();
        }

        [Fact]
        public void IsFuture_ShouldReturnTrueForFutureDate()
        {
            var workout = WorkoutFactory.Default();
            workout.Update(workout.Name, DateTime.UtcNow.AddDays(1));

            workout.IsFuture().Should().BeTrue();
        }
    }
}
