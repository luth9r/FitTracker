using FitTracker.Domain.Entities;

namespace FitTrackerInfrastructure.Tests.Helpers;

public static class UserTestHelper
{
    public static User CreateTestUser(
        string username = "testuser",
        string email = "test@example.com",
        string passwordHash = "hashedpassword123",
        string firstName = null,
        string lastName = null,
        bool isEmailVerified = false)
    {
        var user = User.Create(username, email, passwordHash, firstName, lastName);

        if (isEmailVerified)
        {
            user.SetEmailVerified();
        }

        return user;
    }

    public static User CreateVerifiedUser()
    {
        var user = User.Create(
            "verifieduser",
            "verified@example.com",
            "hashedpassword123",
            "John",
            "Doe");

        user.SetEmailVerified();
        user.UpdateProfile("John", "Doe", "Test bio", "https://example.com/avatar.jpg");

        return user;
    }

    public static User CreateGoogleUser(
        string email = "google@example.com",
        string googleProviderId = "google123")
    {
        return User.CreateGoogleUser(
            email,
            googleProviderId,
            "Google",
            "User");
    }
}
