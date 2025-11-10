using System;
using System.Threading;
using Xunit;
using FitTracker.Domain.Entities;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Tests.Factories;

namespace FitTracker.Domain.Tests.Entities
{
    public class UserTests
    {
        private const string PASSWORD_HASH = "hashedpassword123";

        #region Builder Tests

        [Fact]
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            // Act
            var builder = User.CreateBuilder();

            // Assert
            Assert.NotNull(builder);
            Assert.IsType<User.UserBuilder>(builder);
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateUser()
        {
            // Arrange & Act
            var user = UserMother.Default();

            // Assert
            Assert.NotNull(user);
            Assert.Equal("testuser", user.Username);
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal(PASSWORD_HASH, user.PasswordHash);
            Assert.Equal(UnitSystem.Metric, user.PreferredUnits);
        }

        [Fact]
        public void Build_WithMetricUnits_ShouldSetMetricSystem()
        {
            // Arrange & Act
            var user = UserMother.WithMetricUnits();

            // Assert
            Assert.Equal(UnitSystem.Metric, user.PreferredUnits);
            Assert.True(user.UsesMetric());
            Assert.False(user.UsesImperial());
        }

        [Fact]
        public void Build_WithImperialUnits_ShouldSetImperialSystem()
        {
            // Arrange & Act
            var user = UserMother.WithImperialUnits();

            // Assert
            Assert.Equal(UnitSystem.Imperial, user.PreferredUnits);
            Assert.False(user.UsesMetric());
            Assert.True(user.UsesImperial());
        }

        [Fact]
        public void Build_WithOptionalFields_ShouldCreateUserWithAllFields()
        {
            // Arrange & Act
            var user = UserMother.WithAllFields();

            // Assert
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("https://avatar.com/pic.jpg", user.Avatar);
            Assert.Equal("Fitness enthusiast", user.Bio);
            Assert.Equal(UnitSystem.Imperial, user.PreferredUnits);
        }

        [Theory]
        [InlineData("", "test@example.com", PASSWORD_HASH)]
        [InlineData(null, "test@example.com", PASSWORD_HASH)]
        [InlineData("  ", "test@example.com", PASSWORD_HASH)]
        public void Build_WithInvalidUsername_ShouldThrowArgumentException(
            string username, string email, string passwordHash)
        {
            // Arrange
            var builder = User.CreateBuilder()
                .WithUsername(username)
                .WithEmail(email)
                .WithPasswordHash(passwordHash);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => builder.Build());
            Assert.Contains("Username", exception.Message);
        }

