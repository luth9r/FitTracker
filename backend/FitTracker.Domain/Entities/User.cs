using System;
using System.Collections.Generic;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Validators;
using FluentValidation;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a user in the FitTracker application.
    /// </summary>
    public class User : BaseEntity
    {
        #region Constants

        public const int UsernameMaxLength = 50;
        public const int UsernameMinLength = 3;
        public const int EmailMaxLength = 100;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int BioMaxLength = 500;
        public const int AvatarMaxLength = 500;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string Username
        {
            get; private set;
        }

        /// <summary>
        /// Gets the email address of the user.
        /// </summary>
        public string Email
        {
            get; private set;
        }

        /// <summary>
        /// Gets the hashed password of the user.
        /// </summary>
        public string PasswordHash
        {
            get; private set;
        }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string? FirstName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string? LastName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the avatar image URL of the user.
        /// </summary>
        public string? Avatar
        {
            get; private set;
        }

        /// <summary>
        /// Gets the biography of the user.
        /// </summary>
        public string? Bio
        {
            get; private set;
        }

        /// <summary>
        /// Gets the preferred unit system of the user.
        /// </summary>
        public UnitSystem PreferredUnits
        {
            get; private set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor for ORM.
        /// Do not use directly.
        /// </summary>
        private User()
        {
        }

        /// <summary>
        /// Domain constructor used by Builder for creating new users.
        /// Contains business logic, initializes fields, and validates.
        /// </summary>
        private User(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null) : base()
        {
            Username = username;
            Email = email?.ToLowerInvariant();
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            PreferredUnits = UnitSystem.Metric;

            EnsureValid();
        }

        /// <summary>
        /// Constructor for restoring user from persistence layer.
        /// Use <see cref="UserBuilder"/> for creating new users.
        /// </summary>
        public User(
            string username,
            string email,
            string passwordHash,
            string? firstName,
            string? lastName,
            string? avatar,
            string? bio,
            UnitSystem preferredUnits) : this(username, email, passwordHash, firstName, lastName)
        {
            Avatar = avatar;
            Bio = bio;
            PreferredUnits = preferredUnits;

            // No validation here since data is from persistence
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator()
        {
            return new UserValidator();
        }

        #endregion

        #region Builder

        /// <summary>
        /// Creates a new <see cref="UserBuilder"/> instance.
        /// </summary>
        public static UserBuilder CreateBuilder()
        {
            return new UserBuilder();
        }

        /// <summary>
        /// Builder for creating <see cref="User"/> instances.
        /// </summary>
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

            /// <summary>
            /// Builds the <see cref="User"/> entity.
            /// </summary>
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
                    _preferredUnits);
            }
        }

        #endregion

        #region Domain Methods

        /// <summary>
        /// Updates the user profile information.
        /// </summary>
        /// <param name="firstName">New first name</param>
        /// <param name="lastName">New last name</param>
        /// <param name="bio">New biography</param>
        /// <param name="avatar">New avatar URL</param>
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
        /// Updates the user's email address.
        /// </summary>
        /// <param name="email">New email address</param>
        public void UpdateEmail(string email)
        {
            Email = email?.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Updates the password hash.
        /// </summary>
        /// <param name="passwordHash">New password hash</param>
        public void UpdatePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;

            EnsureValid();
        }

        /// <summary>
        /// Updates the preferred unit system.
        /// </summary>
        /// <param name="units">New unit system</param>
        /// <exception cref="ArgumentNullException">If units is null</exception>
        public void UpdatePreferredUnits(UnitSystem units)
        {
            PreferredUnits = units ?? throw new ArgumentNullException(nameof(units));
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Sets preferred units to metric system.
        /// </summary>
        public void SetMetricUnits()
        {
            UpdatePreferredUnits(UnitSystem.Metric);
        }

        /// <summary>
        /// Sets preferred units to imperial system.
        /// </summary>
        public void SetImperialUnits()
        {
            UpdatePreferredUnits(UnitSystem.Imperial);
        }

        /// <summary>
        /// Converts a weight value from current preferred units to specified target units.
        /// </summary>
        /// <param name="weight">Weight value to convert</param>
        /// <param name="targetSystem">Target unit system</param>
        /// <returns>Converted weight value</returns>
        public decimal ConvertWeight(decimal weight, UnitSystem targetSystem)
        {
            return PreferredUnits.ConvertWeight(weight, targetSystem);
        }

        /// <summary>
        /// Gets the unit string for weight in the current preferred unit system.
        /// </summary>
        /// <returns>Unit string for weight</returns>
        public string GetWeightUnit()
        {
            return PreferredUnits.WeightUnit;
        }

        /// <summary>
        /// Gets the unit string for length in the current preferred unit system.
        /// </summary>
        /// <returns>Unit string for length</returns>
        public string GetLengthUnit()
        {
            return PreferredUnits.LengthUnit;
        }

        /// <summary>
        /// Gets the full name of the user, constructed from first and last names if available.
        /// </summary>
        /// <returns>Full name or username if names are unavailable</returns>
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
        /// Gets the display name for the user.
        /// </summary>
        /// <returns>Full name if available, otherwise username</returns>
        public string GetDisplayName()
        {
            return GetFullName();
        }

        /// <summary>
        /// Determines if the user has completed their profile.
        /// </summary>
        /// <returns>True if first name, last name, and bio are filled; otherwise false</returns>
        public bool HasCompletedProfile()
        {
            return !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(Bio);
        }

        /// <summary>
        /// Determines if the user prefers the metric unit system.
        /// </summary>
        /// <returns>True if metric units are preferred; otherwise false</returns>
        public bool UsesMetric()
        {
            return PreferredUnits == UnitSystem.Metric;
        }

        /// <summary>
        /// Determines if the user prefers the imperial unit system.
        /// </summary>
        /// <returns>True if imperial units are preferred; otherwise false</returns>
        public bool UsesImperial()
        {
            return PreferredUnits == UnitSystem.Imperial;
        }

        #endregion
    }
}
