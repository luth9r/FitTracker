using CSharpFunctionalExtensions;
using FitTracker.Api.Controllers;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.UseCases.User.Commands;
using FitTracker.Application.UseCases.User.Commands.Google;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FitTrackerApi.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _controller;
    private readonly DefaultHttpContext _httpContext;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(_mediatorMock.Object, _loggerMock.Object);

        _httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext
        };
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "Password123!"
        };
        var expectedResponse = new LoginResponse
        {
            JWT = "jwt-token",
            PreferredUnits = "metric",
            Email = "test@example.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<LoginResponse, ValidationResult>(expectedResponse));

        // Act
        var result = await _controller.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);

        // Verify cookies were set
        var cookies = _httpContext.Response.Headers.SetCookie;
        cookies.Should().Contain(c => c.Contains("auth-token=jwt-token"));
        cookies.Should().Contain(c => c.Contains("preferred-units=metric"));
        cookies.Should().Contain(c => c.Contains("httponly"));
        cookies.Should().Contain(c => c.Contains("secure"));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnBadRequest_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "ValidPassword123!"
        };
        var validationResult = new ValidationResult(
            new[] { new ValidationFailure("", "Invalid email or password") });

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<LoginResponse, ValidationResult>(validationResult));

        // Act
        var result = await _controller.LoginAsync(request, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        var objectResult = result.Result as ObjectResult;
        objectResult.Value.Should().BeOfType<ValidationProblemDetails>();

        // Verify no cookies were set
        _httpContext.Response.Headers.SetCookie.Should().BeEmpty();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnOk_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!"
        };
        var expectedResponse = new LoginResponse
        {
            JWT = "jwt-token",
            PreferredUnits = "metric",
            Email = "test@example.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<LoginResponse, ValidationResult>(expectedResponse));

        // Act
        var result = await _controller.RegisterAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);

        // RegisterAsync doesn't set cookies
        _httpContext.Response.Headers.SetCookie.Should().BeEmpty();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnBadRequest_WhenUserAlreadyExists()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "existinguser",
            Email = "existing@example.com",
            Password = "ValidPassword123!"
        };
        var validationResult = new ValidationResult(
            new[] { new ValidationFailure("Email", "User with this email already exists") });

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<LoginResponse, ValidationResult>(validationResult));

        // Act
        var result = await _controller.RegisterAsync(request, CancellationToken.None);

        // Assert - используем более гибкую проверку
        result.Should().NotBeNull();

        // Получаем ObjectResult и проверяем его содержимое
        var objectResult = Assert.IsAssignableFrom<ObjectResult>(result);
        Assert.IsType<ValidationProblemDetails>(objectResult.Value);
    }

    [Fact]
    public async Task GoogleLoginAsync_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        var request = new GoogleLoginRequest
        {
            Code = "valid-code",
            CodeVerifier = "verifier"
        };
        var expectedResponse = new LoginResponse
        {
            JWT = "jwt-token",
            PreferredUnits = "metric",
            Email = "test@gmail.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GoogleLoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<LoginResponse, ValidationResult>(expectedResponse));

        // Act
        var result = await _controller.GoogleLoginAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);

        // Verify cookies were set
        var cookies = _httpContext.Response.Headers.SetCookie;
        cookies.Should().Contain(c => c.Contains("auth-token=jwt-token"));
        cookies.Should().Contain(c => c.Contains("preferred-units=metric"));
    }

    [Fact]
    public async Task GoogleRegisterAsync_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        var request = new GoogleRegisterRequest
        {
            Code = "valid-code",
            CodeVerifier = "verifier",
        };
        var expectedResponse = new LoginResponse
        {
            JWT = "jwt-token",
            PreferredUnits = "imperial",
            Email = "test@gmail.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GoogleRegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<LoginResponse, ValidationResult>(expectedResponse));

        // Act
        var result = await _controller.GoogleRegisterAsync(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);

        // Verify cookies were set
        var cookies = _httpContext.Response.Headers.SetCookie;
        cookies.Should().Contain(c => c.Contains("auth-token=jwt-token"));
        cookies.Should().Contain(c => c.Contains("preferred-units=imperial"));
    }

    [Fact]
    public async Task VerifyEmail_ShouldReturnOk_WhenTokenIsValid()
    {
        // Arrange
        var token = "valid-verification-token";
        var expectedResponse = new LoginResponse
        {
            JWT = "jwt-token",
            PreferredUnits = "metric",
            Email = "test@example.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyEmailCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<LoginResponse, ValidationResult>(expectedResponse));

        // Act
        var result = await _controller.VerifyEmail(token, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedResponse);

        // Verify cookies were set
        var cookies = _httpContext.Response.Headers.SetCookie;
        cookies.Should().Contain(c => c.Contains("auth-token=jwt-token"));
        cookies.Should().Contain(c => c.Contains("preferred-units=metric"));
    }

    [Fact]
    public async Task VerifyEmail_ShouldReturnBadRequest_WhenTokenIsEmpty()
    {
        // Act
        var result = await _controller.VerifyEmail("", CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestResult>();

        // Verify mediator was never called
        _mediatorMock.Verify(
            m => m.Send(It.IsAny<VerifyEmailCommand>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task VerifyEmail_ShouldReturnBadRequest_WhenTokenIsInvalid()
    {
        // Arrange
        var token = "invalid-token";
        var validationResult = new ValidationResult(
            new[] { new ValidationFailure("Token", "Invalid or expired token") });

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyEmailCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<LoginResponse, ValidationResult>(validationResult));

        // Act
        var result = await _controller.VerifyEmail(token, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
    }
}
