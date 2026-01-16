using System.Security.Claims;
using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers;
using FitTracker.Application.Features.User.Queries.GetRecentWorkouts;
using FitTracker.Application.Features.User.Queries.GetUserStats;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FitTrackerApi.Tests.Controllers;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<IMediator> _mediatorMock;

    public UserControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);

        _httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext,
        };
    }

    private void SetupUser(Guid userId, string preferredUnits = "metric")
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new("preferred-units", preferredUnits),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        _httpContext.User = principal;
    }

    [Fact]
    public async Task GetCurrentUserAsync_ShouldReturnOk_WithUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetupUser(userId);

        // Act
        var result = await _controller.GetCurrentUser(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(new { UserId = userId });
    }

    [Fact]
    public async Task GetUserStatsAsync_ShouldReturnOk_WithStats()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetupUser(userId, "imperial");
        var expectedStats = new UserStatsResponse(10, 5000, 20, 5);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetUserStatsQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedStats));

        // Act
        var result = await _controller.GetUserStats(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedStats);
    }

    [Fact]
    public async Task GetRecentWorkoutsForUserAsync_ShouldReturnOk_WithWorkouts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetupUser(userId);
        var expectedWorkouts = new List<RecentWorkoutResponse>
        {
            new(Guid.NewGuid(), DateTime.UtcNow, "Workout 1", true, 60, 1000),
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetRecentWorkoutsQuery>(q => q.UserId == userId && q.Take == 5),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<IReadOnlyList<RecentWorkoutResponse>>(expectedWorkouts));

        // Act
        var result = await _controller.GetRecentWorkoutsForUser(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedWorkouts);
    }
}
