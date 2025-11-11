using System;
using FluentAssertions;
using FitTracker.Domain.Entities;
using FitTracker.Tests.Factories;
using Xunit;

namespace FitTracker.Tests.Domain.Entities
{
    public class WorkoutTemplateTests
    {
        [Fact]
        public void Build_Should_Create_WorkoutTemplate_With_Correct_Properties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var name = "Test Workout";
            var description = "Test Description";

            // Act
            var workoutTemplate = WorkoutTemplate.CreateBuilder()
                .ForUser(userId)
                .WithName(name)
                .WithDescription(description)
                .Build();

            // Assert
            workoutTemplate.Should().NotBeNull();
            workoutTemplate.UserId.Should().Be(userId);
            workoutTemplate.Name.Should().Be(name);
            workoutTemplate.Description.Should().Be(description);
            workoutTemplate.UsageCount.Should().Be(0);
            workoutTemplate.LastUsedAt.Should().BeNull();
        }

        [Fact]
        public void Build_Should_Create_WorkoutTemplate_With_Empty_Description_If_Not_Set()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var name = "Test Workout";

            // Act
            var workoutTemplate = WorkoutTemplate.CreateBuilder()
                .ForUser(userId)
                .WithName(name)
                .Build();

            // Assert
            workoutTemplate.Should().NotBeNull();
            workoutTemplate.UserId.Should().Be(userId);
            workoutTemplate.Name.Should().Be(name);
            workoutTemplate.Description.Should().BeNull();
        }

        [Fact]
        public void Build_Should_Throw_Exception_If_Name_Is_Empty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var emptyName = "";

            // Act
            Action act = () => WorkoutTemplate.CreateBuilder()
                .ForUser(userId)
                .WithName(emptyName)
                .Build();

            // Assert
            act.Should().Throw<FluentValidation.ValidationException>();
        }


        [Fact]
        public void Constructor_Should_Initialize_Properties_Correctly()
        {
            var userId = Guid.NewGuid();
            var name = "Morning Routine";
            var description = "Description of the workout";

            var template = WorkoutTemplateFactory.Create(userId, name, description);

            template.UserId.Should().Be(userId);
            template.Name.Should().Be(name);
            template.Description.Should().Be(description);
            template.UsageCount.Should().Be(0);
            template.LastUsedAt.Should().BeNull();
        }

        [Fact]
        public void Update_Should_Change_Name_And_Description_And_Update_UpdatedAt()
        {
            var template = WorkoutTemplateFactory.Create();
            var newName = "New Name";
            var newDescription = "New Description";

            template.Update(newName, newDescription);

            template.Name.Should().Be(newName);
            template.Description.Should().Be(newDescription);
            template.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Update_Should_Throw_When_Name_Is_Null_Or_Whitespace()
        {
            var template = WorkoutTemplateFactory.Create();

            Action act = () => template.Update("  ");

            act.Should().Throw<ArgumentException>()
               .WithMessage("Template name cannot be empty*");
        }

        [Fact]
        public void RecordUsage_Should_Increment_UsageCount_And_Update_LastUsedAt_And_UpdatedAt()
        {
            var template = WorkoutTemplateFactory.Create();

            template.RecordUsage();

            template.UsageCount.Should().Be(1);
            template.LastUsedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            template.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
