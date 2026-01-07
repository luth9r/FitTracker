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
    public async Task Update_ShouldUpdateExistingUser()
    {
        // Arrange
        var user = User.Create("testuser", "test@example.com", "hash123");
        var userEf = mapper.Map<UserEf>(user);
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        // Modify user
        var userToUpdate = mapper.Map<User>(userEf);
        userToUpdate.UpdateProfile("UpdatedFirst", "UpdatedLast", "Updated bio", null);

        // Act
        _repository.Update(userToUpdate);
        await context.SaveChangesAsync();

        // Assert
        var updatedUser = await context.Users.FindAsync(userToUpdate.Id);
        updatedUser.Should().NotBeNull();
        updatedUser.FirstName.Should().Be("UpdatedFirst");
        updatedUser.LastName.Should().Be("UpdatedLast");
        updatedUser.Bio.Should().Be("Updated bio");
    }

    [Fact]
    public async Task Update_ShouldNotModifyCreatedAt()
    {
        // Arrange
        var originalCreatedAt = DateTime.UtcNow;
        var user = User.Create("testuser", "test@example.com", "hash123");

        var userEf = mapper.Map<UserEf>(user);
        userEf.CreatedAt = originalCreatedAt;
        context.Users.Add(userEf);
        await context.SaveChangesAsync();

        var originalId = userEf.Id;
        context.ChangeTracker.Clear();

        var userFromDb = await context.Users.FindAsync(originalId);
        context.ChangeTracker.Clear();

        var userToUpdate = mapper.Map<User>(userFromDb);
        userToUpdate.UpdateProfile("NewName", null, null, null);

        // Act
        _repository.Update(userToUpdate);
        await context.SaveChangesAsync();

        // Assert
        var updatedUser = await context.Users.FindAsync(originalId);
        updatedUser.Should().NotBeNull();
        updatedUser.CreatedAt.Should().BeCloseTo(originalCreatedAt, TimeSpan.FromSeconds(1));
        updatedUser.FirstName.Should().Be("NewName");
    }
}
