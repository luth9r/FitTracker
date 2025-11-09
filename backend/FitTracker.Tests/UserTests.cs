using FitTracker.Domain.Entities;
using static FitTracker.Domain.Entities.User;

namespace FitTracker.Tests
{
    public class UserTests
    {
        [Fact]
        public void User_Creation_SetsProperties()
        {
            var user = new UserBuilder()
                .WithUsername("username")
                .WithEmail("email@test.com")
                .WithPasswordHash(new string('A', 61))
                .WithFirstName("test")
                .WithLastName("test")
                .WithAvatar("avatar")
                .WithBio("bio")
                .WithMetricUnits()
                .Build();

            Assert.Equal("username", user.Username);
            Assert.Equal("email@test.com", user.Email);
            Assert.Equal(new string('A', 61), user.PasswordHash);
            Assert.Equal("test", user.FirstName);
            Assert.Equal("test", user.LastName);
            Assert.Equal("avatar", user.Avatar);
            Assert.Equal("bio", user.Bio);
            Assert.Equal("metric", user.PreferredUnits.Name);

        }
    }
}
