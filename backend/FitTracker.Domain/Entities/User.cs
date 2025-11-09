// FitTracker.Domain/Entities/User.cs
using System;
using System.Collections.Generic;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Validators;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a user in the FitTracker application
    /// </summary>
    public class User : BaseEntity
    {
        // ============================================
        // Constants
        // ============================================
        public const int UsernameMaxLength = 50;
        public const int UsernameMinLength = 3;
        public const int EmailMaxLength = 100;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int BioMaxLength = 500;
        public const int AvatarMaxLength = 500;

        // ============================================
        // Properties
        // ============================================
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Avatar { get; private set; }
        public string? Bio { get; private set; }
        public UnitSystem PreferredUnits { get; private set; }

        // ============================================
        // Constructors
        // ============================================

        /// <summary>
        /// Domain constructor
        /// </summary>
        private User(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null) : base()
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

            Username = username;
            Email = email.ToLowerInvariant();
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            PreferredUnits = UnitSystem.Metric;

            EnsureValid();
        }

        public User(string username, string email, string passwordHash, string? firstName, string? lastName, string? avatar, string? bio, UnitSystem preferredUnits) : this(username, email, passwordHash, firstName, lastName)
        {
            Avatar = avatar;
            Bio = bio;
            PreferredUnits = preferredUnits;
        }



        // ============================================
        // Validator
        // ============================================
        protected override IValidator GetValidator()
        {
            return new UserValidator();
        }

        // ============================================
        // Builder Pattern
        // ============================================

        /// <summary>
        /// Creates a new builder for User
        /// </summary>
        public static UserBuilder CreateBuilder() => new UserBuilder();

        public class UserBuilder
        {
            private string _username = string.Empty;
            private string _email = string.Empty;
            private string _passwordHash = string.Empty;
            private string? _firstName;
            private string? _lastName;
            private string? _avatar;
            private string? _bio;
            private UnitSystem _preferredUnits = UnitSystem.Metric;

            public UserBuilder WithUsername(string username)
            {
                _username = username;
                return this;
            }

            public UserBuilder WithEmail(string email)
            {
                _email = email;
                return this;
            }

            public UserBuilder WithPasswordHash(string passwordHash)
            {
                _passwordHash = passwordHash;
                return this;
            }

            public UserBuilder WithFirstName(string? firstName)
            {
                _firstName = firstName;
                return this;
            }

            public UserBuilder WithLastName(string? lastName)
            {
                _lastName = lastName;
                return this;
            }

            public UserBuilder WithAvatar(string? avatar)
            {
                _avatar = avatar;
                return this;
            }

            public UserBuilder WithBio(string? bio)
            {
                _bio = bio;
                return this;
            }


            public UserBuilder WithPreferredUnits(UnitSystem preferredUnits)
            {
                _preferredUnits = preferredUnits ?? throw new ArgumentNullException(nameof(preferredUnits));
                return this;
            }

            public UserBuilder WithMetricUnits()
            {
                _preferredUnits = UnitSystem.Metric;
                return this;
            }

            public UserBuilder WithImperialUnits()
            {
                _preferredUnits = UnitSystem.Imperial;
                return this;
            }

            public User Build()
            {
                return new User(
                    _username,
                    _email,
                    _passwordHash,
                    _firstName,
                    _lastName,
                    _avatar,
                    _bio,
                    _preferredUnits
                );
            }
        }

        // ============================================
        // Domain Methods
        // ============================================

        /// <summary>
        /// Update user profile information
        /// </summary>
        public void UpdateProfile(string? firstName, string? lastName, string? bio, string? avatar)
        {
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            Avatar = avatar;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Update email address
        /// </summary>
        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Update password hash
        /// </summary>
        public void UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Change preferred unit system
        /// </summary>
        public void UpdatePreferredUnits(UnitSystem units)
        {
            PreferredUnits = units ?? throw new ArgumentNullException(nameof(units));
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Set to metric units
        /// </summary>
        public void SetMetricUnits() => UpdatePreferredUnits(UnitSystem.Metric);

        /// <summary>
        /// Set to imperial units
        /// </summary>
        public void SetImperialUnits() => UpdatePreferredUnits(UnitSystem.Imperial);


        /// <summary>
        /// Convert weight between unit systems
        /// </summary>
        public decimal ConvertWeight(decimal weight, UnitSystem targetSystem)
        {
            return PreferredUnits.ConvertWeight(weight, targetSystem);
        }

        /// <summary>
        /// Get current weight unit
        /// </summary>
        public string GetWeightUnit() => PreferredUnits.WeightUnit;

        /// <summary>
        /// Get current length unit
        /// </summary>
        public string GetLengthUnit() => PreferredUnits.LengthUnit;

        /// <summary>
        /// Get full name of user
        /// </summary>
        public string GetFullName()
        {
            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                return $"{FirstName} {LastName}";

            if (!string.IsNullOrWhiteSpace(FirstName))
                return FirstName;

            if (!string.IsNullOrWhiteSpace(LastName))
                return LastName;

            return Username;
        }

        /// <summary>
        /// Get display name (full name or username)
        /// </summary>
        public string GetDisplayName()
        {
            return GetFullName();
        }

        /// <summary>
        /// Check if profile is completed
        /// </summary>
        public bool HasCompletedProfile()
        {
            return !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(Bio);
        }

        /// <summary>
        /// Check if user uses metric system
        /// </summary>
        public bool UsesMetric() => PreferredUnits == UnitSystem.Metric;

        /// <summary>
        /// Check if user uses imperial system
        /// </summary>
        public bool UsesImperial() => PreferredUnits == UnitSystem.Imperial;
    }
}
