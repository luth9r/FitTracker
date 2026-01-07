using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Handlers.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTrackerInfrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerApplication.Tests.CommandHandlers;

public class ForgotPasswordCommandHandlerTests
{
    private readonly ForgotPasswordCommandHandler _handler;
    private readonly Mock<IUserReadRepository> _mockReadRepository = new();
    private readonly Mock<IUserWriteRepository> _mockWriteRepository = new();
    private readonly Mock<IUnitOfWork> _mockUnit = new();
    private readonly Mock<IRateLimitService> _mockRateLimitService = new();
    private readonly Mock<ILocalizationService> _mockLocalization = new();
    private readonly Mock<ILogger<ForgotPasswordCommandHandler>> _mockLogger = new();

    public ForgotPasswordCommandHandlerTests()
    {
        _handler = new ForgotPasswordCommandHandler(
            _mockReadRepository.Object,
            _mockWriteRepository.Object,
            _mockUnit.Object,
            _mockRateLimitService.Object,
            _mockLocalization.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldReturnSuccess()
    {
        // Arrange
        var command = new ForgotPasswordCommand("nonexistent@example.com");
        _mockReadRepository
            .Setup(r => r.GetByEmailReadonlyAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockWriteRepository.Verify(
            w => w.Update(It.IsAny<User>()),
            Times.Never);
        _mockUnit.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UserEmailNotVerified_ShouldReturnSuccess()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser("unverified", "unverified@example.com");
        // Assuming user has IsEmailVerified property that can be set to false
        var command = new ForgotPasswordCommand(user.Email);

        _mockReadRepository
            .Setup(r => r.GetByEmailReadonlyAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockWriteRepository.Verify(
            w => w.Update(It.IsAny<User>()),
            Times.Never);
        _mockUnit.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Password reset denied")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_RateLimitExceeded_ShouldReturnFailure()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser("ratelimited", "ratelimited@example.com", isEmailVerified: true);
        var command = new ForgotPasswordCommand(user.Email);
        var culture = "en-US";
        var errorMessage = "Rate limit exceeded";

        _mockLocalization
            .Setup(l => l.GetCurrentCulture())
            .Returns(culture);

        _mockReadRepository
            .Setup(r => r.GetByEmailReadonlyAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockRateLimitService
            .Setup(r => r.IsAllowedAsync(
                $"ratelimit:password-reset:{user.Id}",
                TimeSpan.FromMinutes(1)))
            .ReturnsAsync(false);

        _mockLocalization
            .Setup(l => l.GetString("User.RateLimitExceeded", culture))
            .Returns(errorMessage);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(errorMessage);

        _mockLocalization.Verify(
            l => l.GetCurrentCulture(),
            Times.Once);

        _mockRateLimitService.Verify(
            r => r.IsAllowedAsync(
                $"ratelimit:password-reset:{user.Id}",
                TimeSpan.FromMinutes(1)),
            Times.Once);

        _mockWriteRepository.Verify(
            w => w.Update(It.IsAny<User>()),
            Times.Never);

        _mockUnit.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateUserAndSaveChanges()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser("validuser", "valid@example.com", isEmailVerified: true);
        var command = new ForgotPasswordCommand(user.Email);
        var culture = "en-US";

        _mockLocalization
            .Setup(l => l.GetCurrentCulture())
            .Returns(culture);

        _mockReadRepository
            .Setup(r => r.GetByEmailReadonlyAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockRateLimitService
            .Setup(r => r.IsAllowedAsync(
                $"ratelimit:password-reset:{user.Id}",
                TimeSpan.FromMinutes(1)))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockReadRepository.Verify(
            r => r.GetByEmailReadonlyAsync(command.Email, It.IsAny<CancellationToken>()),
            Times.Once);
        _mockRateLimitService.Verify(
            r => r.IsAllowedAsync(
                $"ratelimit:password-reset:{user.Id}",
                TimeSpan.FromMinutes(1)),
            Times.Once);
        _mockWriteRepository.Verify(
            w => w.Update(user),
            Times.Once);
        _mockUnit.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CancellationRequested_ShouldRespectCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var command = new ForgotPasswordCommand("cancel@example.com");

        _mockReadRepository
            .Setup(r => r.GetByEmailReadonlyAsync(
                It.IsAny<string>(),
                It.Is<CancellationToken>(ct => ct.IsCancellationRequested)))
            .ThrowsAsync(new OperationCanceledException(cts.Token));

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            _handler.Handle(command, cts.Token));
    }
}
