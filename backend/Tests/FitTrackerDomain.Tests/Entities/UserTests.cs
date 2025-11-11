using FitTracker.Domain.Entities;
using FitTracker.Domain.Tests.Factories;
using FitTracker.Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace FitTracker.Domain.Tests.Entities
{
    public class UserTests
    {
        private const string PASSWORD_HASH = "hashedpassword123";

        #region Builder Tests

        [Fact]
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            var builder = User.CreateBuilder();

            builder.Should().NotBeNull();
            builder.Should().BeOfType<User.UserBuilder>();
        }

        [Fact]
        public void Build_WithValidData_ShouldCreateUser()
        {
            var user = UserFactory.Default();

            user.Should().NotBeNull();
            user.Username.Should().Be("testuser");
            user.Email.Should().Be("test@example.com");
            user.PasswordHash.Should().Be(PASSWORD_HASH);
            user.PreferredUnits.Should().Be(UnitSystem.Metric);
        }

        [Fact]
        public void Build_WithMetricUnits_ShouldSetMetricSystem()
        {
            var user = UserFactory.WithMetricUnits();

            user.PreferredUnits.Should().Be(UnitSystem.Metric);
            user.UsesMetric().Should().BeTrue();
            user.UsesImperial().Should().BeFalse();
        }

        [Fact]
        public void Build_WithImperialUnits_ShouldSetImperialSystem()
        {
            var user = UserFactory.WithImperialUnits();

            user.PreferredUnits.Should().Be(UnitSystem.Imperial);
            user.UsesMetric().Should().BeFalse();
            user.UsesImperial().Should().BeTrue();
        }

        [Fact]
        public void Build_WithOptionalFields_ShouldCreateUserWithAllFields()
        {
            var user = UserFactory.WithAllFields();

            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Doe");
            user.Avatar.Should().Be("https://avatar.com/pic.jpg");
            user.Bio.Should().Be("Fitness enthusiast");
            user.PreferredUnits.Should().Be(UnitSystem.Imperial);
        }

        [Theory]
        [InlineData("", "test@example.com", PASSWORD_HASH)]
        [InlineData(null, "test@example.com", PASSWORD_HASH)]
        [InlineData("  ", "test@example.com", PASSWORD_HASH)]
        public void Build_WithInvalidUsername_ShouldThrowValidationException(
            string username, string email, string passwordHash)
        {
            var builder = User.CreateBuilder()
                .WithUsername(username)
                .WithEmail(email)
                .WithPasswordHash(passwordHash);

            Action act = () => builder.Build();

            act.Should().Throw<ValidationException>().WithMessage("*username*");
        }

        [Theory]
        [InlineData("testuser", "", PASSWORD_HASH)]
        [InlineData("testuser", null, PASSWORD_HASH)]
        [InlineData("testuser", "  ", PASSWORD_HASH)]
        public void Build_WithInvalidEmail_ShouldThrowValidationException(
            string username, string email, string passwordHash)
        {
            var builder = User.CreateBuilder()
                .WithUsername(username)
                .WithEmail(email)
                .WithPasswordHash(passwordHash);

            Action act = () => builder.Build();

            act.Should().Throw<ValidationException>().WithMessage("*email*");
        }

        [Theory]
        [InlineData("testuser", "test@example.com", "")]
        [InlineData("testuser", "test@example.com", null)]
        [InlineData("testuser", "test@example.com", "  ")]
        public void Build_WithInvalidPasswordHash_ShouldThrowValidationException(
            string username, string email, string passwordHash)
        {
            var builder = User.CreateBuilder()
                .WithUsername(username)
                .WithEmail(email)
                .WithPasswordHash(passwordHash);

            Action act = () => builder.Build();

            act.Should().Throw<ValidationException>().WithMessage("*password*");
        }

        [Fact]
        public void Build_EmailShouldBeLowerCase()
        {
            var user = UserFactory.WithEmail("TEST@EXAMPLE.COM");

            user.Email.Should().Be("test@example.com");
        }

        #endregion

        #region Update Profile Tests

        [Fact]
        public void UpdateProfile_WithValidData_ShouldUpdateFields()
        {
            var user = UserFactory.Default();
            var originalUpdatedAt = user.UpdatedAt;

            Thread.Sleep(10);
            user.UpdateProfile("Jane", "Smith", "New bio", "https://newavatar.com/pic.jpg");

            user.FirstName.Should().Be("Jane");
            user.LastName.Should().Be("Smith");
            user.Bio.Should().Be("New bio");
            user.Avatar.Should().Be("https://newavatar.com/pic.jpg");
            user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void UpdateProfile_WithNullValues_ShouldAcceptNulls()
        {
            var user = UserFactory.WithCompletedProfile();

            user.UpdateProfile(null, null, null, null);

            user.FirstName.Should().BeNull();
            user.LastName.Should().BeNull();
            user.Bio.Should().BeNull();
            user.Avatar.Should().BeNull();
        }

        #endregion

        #region Update Email Tests

        [Fact]
        public void UpdateEmail_WithValidEmail_ShouldUpdateEmail()
        {
            var user = UserFactory.Default();
            var originalUpdatedAt = user.UpdatedAt;

            Thread.Sleep(10);
            user.UpdateEmail("newemail@example.com");

            user.Email.Should().Be("newemail@example.com");
            user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void UpdateEmail_ShouldConvertToLowerCase()
        {
            var user = UserFactory.Default();

            user.UpdateEmail("NEWEMAIL@EXAMPLE.COM");

            user.Email.Should().Be("newemail@example.com");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void UpdateEmail_WithInvalidEmail_ShouldThrowValidationException(string email)
        {
            var user = UserFactory.Default();

            Action act = () => user.UpdateEmail(email);

            act.Should().Throw<ValidationException>().WithMessage("*email*");
        }

        #endregion

        #region Update Password Tests

        [Fact]
        public void UpdatePasswordHash_WithValidHash_ShouldUpdatePassword()
        {
            var user = UserFactory.Default();
            var originalUpdatedAt = user.UpdatedAt;

            Thread.Sleep(10);
            user.UpdatePasswordHash("newhash123");

            user.PasswordHash.Should().Be("newhash123");
            user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void UpdatePasswordHash_WithInvalidHash_ShouldThrowValidationException(string hash)
        {
            var user = UserFactory.Default();

            Action act = () => user.UpdatePasswordHash(hash);

            act.Should().Throw<ValidationException>().WithMessage("*password*");
        }

        #endregion

        #region Preferred Units Tests

        [Fact]
        public void UpdatePreferredUnits_WithValidUnit_ShouldUpdateUnits()
        {
            var user = UserFactory.Default();
            var originalUpdatedAt = user.UpdatedAt;

            Thread.Sleep(10);
            user.UpdatePreferredUnits(UnitSystem.Imperial);

            user.PreferredUnits.Should().Be(UnitSystem.Imperial);
            user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
        }

        [Fact]
        public void SetMetricUnits_ShouldSetToMetric()
        {
            var user = UserFactory.WithImperialUnits();

            user.SetMetricUnits();

            user.PreferredUnits.Should().Be(UnitSystem.Metric);
            user.UsesMetric().Should().BeTrue();
        }

        [Fact]
        public void SetImperialUnits_ShouldSetToImperial()
        {
            var user = UserFactory.WithMetricUnits();

            user.SetImperialUnits();

            user.PreferredUnits.Should().Be(UnitSystem.Imperial);
            user.UsesImperial().Should().BeTrue();
        }

        [Fact]
        public void UpdatePreferredUnits_WithNull_ShouldThrowArgumentNullException()
        {
            var user = UserFactory.Default();

            Action act = () => user.UpdatePreferredUnits(null);

            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #region Display Name Tests

        [Fact]
        public void GetFullName_WithFirstAndLastName_ShouldReturnFullName()
        {
            var user = UserFactory.WithFullName("John", "Doe");

            var fullName = user.GetFullName();

            fullName.Should().Be("John Doe");
        }

        [Fact]
        public void GetFullName_WithOnlyFirstName_ShouldReturnFirstName()
        {
            var user = UserFactory.WithFirstNameOnly("John");

            var fullName = user.GetFullName();

            fullName.Should().Be("John");
        }

        [Fact]
        public void GetFullName_WithOnlyLastName_ShouldReturnLastName()
        {
            var user = UserFactory.WithLastNameOnly("Doe");

            var fullName = user.GetFullName();

            fullName.Should().Be("Doe");
        }

        [Fact]
        public void GetFullName_WithoutNames_ShouldReturnUsername()
        {
            var user = UserFactory.Default();

            var fullName = user.GetFullName();

            fullName.Should().Be("testuser");
        }

        [Fact]
        public void GetDisplayName_ShouldReturnSameAsGetFullName()
        {
            var user = UserFactory.WithFullName("John", "Doe");

            var displayName = user.GetDisplayName();
            var fullName = user.GetFullName();

            displayName.Should().Be(fullName);
        }

        #endregion

        #region Profile Completion Tests

        [Fact]
        public void HasCompletedProfile_WithAllFields_ShouldReturnTrue()
        {
            var user = UserFactory.WithCompletedProfile();

            var isCompleted = user.HasCompletedProfile();

            isCompleted.Should().BeTrue();
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
            var user = User.CreateBuilder()
                .WithUsername("testuser")
                .WithEmail("test@example.com")
                .WithPasswordHash(PASSWORD_HASH)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithBio(bio)
                .Build();

            var isCompleted = user.HasCompletedProfile();

            isCompleted.Should().BeFalse();
        }

        #endregion

        #region Unit System Tests

        [Fact]
        public void GetWeightUnit_WithMetric_ShouldReturnKg()
        {
            var user = UserFactory.WithMetricUnits();

            var unit = user.GetWeightUnit();

            unit.Should().Be(UnitSystem.Metric.WeightUnit);
        }

        [Fact]
        public void GetWeightUnit_WithImperial_ShouldReturnLbs()
        {
            var user = UserFactory.WithImperialUnits();

            var unit = user.GetWeightUnit();

            unit.Should().Be(UnitSystem.Imperial.WeightUnit);
        }

        [Fact]
        public void GetLengthUnit_WithMetric_ShouldReturnCm()
        {
            var user = UserFactory.WithMetricUnits();

            var unit = user.GetLengthUnit();

            unit.Should().Be(UnitSystem.Metric.LengthUnit);
        }

        [Theory]
        [InlineData(10, "metric", "imperial", 22.0462)]
        [InlineData(22.0462, "imperial", "metric", 10)]
        [InlineData(5, "metric", "metric", 5)]
        public void ConvertWeight_Should_Convert_Correctly(decimal weight, string fromUnit, string toUnit, decimal expected)
        {
            // Arrange
            var from = UnitSystem.FromString(fromUnit);
            var to = UnitSystem.FromString(toUnit);

            var user = UserFactory.WithPreferedUnits(from);

            // Act
            var result = user.ConvertWeight(weight, to);

            // Assert
            result.Should().BeApproximately(expected, 0.0001m);
        }

        #endregion

        #region Entity Base Properties Tests

        [Fact]
        public void Build_ShouldGenerateId()
        {
            var user = UserFactory.Default();

            user.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void Build_ShouldSetCreatedAt()
        {
            var beforeCreation = DateTime.UtcNow;

            var user = UserFactory.Default();

            var afterCreation = DateTime.UtcNow;

            user.CreatedAt.Should().BeOnOrAfter(beforeCreation).And.BeOnOrBefore(afterCreation);
        }

        [Fact]
        public void Build_ShouldSetUpdatedAt()
        {
            var beforeCreation = DateTime.UtcNow;

            var user = UserFactory.Default();

            var afterCreation = DateTime.UtcNow;

            user.UpdatedAt.Should().BeOnOrAfter(beforeCreation).And.BeOnOrBefore(afterCreation);
        }

        #endregion
    }
}
