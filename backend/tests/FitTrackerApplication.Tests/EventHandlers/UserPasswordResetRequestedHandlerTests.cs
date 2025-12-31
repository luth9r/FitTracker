using FitTracker.Application.Events;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Handlers.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerApplication.Tests.EventHandlers
{
    public class UserPasswordResetRequestedHandlerTests
    {
        private readonly Mock<ILocalizationService> _localizationMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenServiceMock;
        private readonly Mock<ILogger<UserPasswordResetRequestedHandler>> _loggerMock;
        private readonly UserPasswordResetRequestedHandler _handler;

        public UserPasswordResetRequestedHandlerTests()
        {
            _localizationMock = new Mock<ILocalizationService>();
            _emailServiceMock = new Mock<IEmailService>();
            _configurationMock = new Mock<IConfiguration>();
            _jwtTokenServiceMock = new Mock<IJwtTokenGenerator>();
            _loggerMock = new Mock<ILogger<UserPasswordResetRequestedHandler>>();

            _handler = new UserPasswordResetRequestedHandler(
                _localizationMock.Object,
                _emailServiceMock.Object,
                _configurationMock.Object,
                _jwtTokenServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldGenerateTokenAndSendEmail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "TestUser";
            var email = "test@example.com";
            var generatedToken = "generated-jwt-token";
            var resetLinkBase = "https://app.com/reset-password";
            var subject = "Reset your password";
            var bodyTemplate = "Hello {0}, click here: {1}";

            var notification = new UserPasswordResetRequestedEvent(userId, email, username);

            _jwtTokenServiceMock
                .Setup(x => x.GeneratePasswordResetToken(userId))
                .Returns(generatedToken);

            _configurationMock
                .Setup(x => x["App:ResetPasswordLinkBase"])
                .Returns(resetLinkBase);

            _localizationMock
                .Setup(x => x.GetString("Email.ResetPassword.Subject"))
                .Returns(subject);

            _localizationMock
                .Setup(x => x.GetString("Email.ResetPassword.Body"))
                .Returns(bodyTemplate);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _jwtTokenServiceMock.Verify(
                x => x.GeneratePasswordResetToken(userId),
                Times.Once);

            _emailServiceMock.Verify(
                x => x.SendEmailAsync(
                    email,
                    subject,
                    It.Is<string>(body =>
                        body.Contains(username) &&
                        body.Contains(generatedToken)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
