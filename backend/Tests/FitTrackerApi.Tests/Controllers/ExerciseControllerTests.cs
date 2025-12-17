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
    }
}

