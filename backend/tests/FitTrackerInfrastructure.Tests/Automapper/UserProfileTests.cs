// FitTracker.Infrastructure.Tests/Automapper/UserProfileTests.cs
using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Automapper.Entities;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FluentAssertions;
using FitTrackerInfrastructure.Tests.Helpers;
using FitTrackerInfrastructure.Tests.TestDoubles;

namespace FitTrackerInfrastructure.Tests.Automapper;

public class UserProfileTests
{
    private readonly IMapper _mapper;

    public UserProfileTests()
    {
        var config = MapperConfigurationHelper.Create<UserProfile>();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_ShouldBeValid()
    {
        // Act & Assert
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void Should_Map_UserEf_To_User()
    {
        // Arrange
        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            FirstName = "John",
            LastName = "Doe",
            Avatar = "https://example.com/avatar.jpg",
            Bio = "Test bio",
            IsEmailVerified = true,
            GoogleProviderId = null,
            CreatedAt = DateTime.UtcNow.AddDays(-10),
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var user = _mapper.Map<User>(userEf);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(userEf.Id);
        user.Username.Should().Be(userEf.Username);
        user.Email.Should().Be(userEf.Email);
        user.PasswordHash.Should().Be(userEf.PasswordHash);
        user.FirstName.Should().Be(userEf.FirstName);
        user.LastName.Should().Be(userEf.LastName);
        user.Avatar.Should().Be(userEf.Avatar);
        user.Bio.Should().Be(userEf.Bio);
        user.IsEmailVerified.Should().BeTrue();
        user.GoogleProviderId.Should().BeNull();
        user.CreatedAt.Should().Be(userEf.CreatedAt);
        user.UpdatedAt.Should().Be(userEf.UpdatedAt);
    }

    [Fact]
    public void Should_Map_User_To_UserEf()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser(
            firstName: "John",
            lastName: "Doe");

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.Should().NotBeNull();
        userEf.Id.Should().Be(user.Id);
        userEf.Username.Should().Be(user.Username);
        userEf.Email.Should().Be(user.Email);
        userEf.PasswordHash.Should().Be(user.PasswordHash);
        userEf.FirstName.Should().Be(user.FirstName);
        userEf.LastName.Should().Be(user.LastName);
        userEf.CreatedAt.Should().Be(user.CreatedAt);
    }

    [Fact]
    public void Should_Map_UserEf_With_GoogleProviderId_To_User()
    {
        // Arrange
        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "google_user",
            Email = "google@example.com",
            PasswordHash = null,
            FirstName = "Jane",
            LastName = "Smith",
            IsEmailVerified = true,
            GoogleProviderId = "google_123456",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var user = _mapper.Map<User>(userEf);

        // Assert
        user.Should().NotBeNull();
        user.GoogleProviderId.Should().Be("google_123456");
        user.IsEmailVerified.Should().BeTrue();
        user.PasswordHash.Should().BeNull();
    }

    [Fact]
    public void Should_Map_GoogleUser_To_UserEf()
    {
        // Arrange
        var user = UserTestHelper.CreateGoogleUser(
            "google@example.com",
            "google_123456");

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.Should().NotBeNull();
        userEf.GoogleProviderId.Should().Be("google_123456");
        userEf.IsEmailVerified.Should().BeTrue();
        userEf.Email.Should().Be("google@example.com");
        userEf.FirstName.Should().Be("Google");
        userEf.LastName.Should().Be("User");
    }

    [Fact]
    public void Should_Transfer_DomainEvents_When_Mapping_User_To_UserEf()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser();

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.DomainEvents.Should().HaveCount(user.DomainEvents.Count);
    }

    [Fact]
    public void Should_Map_User_With_Null_Optional_Fields_To_UserEf()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser(
            username: "minimaluser",
            email: "minimal@example.com");

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.Should().NotBeNull();
        userEf.Username.Should().Be("minimaluser");
        userEf.FirstName.Should().BeNull();
        userEf.LastName.Should().BeNull();
        userEf.Avatar.Should().BeNull();
        userEf.Bio.Should().BeNull();
        userEf.GoogleProviderId.Should().BeNull();
    }

    [Fact]
    public void Should_Map_UserEf_With_Null_Optional_Fields_To_User()
    {
        // Arrange
        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "minimaluser",
            Email = "minimal@example.com",
            PasswordHash = "hash123",
            FirstName = null,
            LastName = null,
            Avatar = null,
            Bio = null,
            IsEmailVerified = false,
            GoogleProviderId = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var user = _mapper.Map<User>(userEf);

        // Assert
        user.Should().NotBeNull();
        user.FirstName.Should().BeNull();
        user.LastName.Should().BeNull();
        user.Avatar.Should().BeNull();
        user.Bio.Should().BeNull();
        user.GoogleProviderId.Should().BeNull();
    }

    [Fact]
    public void Should_Ignore_Navigation_Properties_When_Mapping_User_To_UserEf()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser();

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.Workouts.Should().BeEmpty();
        userEf.CustomExercises.Should().BeEmpty();
        userEf.WorkoutTemplates.Should().BeEmpty();
        userEf.UserAchievements.Should().BeEmpty();
        userEf.ExerciseRecords.Should().BeEmpty();
    }

    [Fact]
    public void Should_Preserve_CreatedAt_When_Mapping_User_To_UserEf()
    {
        // Arrange
        var originalCreatedAt = DateTime.UtcNow.AddDays(-30);
        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash123",
            IsEmailVerified = false,
            CreatedAt = originalCreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var user = _mapper.Map<User>(userEf);

        // Act
        var mappedBack = _mapper.Map<UserEf>(user);

        // Assert
        mappedBack.CreatedAt.Should().Be(originalCreatedAt);
    }

    [Fact]
    public void Should_Map_Lowercase_Email_Correctly()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser(email: "Test@Example.COM");

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.Email.Should().Be("test@example.com"); // Email normalized to lowercase in domain
    }

    [Fact]
    public void Should_Map_VerifiedUser_Correctly()
    {
        // Arrange
        var user = UserTestHelper.CreateVerifiedUser();

        // Act
        var userEf = _mapper.Map<UserEf>(user);

        // Assert
        userEf.Should().NotBeNull();
        userEf.Username.Should().Be("verifieduser");
        userEf.Email.Should().Be("verified@example.com");
        userEf.FirstName.Should().Be("John");
        userEf.LastName.Should().Be("Doe");
        userEf.Bio.Should().Be("Test bio");
        userEf.Avatar.Should().Be("https://example.com/avatar.jpg");
        userEf.IsEmailVerified.Should().BeTrue();
    }
}
