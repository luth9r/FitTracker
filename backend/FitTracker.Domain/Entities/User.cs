using FitTracker.Domain.ValueObjects;

namespace FitTracker.Domain.Entities
{
    /// <summary>
    /// Represents a user in the FitTracker application.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// The maximum length allowed for the username.
        /// </summary>
        public const int UsernameMaxLength = 50;
        /// <summary>
        /// The minimum length required for the username.
        /// </summary>
        public const int UsernameMinLength = 3;
        /// <summary>
        /// The maximum length allowed for the email address.
        /// </summary>
        public const int EmailMaxLength = 100;
        /// <summary>
        /// The maximum length allowed for the first name.
        /// </summary>
        public const int FirstNameMaxLength = 50;
        /// <summary>
        /// The maximum length allowed for the last name.
        /// </summary>
        public const int LastNameMaxLength = 50;
        /// <summary>
        /// The maximum length allowed for the biography.
        /// </summary>
        public const int BioMaxLength = 500;
        /// <summary>
        /// The maximum length allowed for the avatar URL.
        /// </summary>
        public const int AvatarMaxLength = 500;

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the email address of the user.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the hashed password of the user.
        /// </summary>
        public string? PasswordHash { get; private set; }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string? FirstName { get; private set; }

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string? LastName { get; private set; }

        /// <summary>
        /// Gets the URL of the user's avatar.
        /// </summary>
        public string? Avatar { get; private set; }

        /// <summary>
        /// Gets the biography or bio of the user.
        /// </summary>
        public string? Bio { get; private set; }

        /// <summary>
        /// Gets the user's preferred unit system (Metric or Imperial).
        /// </summary>
        public UnitSystem PreferredUnits { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user's email address has been verified.
        /// </summary>
        public bool IsEmailVerified { get; private set; }

        /// <summary>
        /// Gets the Google provider ID if linked.
        /// </summary>
        public string? GoogleProviderId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="email">The email address.</param>
        /// <param name="passwordHash">The hashed password.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="avatar">The avatar URL.</param>
        /// <param name="bio">The biography.</param>
        /// <param name="preferredUnits">The preferred unit system.</param>
        /// <param name="isEmailVerified">Whether the email is verified.</param>
        /// <param name="googleProviderId">The Google provider ID.</param>
        /// <param name="createdAt">The date and time of creation.</param>
        /// <param name="updatedAt">The date and time of the last update.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        private User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email address.</param>
        /// <param name="passwordHash">The hashed password.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        private User(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null)
            : base()
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

        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email address.</param>
        /// <param name="passwordHash">The hashed password.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>A new instance of <see cref="User"/>.</returns>
        public static User Create(
            string username,
            string email,
            string passwordHash,
            string? firstName = null,
            string? lastName = null)
        {
            return new User(username, email, passwordHash, firstName, lastName);
        }

        /// <summary>
        /// Creates a new <see cref="User"/> from Google authentication.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="googleProviderId">The Google provider ID.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>A new instance of <see cref="User"/>.</returns>
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

        /// <summary>
        /// Updates the user's profile information.
        /// </summary>
        /// <param name="firstName">The new first name.</param>
        /// <param name="lastName">The new last name.</param>
        /// <param name="bio">The new biography.</param>
        /// <param name="avatar">The new avatar URL.</param>
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

        /// <summary>
        /// Updates the user's email address.
        /// </summary>
        /// <param name="email">The new email address.</param>
        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || email.Length > EmailMaxLength)
            {
                throw new ArgumentException($"Email cannot exceed {EmailMaxLength} characters", nameof(email));
            }

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the user's password hash.
        /// </summary>
        /// <param name="passwordHash">The new password hash.</param>
        public void UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash is required", nameof(passwordHash));
            }

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the user's preferred units.
        /// </summary>
        /// <param name="units">The new preferred unit system.</param>
        public void UpdatePreferredUnits(UnitSystem units)
        {
            PreferredUnits = units;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the user's email as verified.
        /// </summary>
        public void SetEmailVerified()
        {
            IsEmailVerified = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Sets the Google provider ID for the user.
        /// </summary>
        /// <param name="googleProviderId">The Google provider ID.</param>
        public void SetGoogleProviderId(string googleProviderId)
        {
            if (string.IsNullOrWhiteSpace(googleProviderId))
            {
                throw new ArgumentException("Google provider ID is required", nameof(googleProviderId));
            }

            GoogleProviderId = googleProviderId;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the full name of the user, falling back to username if names are not set.
        /// </summary>
        /// <returns>The full name or username.</returns>
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

        /// <summary>
        /// Determines whether the user uses metric units.
        /// </summary>
        /// <returns><c>true</c> if metric; otherwise, <c>false</c>.</returns>
        public bool UsesMetric() => PreferredUnits == UnitSystem.Metric;

        /// <summary>
        /// Determines whether the user uses imperial units.
        /// </summary>
        /// <returns><c>true</c> if imperial; otherwise, <c>false</c>.</returns>
        public bool UsesImperial() => PreferredUnits == UnitSystem.Imperial;

        /// <summary>
        /// Gets the display name of the user (alias for GetFullName).
        /// </summary>
        public string DisplayName => GetFullName();
    }
}
