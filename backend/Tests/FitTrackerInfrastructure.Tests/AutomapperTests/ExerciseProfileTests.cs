using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.Enums;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class ExerciseProfileTests
    {
        private readonly IMapper _mapper;

        public ExerciseProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ExerciseProfile>();

            var config = new MapperConfiguration(
                configExpression,
                NullLoggerFactory.Instance
            );
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Configuration_Should_BeValid()
        {
            // Arrange & Act
            Action act = () => _mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Should_Map_Exercise_To_ExerciseEf()
        {
            // Arrange
            var exercise = new Exercise(
                name: "Bench Press",
                description: "Chest exercise",
                imageUrl: "https://example.com/image.jpg",
                videoUrl: "https://example.com/video.mp4",
                muscleGroup: MuscleGroup.Chest,
                equipment: Equipment.Barbell,
                isCustom: true,
                userId: Guid.NewGuid()
            );

            // Act
            var result = _mapper.Map<ExerciseEf>(exercise);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(exercise.Name);
            result.Description.Should().Be(exercise.Description);
            result.ImageUrl.Should().Be(exercise.ImageUrl);
            result.VideoUrl.Should().Be(exercise.VideoUrl);
            result.MuscleGroup.Should().Be((int)exercise.MuscleGroup);
            result.Equipment.Should().Be((int)exercise.Equipment);
            result.IsCustom.Should().Be(exercise.IsCustom);
            result.UserId.Should().Be(exercise.UserId);
            result.User.Should().BeNull();
            result.WorkoutExercises.Should().BeNull();
        }

        [Fact]
        public void Should_Map_ExerciseEf_To_Exercise()
        {
            // Arrange
            var exerciseEf = new ExerciseEf
            {
                Name = "Squat",
                Description = "Leg exercise",
                ImageUrl = "https://example.com/squat.jpg",
                VideoUrl = "https://example.com/squat.mp4",
                MuscleGroup = (int)MuscleGroup.Legs,
                Equipment = (int)Equipment.Barbell,
                IsCustom = false,
                UserId = Guid.NewGuid()
            };

            // Act
            var result = _mapper.Map<Exercise>(exerciseEf);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(exerciseEf.Name);
            result.Description.Should().Be(exerciseEf.Description);
            result.ImageUrl.Should().Be(exerciseEf.ImageUrl);
            result.VideoUrl.Should().Be(exerciseEf.VideoUrl);
            result.MuscleGroup.Should().Be((MuscleGroup)exerciseEf.MuscleGroup);
            result.Equipment.Should().Be((Equipment)exerciseEf.Equipment);
            result.IsCustom.Should().Be(exerciseEf.IsCustom);
            result.UserId.Should().Be(exerciseEf.UserId);
        }
    }
}
