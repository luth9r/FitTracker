using FitTracker.Application.Events;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Handlers.Commands;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTrackerInfrastructure.Tests.Helpers;
using FluentAssertions;
using MediatR;
using Moq;

namespace FitTrackerApplication.Tests.CommandHandlers;

public class ForgotPasswordCommandHandlerTests
{
    private readonly ForgotPasswordCommandHandler _handler;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IUserReadRepository> _mockReadRepository;

    public ForgotPasswordCommandHandlerTests()
    {
        _mockReadRepository = new Mock<IUserReadRepository>();
        _mockMediator = new Mock<IMediator>();
        _handler = new ForgotPasswordCommandHandler(_mockReadRepository.Object, _mockMediator.Object);
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
        _mockMediator.Verify(
            m => m.Publish(
                It.IsAny<UserPasswordResetRequestedEvent>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UserFound_ShouldPublishEventAndReturnSuccess()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser("founduser", "found@example.com");
        var command = new ForgotPasswordCommand(user.Email);

        _mockReadRepository
            .Setup(r => r.GetByEmailReadonlyAsync(user.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockMediator
            .Setup(m => m.Publish(
                It.Is<UserPasswordResetRequestedEvent>(e =>
                    e.UserId == user.Id &&
                    e.Email == user.Email &&
                    e.Username == user.Username),
                It.IsAny<CancellationToken>()))
            .Verifiable();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockReadRepository.Verify(
            r => r.GetByEmailReadonlyAsync(
                user.Email,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mockMediator.VerifyAll();
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


        await Assert.ThrowsAsync<OperationCanceledException>(() => _handler.Handle(command, cts.Token));
    }
}
