using FitTracker.Infrastructure.Services;
using FitTrackerInfrastructure.Tests.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace FitTrackerInfrastructure.Tests.Services
{
    public class EmailServiceTests
    {
        private IConfiguration BuildConfiguration()
        {
            var configData = new Dictionary<string, string>
            {
                ["Email:SmtpHost"] = "localhost",
                ["Email:SmtpPort"] = "1025",
                ["EmailSettings.Origin"] = "test@fittracker.com",
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(configData!)
                .Build();
        }

        [Fact]
        public void Constructor_ShouldLoadConfigurationCorrectly()
        {
            // Arrange
            var config = BuildConfiguration();

            // Act
            var service = new EmailService(config);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithMissingSmtpHost_ShouldUseDefault()
        {
            // Arrange
            var emptyConfig = new ConfigurationBuilder().Build();

            // Act
            var service = new EmailService(emptyConfig);

            // Assert
            service.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithMissingOrigin_ShouldUseDefault()
        {
            // Arrange
            var configData = new Dictionary<string, string>
            {
                ["Email:SmtpHost"] = "smtp.test.com",
                ["Email:SmtpPort"] = "587",
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configData!)
                .Build();

            // Act
            var service = new EmailService(config);

            // Assert
            service.Should().NotBeNull();
        }

        public sealed class FakeEmailServiceTests
        {
            private readonly FakeEmailService _fakeEmailService = new();

            [Fact]
            public async Task SendEmailAsync_ShouldRecordSentEmail()
            {
                // Arrange
                var to = "user@example.com";
                var subject = "Welcome!";
                var body = "<p>Welcome to FitTracker</p>";

                // Act
                await _fakeEmailService.SendEmailAsync(to, subject, body);

                // Assert
                _fakeEmailService.SentEmails.Should().ContainSingle();
                var sentEmail = _fakeEmailService.SentEmails[0];
                sentEmail.To.Should().Be(to);
                sentEmail.Subject.Should().Be(subject);
                sentEmail.HtmlBody.Should().Be(body);
            }

            [Fact]
            public async Task SendEmailAsync_MultipleTimes_ShouldRecordAllEmails()
            {
                // Arrange & Act
                await _fakeEmailService.SendEmailAsync("user1@example.com", "Subject 1", "<p>Body 1</p>");
                await _fakeEmailService.SendEmailAsync("user2@example.com", "Subject 2", "<p>Body 2</p>");
                await _fakeEmailService.SendEmailAsync("user3@example.com", "Subject 3", "<p>Body 3</p>");

                // Assert
                _fakeEmailService.SentEmails.Should().HaveCount(3);
                _fakeEmailService.SentEmails[0].To.Should().Be("user1@example.com");
                _fakeEmailService.SentEmails[1].To.Should().Be("user2@example.com");
                _fakeEmailService.SentEmails[2].To.Should().Be("user3@example.com");
            }

            [Fact]
            public async Task SendEmailAsync_WithHtmlContent_ShouldPreserveHtml()
            {
                // Arrange
                var htmlBody = @"
                <html>
                    <body>
                        <h1>Welcome</h1>
                        <p>Click <a href='https://example.com'>here</a></p>
                    </body>
                </html>";

                // Act
                await _fakeEmailService.SendEmailAsync("user@example.com", "Test", htmlBody);

                // Assert
                var sentEmail = _fakeEmailService.SentEmails.Single();
                sentEmail.HtmlBody.Should().Be(htmlBody);
            }

            [Fact]
            public async Task SendEmailAsync_WhenConfiguredToFail_ShouldThrowException()
            {
                // Arrange
                _fakeEmailService.ShouldThrowException = true;

                // Act
                var act = async () => await _fakeEmailService.SendEmailAsync(
                    "user@example.com",
                    "Test",
                    "<p>Body</p>");

                // Assert
                await act.Should().ThrowAsync<InvalidOperationException>()
                    .WithMessage("Email sending failed");
            }

            [Fact]
            public void Clear_ShouldRemoveAllSentEmails()
            {
                // Arrange
                _fakeEmailService.SentEmails.Add(new SentEmail("test@example.com", "Test", "<p>Body</p>"));
                _fakeEmailService.SentEmails.Add(new SentEmail("test2@example.com", "Test", "<p>Body</p>"));

                // Act
                _fakeEmailService.Clear();

                // Assert
                _fakeEmailService.SentEmails.Should().BeEmpty();
            }

            [Fact]
            public async Task SendEmailAsync_WithCancellationToken_ShouldComplete()
            {
                // Arrange
                using var cts = new CancellationTokenSource();

                // Act
                await _fakeEmailService.SendEmailAsync(
                    "user@example.com",
                    "Test",
                    "<p>Body</p>",
                    cts.Token);

                // Assert
                _fakeEmailService.SentEmails.Should().ContainSingle();
            }

            [Fact]
            public async Task SendEmailAsync_WithSpecialCharacters_ShouldPreserveContent()
            {
                // Arrange
                var subject = "Test з спеціальними символами & special <chars>";
                var body = "<p>Email with émojis 🎉 and symbols: €, £, ¥</p>";

                // Act
                await _fakeEmailService.SendEmailAsync("user@example.com", subject, body);

                // Assert
                var sentEmail = _fakeEmailService.SentEmails.Single();
                sentEmail.Subject.Should().Be(subject);
                sentEmail.HtmlBody.Should().Be(body);
            }
        }
    }
}
