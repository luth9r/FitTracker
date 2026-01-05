using System.Security.Claims;
using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers;
using FitTracker.Application.DTOs.Exercise;
using FitTracker.Application.UseCases.Exercise.Queries;
using FitTracker.Domain.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FitTrackerApi.Tests.Controllers;

public class ExerciseControllerTests
{
    private readonly ExerciseController _controller;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<IMediator> _mediatorMock;

    public ExerciseControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ExerciseController(_mediatorMock.Object);

        _httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext,
        };
    }

    private void SetUser(Guid userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        _httpContext.User = new ClaimsPrincipal(identity);
    }

    [Fact]
    public async Task GetExercisesAsync_ShouldReturnOnlyStandardExercises_WhenFilterTypeIsStandard()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUser(userId);

        var filterType = ExerciseFilterType.Standard;

        var expectedResponse = new List<ExerciseResponse>
        {
            new(
                Guid.NewGuid(),
                "Barbell Bench Press",
                "Compound chest exercise performed on a flat bench",
                null,
                null,
                "Chest",
                "Barbell",
                false),
        };

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<GetExerciseQuery>(q =>
                    q.Type == filterType &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<IReadOnlyList<ExerciseResponse>>(expectedResponse));

        // Act
        var result = await _controller.GetExercises(filterType, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeAssignableTo<IReadOnlyList<ExerciseResponse>>();

        var value = (IReadOnlyList<ExerciseResponse>)okResult.Value!;
        value.Should().HaveCount(1);
        value[0].Name.Should().Be("Barbell Bench Press");

        _mediatorMock.Verify(m => m.Send(
                It.Is<GetExerciseQuery>(q =>
                    q.Type == ExerciseFilterType.Standard &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExercisesAsync_ShouldReturnAllExercises_WhenFilterTypeIsAll()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUser(userId);

        var filterType = ExerciseFilterType.All;

        var expectedResponse = new List<ExerciseResponse>
        {
            new(
                Guid.NewGuid(),
                "Barbell Bench Press",
                "Compound chest exercise performed on a flat bench",
                null,
                null,
                "Chest",
                "Barbell",
                false),
            new(
                Guid.NewGuid(),
                "John's Special Curl",
                "Custom bicep curl variation",
                null,
                null,
                "Biceps",
                "Dumbbell",
                true),
        };

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<GetExerciseQuery>(q =>
                    q.Type == filterType &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<IReadOnlyList<ExerciseResponse>>(expectedResponse));

        // Act
        var result = await _controller.GetExercises(filterType, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeAssignableTo<IReadOnlyList<ExerciseResponse>>();

        var value = (IReadOnlyList<ExerciseResponse>)okResult.Value!;
        value.Should().HaveCount(2);
        value.Should().ContainSingle(x => x.IsCustom);
        value.Should().ContainSingle(x => !x.IsCustom);

        _mediatorMock.Verify(m => m.Send(
                It.Is<GetExerciseQuery>(q =>
                    q.Type == ExerciseFilterType.All &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExercisesAsync_ShouldUseDefaultFilterAll_WhenTypeNotProvided()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUser(userId);

        var expectedResponse = new List<ExerciseResponse>();

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<GetExerciseQuery>(q =>
                    q.Type == ExerciseFilterType.All &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<IReadOnlyList<ExerciseResponse>>(expectedResponse));

        // Act
        var result = await _controller.GetExercises(cancellationToken: CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeAssignableTo<IReadOnlyList<ExerciseResponse>>();

        var value = (IReadOnlyList<ExerciseResponse>)okResult.Value!;
        value.Should().BeEmpty();

        _mediatorMock.Verify(m => m.Send(
                It.Is<GetExerciseQuery>(q =>
                    q.Type == ExerciseFilterType.All &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetExerciseDetailsByIdAsync_ShouldReturnExerciseDetails_WhenExerciseExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        SetUser(userId);

        var exerciseId = Guid.NewGuid();

        var expectedResponse = new ExerciseDetailsResponse(
            exerciseId,
            "Bench Press",
            "Chest",
            "Barbell",
            "Flat barbell bench press.",
            "https://example.com/images/bench-press.png",
            "https://example.com/videos/bench-press.mp4",
            false,

            // PR / records
            120.0,
            10,
            900.0,
            2500.0,
            new DateTime(2024, 1, 10),
            new DateTime(2024, 1, 15),
            new DateTime(2024, 1, 20),
            new DateTime(2024, 1, 25),
            15,
            60,
            450,
            20000.0,
            80.0,
            8.0,
            new DateTime(2024, 2, 1),
            new List<ExerciseHistoryPointResponse>
            {
                new("2024-01-10", 500.0),
                new("2024-01-20", 900.0),
                new("2024-01-25", 1100.0),
            });

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<GetExerciseByIdQuery>(q =>
                    q.exerciseId == exerciseId &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedResponse));

        // Act
        var result = await _controller.GetExerciseDetailsById(exerciseId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeAssignableTo<ExerciseDetailsResponse>();
        var value = (ExerciseDetailsResponse)okResult.Value!;

        value.Id.Should().Be(exerciseId);

        _mediatorMock.Verify(m => m.Send(
                It.Is<GetExerciseByIdQuery>(q =>
                    q.exerciseId == exerciseId &&
                    q.UserId == userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
