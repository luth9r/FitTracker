using FitTracker.Domain.ValueObjects;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a user in the FitTracker application.
    /// </summary>
    public class User : BaseEntity
    {
        public const int UsernameMaxLength = 50;
        public const int UsernameMinLength = 3;
        public const int EmailMaxLength = 100;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int BioMaxLength = 500;
        public const int AvatarMaxLength = 500;

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

        internal User(
            Guid id,
            string username,
            string email,
            string? passwordHash,
            string? firstName,
            string? lastName,
            string? avatar,
            string? bio,
            UnitSystem preferredUnits,
            bool isEmailVerified,
            string? googleProviderId,
            DateTime createdAt,
            DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            Avatar = avatar;
            Bio = bio;
            PreferredUnits = preferredUnits;
            IsEmailVerified = isEmailVerified;
            GoogleProviderId = googleProviderId;
        }

        private User()
        {
        }

        private User(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < UsernameMinLength || username.Length > UsernameMaxLength)
            {
                throw new ArgumentException($"Username must be {UsernameMinLength}-{UsernameMaxLength} characters", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(email) || email.Length > EmailMaxLength)
            {
                throw new ArgumentException($"Email cannot exceed {EmailMaxLength} characters", nameof(email));
            }

            if (firstName?.Length > FirstNameMaxLength)
            {
                throw new ArgumentException($"First name cannot exceed {FirstNameMaxLength} characters", nameof(firstName));
            }

            if (lastName?.Length > LastNameMaxLength)
            {
                throw new ArgumentException($"Last name cannot exceed {LastNameMaxLength} characters", nameof(lastName));
            }

            Username = username;
            Email = email.ToLowerInvariant();
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            PreferredUnits = UnitSystem.Metric;
            IsEmailVerified = false;
        }

        public static User Create(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null)
        {
            return new User(username, email, passwordHash, firstName, lastName);
        }

        public static User CreateGoogleUser(
            string email,
            string googleProviderId,
            string? firstName = null,
            string? lastName = null)
        {
            var user = Create(
                username: $"google_{Guid.NewGuid():N}",
                email: email,
                passwordHash: string.Empty);

            user.SetGoogleProviderId(googleProviderId);
            user.UpdateProfile(firstName, lastName, null, null);

            return user;
        }

        public void UpdateProfile(string? firstName, string? lastName, string? bio, string? avatar)
        {
            if (firstName?.Length > FirstNameMaxLength)
            {
                throw new ArgumentException($"First name cannot exceed {FirstNameMaxLength} characters", nameof(firstName));
            }

            if (lastName?.Length > LastNameMaxLength)
            {
                throw new ArgumentException($"Last name cannot exceed {LastNameMaxLength} characters", nameof(lastName));
            }

            if (bio?.Length > BioMaxLength)
            {
                throw new ArgumentException($"Bio cannot exceed {BioMaxLength} characters", nameof(bio));
            }

            if (avatar?.Length > AvatarMaxLength)
            {
                throw new ArgumentException($"Avatar URL cannot exceed {AvatarMaxLength} characters", nameof(avatar));
            }

            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            Avatar = avatar;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > EmailMaxLength)
            {
                throw new ArgumentException($"Email cannot exceed {EmailMaxLength} characters", nameof(email));
            }

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash is required", nameof(passwordHash));
            }

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePreferredUnits(UnitSystem units)
        {
            PreferredUnits = units;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetEmailVerified()
        {
            IsEmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetGoogleProviderId(string googleProviderId)
        {
            if (string.IsNullOrWhiteSpace(googleProviderId))
            {
                throw new ArgumentException("Google provider ID is required", nameof(googleProviderId));
            }

            GoogleProviderId = googleProviderId;
            UpdatedAt = DateTime.UtcNow;
        }

        public string GetFullName()
        {
            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                return $"{FirstName} {LastName}";
            }

            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                return FirstName;
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                return LastName;
            }

            return Username;
        }

        public bool UsesMetric() => PreferredUnits == UnitSystem.Metric;

        public bool UsesImperial() => PreferredUnits == UnitSystem.Imperial;

        public string DisplayName => GetFullName();
    }
}
