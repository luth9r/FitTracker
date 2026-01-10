using AutoMapper;
using FitTracker.Application.DTOs.Auth;
using FitTracker.Application.DTOs.Auth.Google;
using FitTracker.Application.Interfaces;
using FitTracker.Application.UseCases.User.Commands.Google;
using FitTracker.Application.UseCases.User.Handlers.Commands.Google;
using FitTracker.Domain.Abstract.Interfaces;
using FitTracker.Domain.Entities;
using FitTrackerInfrastructure.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerApplication.Tests.CommandHandlers;

public class GoogleLoginCommandHandlerTests
{
    private readonly GoogleLoginCommandHandler _handler;
    private readonly Mock<IGoogleOAuthService> _mockGoogleOAuth;
    private readonly Mock<IJwtTokenGenerator> _mockJwtService;
    private readonly Mock<ILocalizationService> _mockLocalization;
    private readonly Mock<ILogger<GoogleLoginCommandHandler>> _mockLogger;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserReadRepository> _mockUserRepo;

    public GoogleLoginCommandHandlerTests()
    {
        _mockLogger = new Mock<ILogger<GoogleLoginCommandHandler>>();
        _mockGoogleOAuth = new Mock<IGoogleOAuthService>();
        _mockUserRepo = new Mock<IUserReadRepository>();
        _mockJwtService = new Mock<IJwtTokenGenerator>();
        _mockLocalization = new Mock<ILocalizationService>();
        _mockMapper = new Mock<IMapper>();

        _handler = new GoogleLoginCommandHandler(
            _mockLogger.Object,
            _mockGoogleOAuth.Object,
            _mockUserRepo.Object,
            _mockJwtService.Object,
            _mockLocalization.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_InvalidGooglePayload_ShouldReturnValidationFailure()
    {
        // Arrange
        var command = new GoogleLoginCommand(new GoogleLoginRequest("code", "verifier"));
        var tokenResponse = new TokenResponse
        {
            AccessToken = "access_token",
            ExpiresIn = 3600,
            Scope = "openid email profile",
            TokenType = "Bearer",
            IdToken = "valid_id_token",
            RefreshToken = null!,
        };

        _mockGoogleOAuth.Setup(s => s.ExchangeCodeForTokensAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(tokenResponse);

        _mockGoogleOAuth.Setup(s => s.ValidateAsync(tokenResponse.IdToken))
            .ReturnsAsync((GoogleTokenPayload)null);

        _mockLocalization.Setup(l => l.GetString("Google.Auth.InvalidToken"))
            .Returns("Invalid payload");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Invalid payload");
    }

    [Fact]
    public async Task Handle_UserFoundByGoogleId_ShouldReturnSuccess()
    {
        // Arrange
        var user = UserTestHelper.CreateGoogleUser("googleuser@example.com");
        var command = new GoogleLoginCommand(new GoogleLoginRequest("code", "verifier"));
        var tokenResponse = new TokenResponse
        {
            AccessToken = "mock_access",
            ExpiresIn = 3600,
            Scope = "openid email profile",
            TokenType = "Bearer",
            IdToken = "mock_id_token",
            RefreshToken = null!,
        };

        var googlePayload = new GoogleTokenPayload(
            user.GoogleProviderId!,
            user.Email,
            "Google",
            "User");

        var jwtToken = "eyJhbGciOiJIUzI1NiJ9...";
        var mappedResponse = new LoginResponse(user.Username, user.Email, jwtToken);

        _mockGoogleOAuth.Setup(s => s.ExchangeCodeForTokensAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(tokenResponse);

        _mockGoogleOAuth.Setup(s => s.ValidateAsync(tokenResponse.IdToken))
            .ReturnsAsync(googlePayload);

        _mockUserRepo.Setup(r => r.GetByGoogleTokenReadonlyAsync(
                user.GoogleProviderId!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockJwtService.Setup(s => s.GenerateToken(user))
            .Returns(jwtToken);

        _mockMapper.Setup(m => m.Map<LoginResponse>(user))
            .Returns(mappedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(mappedResponse with { JWT = jwtToken });

        _mockUserRepo.Verify(
            r => r.GetByEmailReadonlyAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UserFoundByEmailFallback_ShouldReturnSuccess()
    {
        // Arrange
        var user = UserTestHelper.CreateTestUser("fallbackuser", "fallback@example.com");
        var command = new GoogleLoginCommand(new GoogleLoginRequest("code", "verifier"));
        var tokenResponse = new TokenResponse
        {
            AccessToken = "access_token",
            ExpiresIn = 3600,
            Scope = "openid profile email",
            TokenType = "Bearer",
            IdToken = "id_token",
            RefreshToken = null,
        };

        var googlePayload = new GoogleTokenPayload(
            "unknown_google_id",
            user.Email,
            "Fallback",
            "User");

        var jwtToken = "fallback_jwt_token";
        var mappedResponse = new LoginResponse(user.Username, user.Email, jwtToken);

        _mockGoogleOAuth.Setup(s => s.ExchangeCodeForTokensAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(tokenResponse);

        _mockGoogleOAuth.Setup(s => s.ValidateAsync(tokenResponse.IdToken))
            .ReturnsAsync(googlePayload);

        _mockUserRepo.Setup(r => r.GetByGoogleTokenReadonlyAsync(
                googlePayload.GoogleId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        _mockUserRepo.Setup(r => r.GetByEmailReadonlyAsync(
                user.Email,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockJwtService.Setup(s => s.GenerateToken(user))
            .Returns(jwtToken);

        _mockMapper.Setup(m => m.Map<LoginResponse>(user))
            .Returns(mappedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(mappedResponse with { JWT = jwtToken });

        _mockUserRepo.Verify(
            r => r.GetByGoogleTokenReadonlyAsync(
                googlePayload.GoogleId,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mockUserRepo.Verify(
            r => r.GetByEmailReadonlyAsync(
                user.Email,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var command = new GoogleLoginCommand(new GoogleLoginRequest("code", "verifier"));
        var tokenResponse = new TokenResponse
        {
            AccessToken = "access",
            ExpiresIn = 3600,
            Scope = "scope",
            TokenType = "Bearer",
            IdToken = "id_token",
            RefreshToken = null,
        };

        var googlePayload = new GoogleTokenPayload("unknown", "unknown@example.com", "Unknown", "User");

        _mockGoogleOAuth.Setup(s => s.ExchangeCodeForTokensAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(tokenResponse);

        _mockGoogleOAuth.Setup(s => s.ValidateAsync(tokenResponse.IdToken)).ReturnsAsync(googlePayload);

        _mockUserRepo.Setup(r => r.GetByGoogleTokenReadonlyAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        _mockUserRepo.Setup(r => r.GetByEmailReadonlyAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);

        _mockLocalization.Setup(l => l.GetString("Google.Auth.NotFound"))
            .Returns("User not found");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Errors.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("User not found");
    }
}
