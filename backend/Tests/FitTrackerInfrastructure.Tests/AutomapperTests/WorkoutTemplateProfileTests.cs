using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Xunit;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class WorkoutTemplateProfileTests
    {
        private readonly IMapper _mapper;

        public WorkoutTemplateProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<WorkoutTemplateProfile>();

            var config = new MapperConfiguration(
                configExpression,
                NullLoggerFactory.Instance
            );
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Configuration_Should_BeValid()
        {
            Action act = () => _mapper.ConfigurationProvider.AssertConfigurationIsValid();
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Map_WorkoutTemplate_To_WorkoutTemplateEf()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var name = "Strength Plan";
            var description = "Plan description";
            var usageCount = 7;
            var lastUsedAt = DateTime.UtcNow.AddDays(-3);

            var workoutTemplate = new WorkoutTemplate(
                userId,
                name,
                description,
                usageCount,
                lastUsedAt
            );

            // Act
            var result = _mapper.Map<WorkoutTemplateEf>(workoutTemplate);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Name.Should().Be(name);
            result.Description.Should().Be(description);
            result.UsageCount.Should().Be(usageCount);
            result.LastUsedAt.Should().Be(lastUsedAt);
            result.Exercises.Should().BeEmpty();
        }

        [Fact]
        public void Should_Map_WorkoutTemplateEf_To_WorkoutTemplate()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var name = "Endurance Plan";
            var description = "Endurance plan details";
            var usageCount = 12;
            var lastUsedAt = DateTime.UtcNow.AddDays(-1);

            var workoutTemplateEf = new WorkoutTemplateEf
            {
                UserId = userId,
                Name = name,
                Description = description,
                UsageCount = usageCount,
                LastUsedAt = lastUsedAt
            };

            // Act
            var result = _mapper.Map<WorkoutTemplate>(workoutTemplateEf);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Name.Should().Be(name);
            result.Description.Should().Be(description);
            result.UsageCount.Should().Be(usageCount);
            result.LastUsedAt.Should().Be(lastUsedAt);
        }
    }
}
