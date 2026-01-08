using FitTracker.Application.Interfaces;
using FitTracker.Domain.Events;
using FitTracker.Infrastructure.Messaging.Consumers;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerInfrastructure.Tests.Helpers;

public class UserRequestedPasswordResetConsumerTests
{
    private readonly Mock<IEmailService> _mockEmail = new();
    private readonly Mock<IJwtTokenGenerator> _mockJwt = new();
    private readonly Mock<IConfiguration> _mockConfig = new();
    private readonly Mock<ILocalizationService> _mockLocalization = new();

    [Fact]
    public async Task Consume_ValidEvent_ShouldSendResetEmail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "ironman@stark.com";
        var resetToken = "jwt-reset-token";
        var eventMessage = UserRequestedPasswordResetEvent.Create(userId, email, "Tony", "en-US");

        _mockConfig.Setup(c => c["App:ResetPasswordLinkBase"]).Returns("https://fit.com/reset");
        _mockJwt.Setup(j => j.GeneratePasswordResetToken(userId)).Returns(resetToken);
        _mockLocalization.Setup(l => l.GetString("Email.ResetPassword.Subject")).Returns("Reset your password");
        _mockLocalization.Setup(l => l.GetString("Email.ResetPassword.Body")).Returns("Token: {1}");

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserRequestedPasswordResetConsumer>(
            _mockEmail.Object,
            _mockJwt.Object,
            _mockConfig.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);

        // Assert
        var consumerHarness = harness.GetConsumerHarness<UserRequestedPasswordResetConsumer>();
        (await consumerHarness.Consumed.Any<UserRequestedPasswordResetEvent>()).Should().BeTrue();

        _mockJwt.Verify(j => j.GeneratePasswordResetToken(userId), Times.Once);
        _mockJwt.Verify(j => j.GenerateVerificationToken(It.IsAny<Guid>()), Times.Never);

        _mockEmail.Verify(e => e.SendEmailAsync(
            email,
            "Reset your password",
            It.Is<string>(b => b.Contains(resetToken)),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Consume_LocalizationMissing_ShouldStillAttemptToSendEmail()
    {
        // Arrange
        var eventMessage = UserRequestedPasswordResetEvent.Create(Guid.NewGuid(), "test@test.com", "user", "en");

        _mockLocalization.Setup(l => l.GetString(It.IsAny<string>())).Returns(string.Empty);
        _mockJwt.Setup(j => j.GeneratePasswordResetToken(It.IsAny<Guid>())).Returns("token");

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserRequestedPasswordResetConsumer>(
            _mockEmail.Object,
            _mockJwt.Object,
            _mockConfig.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);

        // Assert
        (await harness.Consumed.Any<UserRequestedPasswordResetEvent>()).Should().BeTrue();
        _mockEmail.Verify(e => e.SendEmailAsync(It.IsAny<string>(), "", It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
