using FitTracker.Application.Interfaces;
using FitTracker.Domain.Events;
using FitTracker.Infrastructure.Messaging.Consumers;
using FitTrackerInfrastructure.Tests.Helpers;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerInfrastructure.Tests.Consumers;

public class UserVerificationRequestedConsumerTests
{
    private readonly Mock<IEmailService> _mockEmail = new();
    private readonly Mock<IJwtTokenGenerator> _mockJwt = new();
    private readonly Mock<IConfiguration> _mockConfig = new();
    private readonly Mock<ILocalizationService> _mockLocalization = new();

    [Fact]
    public async Task Consume_ValidEvent_ShouldGenerateTokenAndSendEmail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "verify-me@test.com";
        var username = "IronMan";
        var token = "verification-jwt-token";
        var culture = "en-US";

        var eventMessage = UserRequestedVerificationEvent.Create(userId, email, username, culture);

        _mockConfig.Setup(c => c["App:VerificationLinkBase"]).Returns("https://fit.com/verify");
        _mockJwt.Setup(j => j.GenerateVerificationToken(userId)).Returns(token);
        _mockLocalization.Setup(l => l.GetString("Email.Verification.Subject", culture)).Returns("Verify Account");
        _mockLocalization.Setup(l => l.GetString("Email.Verification.Body", culture)).Returns("Hi {0}, link {1}");

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserVerificationRequestedConsumer>(
            _mockEmail.Object,
            _mockJwt.Object,
            _mockConfig.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);
        await harness.InactivityTask;

        // Assert
        var consumerHarness = harness.GetConsumerHarness<UserVerificationRequestedConsumer>();
        (await consumerHarness.Consumed.Any<UserRequestedVerificationEvent>()).Should().BeTrue();

        _mockJwt.Verify(j => j.GenerateVerificationToken(userId), Times.Once);

        _mockEmail.Verify(
            e => e.SendEmailAsync(
                email,
                "Verify Account",
                It.Is<string>(b => b.Contains(username) && b.Contains(token)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Consume_WhenEmailServiceFails_ShouldThrowExceptionForRetry()
    {
        // Arrange
        var eventMessage = UserRequestedVerificationEvent.Create(Guid.NewGuid(), "error@test.com", "user", "en");

        _mockJwt.Setup(j => j.GenerateVerificationToken(It.IsAny<Guid>())).Returns("token");
        _mockLocalization.Setup(l => l.GetString(It.IsAny<string>(), It.IsAny<string>())).Returns("template");

        _mockEmail
            .Setup(e => e.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("SMTP Down"));

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserVerificationRequestedConsumer>(
            _mockEmail.Object,
            _mockJwt.Object,
            _mockConfig.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);

        // Assert
        (await harness.Published.Any<Fault<UserRequestedVerificationEvent>>()).Should().BeTrue();
    }
}