        [Theory]
        [InlineData("testuser", "", PASSWORD_HASH)]
        [InlineData("testuser", null, PASSWORD_HASH)]
        [InlineData("testuser", "  ", PASSWORD_HASH)]
        public void Build_WithInvalidEmail_ShouldThrowArgumentException(
            string username, string email, string passwordHash)
        {
            // Arrange
            var builder = User.CreateBuilder()
                .WithUsername(username)
                .WithEmail(email)
                .WithPasswordHash(passwordHash);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => builder.Build());
            Assert.Contains("Email", exception.Message);
        }

        [Theory]
        [InlineData("testuser", "test@example.com", "")]
        [InlineData("testuser", "test@example.com", null)]
        [InlineData("testuser", "test@example.com", "  ")]
        public void Build_WithInvalidPasswordHash_ShouldThrowArgumentException(
            string username, string email, string passwordHash)
        {
            // Arrange
            var builder = User.CreateBuilder()
                .WithUsername(username)
                .WithEmail(email)
                .WithPasswordHash(passwordHash);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => builder.Build());
            Assert.Contains("Password", exception.Message);
        }

        [Fact]
        public void Build_EmailShouldBeLowerCase()
        {
            // Arrange & Act
            var user = UserMother.WithEmail("TEST@EXAMPLE.COM");

            // Assert
            Assert.Equal("test@example.com", user.Email);
        }

        #endregion

        #region Update Profile Tests

        [Fact]
        public void UpdateProfile_WithValidData_ShouldUpdateFields()
        {
            // Arrange
            var user = UserMother.Default();
            var originalUpdatedAt = user.UpdatedAt;

            // Act
            Thread.Sleep(10); // Ensure time passes
            user.UpdateProfile("Jane", "Smith", "New bio", "https://newavatar.com/pic.jpg");

            // Assert
            Assert.Equal("Jane", user.FirstName);
            Assert.Equal("Smith", user.LastName);
            Assert.Equal("New bio", user.Bio);
            Assert.Equal("https://newavatar.com/pic.jpg", user.Avatar);
            Assert.True(user.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void UpdateProfile_WithNullValues_ShouldAcceptNulls()
        {
            // Arrange
            var user = UserMother.WithCompletedProfile();

            // Act
            user.UpdateProfile(null, null, null, null);

            // Assert
            Assert.Null(user.FirstName);
            Assert.Null(user.LastName);
            Assert.Null(user.Bio);
            Assert.Null(user.Avatar);
        }

        #endregion

        #region Update Email Tests

        [Fact]
        public void UpdateEmail_WithValidEmail_ShouldUpdateEmail()
        {
            // Arrange
            var user = UserMother.Default();
            var originalUpdatedAt = user.UpdatedAt;

            // Act
            Thread.Sleep(10);
            user.UpdateEmail("newemail@example.com");

            // Assert
            Assert.Equal("newemail@example.com", user.Email);
            Assert.True(user.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void UpdateEmail_ShouldConvertToLowerCase()
        {
            // Arrange
            var user = UserMother.Default();

            // Act
            user.UpdateEmail("NEWEMAIL@EXAMPLE.COM");

            // Assert
            Assert.Equal("newemail@example.com", user.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void UpdateEmail_WithInvalidEmail_ShouldThrowArgumentException(string email)
        {
            // Arrange
            var user = UserMother.Default();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => user.UpdateEmail(email));
            Assert.Contains("Email", exception.Message);
        }

        #endregion

        #region Update Password Tests

        [Fact]
        public void UpdatePasswordHash_WithValidHash_ShouldUpdatePassword()
        {
            // Arrange
            var user = UserMother.Default();
            var originalUpdatedAt = user.UpdatedAt;

            // Act
            Thread.Sleep(10);
            user.UpdatePasswordHash("newhash123");

            // Assert
            Assert.Equal("newhash123", user.PasswordHash);
            Assert.True(user.UpdatedAt > originalUpdatedAt);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void UpdatePasswordHash_WithInvalidHash_ShouldThrowArgumentException(string hash)
        {
            // Arrange
            var user = UserMother.Default();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => user.UpdatePasswordHash(hash));
            Assert.Contains("Password", exception.Message);
        }

        #endregion

        #region Preferred Units Tests

        [Fact]
        public void UpdatePreferredUnits_WithValidUnit_ShouldUpdateUnits()
        {
            // Arrange
            var user = UserMother.Default();
            var originalUpdatedAt = user.UpdatedAt;

            // Act
            Thread.Sleep(10);
            user.UpdatePreferredUnits(UnitSystem.Imperial);

            // Assert
            Assert.Equal(UnitSystem.Imperial, user.PreferredUnits);
            Assert.True(user.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void SetMetricUnits_ShouldSetToMetric()
        {
            // Arrange
            var user = UserMother.WithImperialUnits();

            // Act
            user.SetMetricUnits();

            // Assert
            Assert.Equal(UnitSystem.Metric, user.PreferredUnits);
            Assert.True(user.UsesMetric());
        }

        [Fact]
        public void SetImperialUnits_ShouldSetToImperial()
        {
            // Arrange
            var user = UserMother.WithMetricUnits();

            // Act
            user.SetImperialUnits();

            // Assert
            Assert.Equal(UnitSystem.Imperial, user.PreferredUnits);
            Assert.True(user.UsesImperial());
        }

        [Fact]
        public void UpdatePreferredUnits_WithNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = UserMother.Default();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => user.UpdatePreferredUnits(null));
        }

        #endregion

        #region Display Name Tests

        [Fact]
        public void GetFullName_WithFirstAndLastName_ShouldReturnFullName()
        {
            // Arrange
            var user = UserMother.WithFullName("John", "Doe");

            // Act
            var fullName = user.GetFullName();

            // Assert
            Assert.Equal("John Doe", fullName);
        }

        [Fact]
        public void GetFullName_WithOnlyFirstName_ShouldReturnFirstName()
        {
            // Arrange
            var user = UserMother.WithFirstNameOnly("John");

            // Act
            var fullName = user.GetFullName();

            // Assert
            Assert.Equal("John", fullName);
        }

        [Fact]
        public void GetFullName_WithOnlyLastName_ShouldReturnLastName()
        {
            // Arrange
            var user = UserMother.WithLastNameOnly("Doe");

            // Act
            var fullName = user.GetFullName();

            // Assert
            Assert.Equal("Doe", fullName);
        }

        [Fact]
        public void GetFullName_WithoutNames_ShouldReturnUsername()
        {
            // Arrange
            var user = UserMother.Default();

            // Act
            var fullName = user.GetFullName();

            // Assert
            Assert.Equal("testuser", fullName);
        }

        [Fact]
        public void GetDisplayName_ShouldReturnSameAsGetFullName()
        {
            // Arrange
            var user = UserMother.WithFullName("John", "Doe");

            // Act
            var displayName = user.GetDisplayName();
            var fullName = user.GetFullName();

            // Assert
            Assert.Equal(fullName, displayName);
        }

        #endregion

        #region Profile Completion Tests

        [Fact]
        public void HasCompletedProfile_WithAllFields_ShouldReturnTrue()
        {
            // Arrange
            var user = UserMother.WithCompletedProfile();

            // Act
            var isCompleted = user.HasCompletedProfile();

            // Assert
            Assert.True(isCompleted);
        }

        [Theory]
        [InlineData(null, "Doe", "Bio")]
        [InlineData("John", null, "Bio")]
        [InlineData("John", "Doe", null)]
        [InlineData("", "Doe", "Bio")]
        [InlineData("John", "", "Bio")]
        [InlineData("John", "Doe", "")]
        public void HasCompletedProfile_WithMissingFields_ShouldReturnFalse(
            string firstName, string lastName, string bio)
        {
            // Arrange
            var user = User.CreateBuilder()
                .WithUsername("testuser")
                .WithEmail("test@example.com")
                .WithPasswordHash(PASSWORD_HASH)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithBio(bio)
                .Build();

            // Act
            var isCompleted = user.HasCompletedProfile();

            // Assert
            Assert.False(isCompleted);
        }

        #endregion

        #region Unit System Tests

        [Fact]
        public void GetWeightUnit_WithMetric_ShouldReturnKg()
        {
            // Arrange
            var user = UserMother.WithMetricUnits();

            // Act
            var unit = user.GetWeightUnit();

            // Assert
            Assert.Equal(UnitSystem.Metric.WeightUnit, unit);
        }

        [Fact]
        public void GetWeightUnit_WithImperial_ShouldReturnLbs()
        {
            // Arrange
            var user = UserMother.WithImperialUnits();

            // Act
            var unit = user.GetWeightUnit();

            // Assert
            Assert.Equal(UnitSystem.Imperial.WeightUnit, unit);
        }

        [Fact]
        public void GetLengthUnit_WithMetric_ShouldReturnCm()
        {
            // Arrange
            var user = UserMother.WithMetricUnits();

            // Act
            var unit = user.GetLengthUnit();

            // Assert
            Assert.Equal(UnitSystem.Metric.LengthUnit, unit);
        }

        #endregion

        #region Entity Base Properties Tests

        [Fact]
        public void Build_ShouldGenerateId()
        {
            // Arrange & Act
            var user = UserMother.Default();

            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public void Build_ShouldSetCreatedAt()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;

            // Act
            var user = UserMother.Default();
            var afterCreation = DateTime.UtcNow;

            // Assert
            Assert.True(user.CreatedAt >= beforeCreation);
            Assert.True(user.CreatedAt <= afterCreation);
        }

        [Fact]
        public void Build_ShouldSetUpdatedAt()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;

            // Act
            var user = UserMother.Default();
            var afterCreation = DateTime.UtcNow;

            // Assert
            Assert.True(user.UpdatedAt >= beforeCreation);
            Assert.True(user.UpdatedAt <= afterCreation);
        }

        #endregion
    }
}
