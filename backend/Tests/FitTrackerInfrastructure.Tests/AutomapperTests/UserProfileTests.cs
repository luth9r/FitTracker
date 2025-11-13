using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.AutomapperTests
{
    public class UserProfileTests
    {
        private readonly IMapper _mapper;

        public UserProfileTests()
        {
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<UserProfile>();

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
        public void Should_Map_User_To_UserEf()
        {
            // Arrange
            var username = "johndoe";
            var email = "johndoe@example.com";
            var passwordHash = "hashed_password";
            var firstName = "John";
            var lastName = "Doe";
            var avatar = "avatar_url";
            var bio = "About John";
            var preferredUnits = UnitSystem.Metric;

            var user = new User(
                username,
                email,
                passwordHash,
                firstName,
                lastName,
                avatar,
                bio,
                preferredUnits
            );

            // Act
            var result = _mapper.Map<UserEf>(user);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            result.Email.Should().Be(email);
            result.PasswordHash.Should().Be(passwordHash);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
            result.Avatar.Should().Be(avatar);
            result.Bio.Should().Be(bio);
            result.PreferredUnits.Should().Be(preferredUnits.ToString());
            result.Workouts.Should().BeEmpty();
            result.CustomExercises.Should().BeEmpty();
            result.WorkoutTemplates.Should().BeEmpty();
            result.Achievements.Should().BeEmpty();
            result.ExerciseRecords.Should().BeEmpty();
        }

        [Fact]
        public void Should_Map_UserEf_To_User()
        {
            // Arrange
            var username = "janedoe";
            var email = "janedoe@example.com";
            var passwordHash = "hashed_password2";
            var firstName = "Jane";
            var lastName = "Doe";
            var avatar = "avatar_url_2";
            var bio = "About Jane";
            var preferredUnits = "Imperial";

            var userEf = new UserEf
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Avatar = avatar,
                Bio = bio,
                PreferredUnits = preferredUnits
            };

            // Act
            var result = _mapper.Map<User>(userEf);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            result.Email.Should().Be(email);
            result.PasswordHash.Should().Be(passwordHash);
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
            result.Avatar.Should().Be(avatar);
            result.Bio.Should().Be(bio);
            result.PreferredUnits.Should().Be(UnitSystem.FromString(preferredUnits));
        }
    }
}
