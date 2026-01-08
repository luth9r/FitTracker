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

public class UserRegisteredConsumerTests
{
    private readonly Mock<IJwtTokenGenerator> _mockJwt = new();
    private readonly Mock<IConfiguration> _mockConfig = new();
    private readonly Mock<IEmailService> _mockEmail = new();
    private readonly Mock<ILocalizationService> _mockLocalization = new();

    [Fact]
    public async Task Consume_ValidEvent_ShouldSendEmail()
    {
        // Arrange
        var eventMessage = UserRegisteredEvent.Create(Guid.NewGuid(), "test@test.com", "user", "en");
        SetupStandardMocks();

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserRegisteredConsumer>(
            _mockJwt.Object,
            _mockConfig.Object,
            _mockEmail.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);

        // Assert
        var consumerHarness = harness.GetConsumerHarness<UserRegisteredConsumer>();
        (await consumerHarness.Consumed.Any<UserRegisteredEvent>()).Should().BeTrue();

        _mockEmail.Verify(e => e.SendEmailAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Consume_EmailServiceFails_ShouldThrowExceptionAndFault()
    {
        // Arrange
        var eventMessage = UserRegisteredEvent.Create(Guid.NewGuid(), "test@test.com", "user", "en");
        SetupStandardMocks();

        _mockEmail
            .Setup(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("SMTP connection failure"));

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserRegisteredConsumer>(
            _mockJwt.Object,
            _mockConfig.Object,
            _mockEmail.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);

        // Assert
        (await harness.Published.Any<Fault<UserRegisteredEvent>>()).Should().BeTrue();
    }

    [Fact]
    public async Task Consume_ValidEvent_ShouldConstructCorrectEmailBody()
    {
        // Arrange
        var username = "IronMan";
        var token = "secret-jwt-123";
        var linkBase = "https://fittracker.com/verify";
        var expectedLink = $"{linkBase}?token={token}";

        var eventMessage = UserRegisteredEvent.Create(Guid.NewGuid(), "tony@stark.com", username, "en-US");

        _mockConfig.Setup(c => c["App:VerificationLinkBase"]).Returns(linkBase);
        _mockJwt.Setup(j => j.GenerateVerificationToken(It.IsAny<Guid>())).Returns(token);
        _mockLocalization.Setup(l => l.GetString("Email.Verification.Body", "en-US"))
            .Returns("Hello {0}, verify here: {1}");

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserRegisteredConsumer>(
            _mockJwt.Object,
            _mockConfig.Object,
            _mockEmail.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);

        // Assert
        _mockEmail.Verify(e => e.SendEmailAsync(
                "tony@stark.com",
                It.IsAny<string>(),
                It.Is<string>(body => body.Contains(username) && body.Contains(expectedLink)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("uk-UA")]
    public async Task Consume_ShouldUseCultureFromEvent(string culture)
    {
        // Arrange
        var eventMessage = UserRegisteredEvent.Create(Guid.NewGuid(), "test@test.com", "user", culture);
        SetupStandardMocks();

        await using var provider = MassTransitTestHelper.CreateConsumerProvider<UserRegisteredConsumer>(
            _mockJwt.Object,
            _mockConfig.Object,
            _mockEmail.Object,
            _mockLocalization.Object);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness.Bus.Publish(eventMessage);
        await harness.InactivityTask;

        var consumerHarness = harness.GetConsumerHarness<UserRegisteredConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<UserRegisteredEvent>());

        // Assert
        _mockLocalization.Verify(l => l.GetString(It.IsAny<string>(), culture), Times.AtLeastOnce);
    }

    private void SetupStandardMocks()
    {
        _mockConfig.Setup(c => c["App:VerificationLinkBase"]).Returns("https://link.com");
        _mockJwt.Setup(j => j.GenerateVerificationToken(It.IsAny<Guid>())).Returns("token");
        _mockLocalization.Setup(l => l.GetString(It.IsAny<string>(), It.IsAny<string>())).Returns("template");
    }
}
