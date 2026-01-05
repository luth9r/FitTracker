using AutoMapper;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FitTrackerInfrastructure.Tests.Repositories;

public sealed class UserReadRepositoryTests
{
    private readonly Mock<IMapper> _mapperMock = new();

    private static FitTrackerDbContext BuildContext()
    {
        var options = new DbContextOptionsBuilder<FitTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FitTrackerDbContext(options);
    }

    private static IMapper BuildMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEf, User>(), NullLoggerFactory.Instance);

        return config.CreateMapper();
    }

    [Fact]
    public async Task GetByIdReadonlyAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        var userId = Guid.NewGuid();
        var userEf = new UserEf
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = "hash123",
            FirstName = "John",
            LastName = "Doe",
            IsEmailVerified = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdReadonlyAsync(userId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Username.Should().Be("testuser");
        result.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task GetByIdReadonlyAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdReadonlyAsync(nonExistentId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUsernameReadonlyAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "johndoe",
            Email = "john@example.com",
            PasswordHash = "hash123",
            IsEmailVerified = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByUsernameReadonlyAsync("johndoe", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("johndoe");
        result.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task GetByUsernameReadonlyAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        // Act
        var result = await repository.GetByUsernameReadonlyAsync("nonexistent", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailReadonlyAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "unique@example.com",
            PasswordHash = "hash123",
            IsEmailVerified = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByEmailReadonlyAsync("unique@example.com", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("unique@example.com");
        result.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task GetByEmailReadonlyAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        // Act
        var result = await repository.GetByEmailReadonlyAsync("nonexistent@example.com", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByGoogleTokenReadonlyAsync_WhenUserExists_ShouldReturnUser()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        var googleToken = "google_123456789";
        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "googleuser",
            Email = "google@example.com",
            GoogleProviderId = googleToken,
            IsEmailVerified = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByGoogleTokenReadonlyAsync(googleToken, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.GoogleProviderId.Should().Be(googleToken);
        result.Email.Should().Be("google@example.com");
    }

    [Fact]
    public async Task GetByGoogleTokenReadonlyAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        // Act
        var result = await repository.GetByGoogleTokenReadonlyAsync("nonexistent_token", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUsernameReadonlyAsync_WithMultipleUsers_ShouldReturnCorrectUser()
    {
        // Arrange
        await using var context = BuildContext();
        var mapper = BuildMapper();
        var repository = new UserReadRepository(context, mapper);

        var users = new[]
        {
            new UserEf
            {
                Id = Guid.NewGuid(),
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = "hash1",
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new UserEf
            {
                Id = Guid.NewGuid(),
                Username = "user2",
                Email = "user2@example.com",
                PasswordHash = "hash2",
                IsEmailVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
            new UserEf
            {
                Id = Guid.NewGuid(),
                Username = "user3",
                Email = "user3@example.com",
                PasswordHash = "hash3",
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            },
        };
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByUsernameReadonlyAsync("user2", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be("user2");
        result.Email.Should().Be("user2@example.com");
        result.IsEmailVerified.Should().BeTrue();
    }
}
