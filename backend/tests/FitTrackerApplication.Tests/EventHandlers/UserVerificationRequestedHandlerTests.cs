using System;
using System.Collections.Generic;
using System.Text;
using FitTracker.Application.Events;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Handlers.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerApplication.Tests.EventHandlers
{
    public class UserVerificationRequestedHandlerTests
    {
        private readonly Mock<IJwtTokenGenerator> _jwtTokenServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<ILocalizationService> _localizationMock;
        private readonly Mock<ILogger<UserVerificationRequestedHandler>> _loggerMock;
        private readonly UserVerificationRequestedHandler _handler;

        public UserVerificationRequestedHandlerTests()
        {
            _jwtTokenServiceMock = new Mock<IJwtTokenGenerator>();
            _configurationMock = new Mock<IConfiguration>();
            _emailServiceMock = new Mock<IEmailService>();
            _localizationMock = new Mock<ILocalizationService>();
            _loggerMock = new Mock<ILogger<UserVerificationRequestedHandler>>();

            _handler = new UserVerificationRequestedHandler(
                _jwtTokenServiceMock.Object,
                _configurationMock.Object,
                _emailServiceMock.Object,
                _localizationMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldGenerateTokenAndSendVerificationEmail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "TestUser";
            var email = "test@example.com";
            var generatedToken = "verification-jwt-token";
            var verificationLinkBase = "https://app.com/verify-email";
            var subject = "Verify your email";
            var bodyTemplate = "Hello {0}, verify here: {1}";

            var notification = new UserRequestedVerificationEvent(userId, email, username);

            _jwtTokenServiceMock
                .Setup(x => x.GenerateVerificationToken(userId))
                .Returns(generatedToken);

            _configurationMock
                .Setup(x => x["App:VerificationLinkBase"])
                .Returns(verificationLinkBase);

            _localizationMock
                .Setup(x => x.GetString("Email.Verification.Subject"))
                .Returns(subject);

            _localizationMock
                .Setup(x => x.GetString("Email.Verification.Body"))
                .Returns(bodyTemplate);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _jwtTokenServiceMock.Verify(
                x => x.GenerateVerificationToken(userId),
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
