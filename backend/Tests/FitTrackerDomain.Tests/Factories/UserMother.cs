using System;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;

namespace FitTracker.Domain.Tests.Factories
{
    /// <summary>
    /// Factory for creating User test data.
    /// </summary>
    public static class UserMother
    {
        private const string PASSWORD_HASH = "hashedpassword123";

        /// <summary>
        /// Creates a default user with minimal data.
        /// </summary>
        public static User Default() => User.CreateBuilder()
            .WithUsername("testuser")
            .WithEmail("test@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .Build();

        /// <summary>
        /// Creates a user with specified username.
        /// </summary>
        public static User WithUsername(string username) => User.CreateBuilder()
            .WithUsername(username)
            .WithEmail($"{username}@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .Build();

        /// <summary>
        /// Creates a user with completed profile.
        /// </summary>
        public static User WithCompletedProfile() => User.CreateBuilder()
            .WithUsername("john_doe")
            .WithEmail("john.doe@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithBio("Fitness enthusiast and gym lover")
            .WithAvatar("https://example.com/avatars/john.jpg")
            .Build();

        /// <summary>
        /// Creates a user with metric units (default).
        /// </summary>
        public static User WithMetricUnits() => User.CreateBuilder()
            .WithUsername("metric_user")
            .WithEmail("metric@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithMetricUnits()
            .Build();

        /// <summary>
        /// Creates a user with imperial units.
        /// </summary>
        public static User WithImperialUnits() => User.CreateBuilder()
            .WithUsername("imperial_user")
            .WithEmail("imperial@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithImperialUnits()
            .Build();

        /// <summary>
        /// Creates a user with custom email.
        /// </summary>
        public static User WithEmail(string email) => User.CreateBuilder()
            .WithUsername("testuser")
            .WithEmail(email)
            .WithPasswordHash(PASSWORD_HASH)
            .Build();

        /// <summary>
        /// Creates a user with only first name.
        /// </summary>
        public static User WithFirstNameOnly(string firstName) => User.CreateBuilder()
            .WithUsername("testuser")
            .WithEmail("test@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithFirstName(firstName)
            .Build();

        /// <summary>
        /// Creates a user with only last name.
        /// </summary>
        public static User WithLastNameOnly(string lastName) => User.CreateBuilder()
            .WithUsername("testuser")
            .WithEmail("test@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithLastName(lastName)
            .Build();

        /// <summary>
        /// Creates a user with first and last name.
        /// </summary>
        public static User WithFullName(string firstName, string lastName) => User.CreateBuilder()
            .WithUsername("testuser")
            .WithEmail("test@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithFirstName(firstName)
            .WithLastName(lastName)
            .Build();

        /// <summary>
        /// Creates a user with all optional fields.
        /// </summary>
        public static User WithAllFields() => User.CreateBuilder()
            .WithUsername("testuser")
            .WithEmail("test@example.com")
            .WithPasswordHash(PASSWORD_HASH)
            .WithFirstName("John")
            .WithLastName("Doe")
            .WithAvatar("https://avatar.com/pic.jpg")
            .WithBio("Fitness enthusiast")
            .WithPreferredUnits(UnitSystem.Imperial)
            .Build();
    }
}
