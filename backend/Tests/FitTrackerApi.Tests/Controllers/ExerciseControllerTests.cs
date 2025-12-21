using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
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

namespace FitTrackerApi.Tests.Controllers
{
    public class ExerciseControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExerciseController _controller;
        private readonly DefaultHttpContext _httpContext;

        public ExerciseControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ExerciseController(_mediatorMock.Object);

            _httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            };
        }

        private void SetUser(Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
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
                    false)
            };

            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetExerciseQuery>(q =>
                        q.Type == filterType &&
                        q.UserId == userId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success<IReadOnlyList<ExerciseResponse>>(expectedResponse));

            // Act
            var result = await _controller.GetExercisesAsync(filterType, CancellationToken.None);

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
                    true)
            };

            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetExerciseQuery>(q =>
                        q.Type == filterType &&
                        q.UserId == userId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success<IReadOnlyList<ExerciseResponse>>(expectedResponse));

            // Act
            var result = await _controller.GetExercisesAsync(filterType, CancellationToken.None);

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
            var result = await _controller.GetExercisesAsync(cancellationToken: CancellationToken.None);

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
                Id: exerciseId,
                Name: "Bench Press",
                MuscleGroup: "Chest",
                Equipment: "Barbell",
                Description: "Flat barbell bench press.",
                ImageUrl: "https://example.com/images/bench-press.png",
                VideoUrl: "https://example.com/videos/bench-press.mp4",
                IsCustom: false,

                // PR / records
                MaxWeightKg: 120.0,
                MaxReps: 10,
                MaxVolume: 900.0,
                MaxTotalVolume: 2500.0,
                MaxWeightDate: new DateTime(2024, 1, 10),
                MaxRepsDate: new DateTime(2024, 1, 15),
                MaxVolumeDate: new DateTime(2024, 1, 20),
                MaxTotalVolumeDate: new DateTime(2024, 1, 25),

                TotalWorkouts: 15,
                TotalSets: 60,
                TotalReps: 450,
                TotalLifted: 20000.0,
                AvgWeightPerSet: 80.0,
                AvgRepsPerSet: 8.0,
                LastPerformed: new DateTime(2024, 2, 1),

                VolumeHistory: new List<ExerciseHistoryPointResponse>
                {
                    new ExerciseHistoryPointResponse("2024-01-10", 500.0),
                    new ExerciseHistoryPointResponse("2024-01-20", 900.0),
                    new ExerciseHistoryPointResponse("2024-01-25", 1100.0),
                });

            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetExerciseByIdQuery>(q =>
                        q.exerciseId == exerciseId &&
                        q.UserId == userId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success<ExerciseDetailsResponse>(expectedResponse));

            // Act
            var result = await _controller.GetExerciseDetailsByIdAsync(exerciseId, cancellationToken: CancellationToken.None);

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
}

