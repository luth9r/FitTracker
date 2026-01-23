using AutoMapper;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTracker.Infrastructure.Automapper;
using FitTracker.Infrastructure.Persistence.Data;
using FitTracker.Infrastructure.Persistence.Data.Entities;
using FitTracker.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FitTrackerInfrastructure.Tests.Repositories;

public sealed class UserWriteRepositoryTests : RepositoryTestBase
{
    private readonly IUserWriteRepository _repository;

    public UserWriteRepositoryTests()
    {
        _repository = new UserWriteRepository(context, mapper);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToContext()
    {
        // Arrange
        var user = User.Create(
            "newuser",
            "newuser@example.com",
            "hash123",
            "John",
            "Doe");

        // Act
        await _repository.AddAsync(user, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
        savedUser.Should().NotBeNull();
        savedUser.Email.Should().Be("newuser@example.com");
        savedUser.FirstName.Should().Be("John");
        savedUser.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task AddAsync_ShouldAddMultipleUsers()
    {
        // Arrange
        var user1 = User.Create("user1", "user1@example.com", "hash1");
        var user2 = User.Create("user2", "user2@example.com", "hash2");

        // Act
        await _repository.AddAsync(user1, CancellationToken.None);
        await _repository.AddAsync(user2, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var users = await context.Users.ToListAsync();
        users.Should().HaveCount(2);
        users.Select(u => u.Username).Should().BeEquivalentTo("user1", "user2");
    }

    [Fact]
    public async Task AddAsync_GoogleUser_ShouldAddUserWithGoogleProviderId()
    {
        // Arrange
        var googleUser = User.CreateGoogleUser(
            "google@example.com",
            "google_123456",
            "Jane",
            "Smith");

        // Act
        await _repository.AddAsync(googleUser, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "google@example.com");
        savedUser.Should().NotBeNull();
        savedUser.GoogleProviderId.Should().Be("google_123456");
        savedUser.IsEmailVerified.Should().BeTrue();
        savedUser.FirstName.Should().Be("Jane");
        savedUser.LastName.Should().Be("Smith");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDomainEntity_WhenUserExists()
    {
        // Arrange
        var userEf = new UserEf
        {
            Id = Guid.NewGuid(),
            Username = "exists",
            Email = "exists@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        // Act
        var result = await _repository.FindByIdAsync(userEf.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userEf.Id);
        result.Username.Should().Be("exists");

        context.Entry(userEf).State.Should().Be(EntityState.Unchanged);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenUserIsLoadedFromRepo()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalUser = new UserEf
        {
            Id = userId,
            Username = "bob",
            Email = "bob@test.com",
            FirstName = "Bob",
            LastName = "Old",
            PasswordHash = "hash"
        };
        context.Users.Add(originalUser);
        await context.SaveChangesAsync();

        // Act
        var domainUser = await _repository.FindByIdAsync(userId, CancellationToken.None);

        domainUser!.UpdateProfile("Bob", "NewLastName", "New Bio", null);

        await _repository.UpdateAsync(domainUser, CancellationToken.None);

        await context.SaveChangesAsync();

        // Assert
        context.ChangeTracker.Clear();
        var updatedUserEf = await context.Users.FindAsync(userId);

        updatedUserEf.Should().NotBeNull();
        updatedUserEf!.LastName.Should().Be("NewLastName");
        updatedUserEf.Bio.Should().Be("New Bio");
        updatedUserEf.FirstName.Should().Be("Bob");
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotModifyCreatedAt()
    {
        // Arrange
        var oldDate = DateTime.UtcNow.AddYears(-1);
        var userId = Guid.NewGuid();

        var userEf = new UserEf
        {
            Id = userId,
            Username = "time",
            Email = "time@test.com",
            PasswordHash = "h",
            CreatedAt = oldDate
        };
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        // Act
        var domainUser = await _repository.FindByIdAsync(userId, CancellationToken.None);

        domainUser!.UpdateProfile("Name", "Last", null, null);

        await _repository.UpdateAsync(domainUser, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        context.ChangeTracker.Clear();
        var result = await context.Users.FindAsync(userId);

        result!.CreatedAt.Should().BeCloseTo(oldDate, TimeSpan.FromMilliseconds(100));
    }
}
