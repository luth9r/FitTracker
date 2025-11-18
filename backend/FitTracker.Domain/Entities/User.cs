using CSharpFunctionalExtensions;
using FitTracker.Domain.Validators;
using FitTracker.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;

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

        public string Username { get; private set; }
        public string Email { get; private set; }
        public string? PasswordHash { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Avatar { get; private set; }
        public string? Bio { get; private set; }
        public UnitSystem PreferredUnits { get; private set; }

        public bool IsEmailVerified { get; private set; }

        public string? GoogleProviderId { get; private set; }

        #endregion

        #region Constructors

        private User()
        {
            // Parameterless constructor for ORM only
        }

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
            IsEmailVerified = false;
        }

        /// <summary>
        /// Constructor for restoring user from persistence layer.
        /// Use factory methods or builder for creating new users.
        /// </summary>
        public User(
            string username,
            string email,
            string passwordHash,
            string? firstName,
            string? lastName,
            string? avatar,
            string? bio,
            UnitSystem preferredUnits,
            bool isEmailVerified = false,
            string? googleProvidedId = null) : this(username, email, passwordHash, firstName, lastName)
        {
            Avatar = avatar;
            Bio = bio;
            PreferredUnits = preferredUnits;
            IsEmailVerified = isEmailVerified;
            GoogleProviderId = googleProvidedId;
        }

        #endregion

        #region Factory method with validation

        public static Result<User, ValidationResult> Create(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null)
        {
            var user = new User(username, email, passwordHash, firstName, lastName);

            return user.ValidateWithResult();
        }

        #endregion

        #region Validation

        protected override IValidator GetValidator() => new UserValidator();

        public ValidationResult Validate()
        {
            var validator = GetValidator();
            return validator.Validate(new ValidationContext<object>(this));
        }

        private Result<User, ValidationResult> ValidateWithResult()
        {
            var result = Validate();
            if (!result.IsValid)
                return Result.Failure<User, ValidationResult>(result);

            return Result.Success<User, ValidationResult>(this);
        }

        public void SetEmailVerified() => this.IsEmailVerified = true;

        #endregion

        #region Builder

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
            private string? _googleProviderId;

            public UserBuilder WithUsername(string username) { _username = username; return this; }

            public UserBuilder WithUniqueUsername()
            {
                _username = $"user_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
                return this;
            }
            public UserBuilder WithEmail(string email) { _email = email; return this; }
            public UserBuilder WithPasswordHash(string passwordHash) { _passwordHash = passwordHash; return this; }
            public UserBuilder WithFirstName(string? firstName) { _firstName = firstName; return this; }
            public UserBuilder WithLastName(string? lastName) { _lastName = lastName; return this; }
            public UserBuilder WithAvatar(string? avatar) { _avatar = avatar; return this; }
            public UserBuilder WithBio(string? bio) { _bio = bio; return this; }
            public UserBuilder WithPreferredUnits(UnitSystem preferredUnits)
            {
                _preferredUnits = preferredUnits;
                return this;
            }
            public UserBuilder WithMetricUnits() { _preferredUnits = UnitSystem.Metric; return this; }
            public UserBuilder WithImperialUnits() { _preferredUnits = UnitSystem.Imperial; return this; }

            public UserBuilder WithGoogleProvidedId(string? googleProviderId) { _googleProviderId = googleProviderId; return this; }

            public Result<User, ValidationResult> Build()
            {
                var user = new User(
                    _username,
                    _email,
                    _passwordHash,
                    _firstName,
                    _lastName,
                    _avatar,
                    _bio,
                    _preferredUnits,
                    googleProvidedId: _googleProviderId);
                var validationResult = user.Validate();
                if (!validationResult.IsValid)
                    return Result.Failure<User, ValidationResult>(validationResult);

                return Result.Success<User, ValidationResult>(user);
            }
        }

        #endregion

        #region Domain Methods

        public Result<User, ValidationResult> UpdateProfile(string? firstName, string? lastName, string? bio, string? avatar)
        {
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            Avatar = avatar;
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public Result<User, ValidationResult> UpdateEmail(string email)
        {
            Email = email?.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public Result<User, ValidationResult> UpdatePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
            return ValidateWithResult();
        }

        public void UpdatePreferredUnits(UnitSystem units)
        {
            PreferredUnits = units;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetMetricUnits() => UpdatePreferredUnits(UnitSystem.Metric);
        public void SetImperialUnits() => UpdatePreferredUnits(UnitSystem.Imperial);

        public decimal ConvertWeight(decimal weight, UnitSystem targetSystem) => PreferredUnits.ConvertWeight(weight, targetSystem);
        public string GetWeightUnit() => PreferredUnits.WeightUnit;
        public string GetLengthUnit() => PreferredUnits.LengthUnit;

        public void SetGoogleProviderId(string googleProviderId)
        {
            GoogleProviderId = googleProviderId;
        }

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

        public string GetDisplayName() => GetFullName();

        public bool HasCompletedProfile() =>
            !string.IsNullOrWhiteSpace(FirstName)
            && !string.IsNullOrWhiteSpace(LastName)
            && !string.IsNullOrWhiteSpace(Bio);

        public bool UsesMetric() => PreferredUnits == UnitSystem.Metric;
        public bool UsesImperial() => PreferredUnits == UnitSystem.Imperial;

        #endregion
    }
}
