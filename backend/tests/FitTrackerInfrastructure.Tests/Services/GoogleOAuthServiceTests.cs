using System.Net;
using System.Text.Json;
using FitTracker.Infrastructure.Services;
using FitTrackerInfrastructure.Tests.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace FitTrackerInfrastructure.Tests.Services;

public sealed class GoogleOAuthServiceTests
{
    private readonly FakeHttpMessageHandler _fakeHttpHandler = new();
    private readonly Mock<ILogger<GoogleOAuthService>> _loggerMock = new();

    private IConfiguration BuildConfiguration()
    {
        var configData = new Dictionary<string, string>
        {
            ["Google:ClientId"] = "test-client-id",
            ["Google:ClientSecret"] = "test-client-secret",
            ["Google:RedirectUri"] = "https://localhost/callback",
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configData!)
            .Build();
    }

    [Fact]
    public async Task ExchangeCodeForTokensAsync_WithValidCode_ShouldReturnTokens()
    {
        // Arrange
        var config = BuildConfiguration();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        var expectedResponse = new
        {
            access_token = "test-access-token",
            refresh_token = "test-refresh-token",
            id_token = "test-id-token",
            expires_in = 3600,
            token_type = "Bearer",
        };

        _fakeHttpHandler.AddResponse(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(expectedResponse));

        // Act
        var result = await service.ExchangeCodeForTokensAsync("test-code", "test-verifier");

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("test-access-token");
        result.RefreshToken.Should().Be("test-refresh-token");
        result.IdToken.Should().Be("test-id-token");
        result.ExpiresIn.Should().Be(3600);

        _fakeHttpHandler.Requests.Should().ContainSingle();
        var request = _fakeHttpHandler.Requests[0];
        request.Method.Should().Be(HttpMethod.Post);
        request.RequestUri.Should().Be("https://oauth2.googleapis.com/token");
    }

    [Fact]
    public async Task ExchangeCodeForTokensAsync_ShouldIncludeCorrectParameters()
    {
        // Arrange
        var config = BuildConfiguration();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        var expectedResponse = new
        {
            access_token = "token",
            id_token = "id",
            expires_in = 3600,
        };

        _fakeHttpHandler.AddResponse(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(expectedResponse));

        // Act
        await service.ExchangeCodeForTokensAsync("auth-code", "code-verifier-123");

        // Assert
        var request = _fakeHttpHandler.Requests[0];
        var content = await request.Content!.ReadAsStringAsync();

        content.Should().Contain("code=auth-code");
        content.Should().Contain("client_id=test-client-id");
        content.Should().Contain("client_secret=test-client-secret");
        content.Should().Contain("redirect_uri=https%3A%2F%2Flocalhost%2Fcallback");
        content.Should().Contain("grant_type=authorization_code");
        content.Should().Contain("code_verifier=code-verifier-123");
    }

    [Fact]
    public async Task ExchangeCodeForTokensAsync_WhenGoogleReturnsError_ShouldThrowException()
    {
        // Arrange
        var config = BuildConfiguration();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        var errorResponse = new
        {
            error = "invalid_grant",
            error_description = "Code was already redeemed",
        };

        _fakeHttpHandler.AddResponse(
            HttpStatusCode.BadRequest,
            JsonSerializer.Serialize(errorResponse));

        // Act
        var act = async () => await service.ExchangeCodeForTokensAsync("invalid-code", "verifier");

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Google token exchange failed:*");
    }

    [Fact]
    public async Task ExchangeCodeForTokensAsync_WithNetworkError_ShouldThrowException()
    {
        // Arrange
        var config = BuildConfiguration();
        var faultyHandler = new Mock<HttpMessageHandler>();
        faultyHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(faultyHandler.Object);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        // Act
        var act = async () => await service.ExchangeCodeForTokensAsync("code", "verifier");

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>()
            .WithMessage("Network error");
    }

    [Fact]
    public void Constructor_WithMissingConfiguration_ShouldNotThrow()
    {
        // Arrange
        var emptyConfig = new ConfigurationBuilder().Build();
        var httpClient = new HttpClient(_fakeHttpHandler);

        // Act
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, emptyConfig);

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public async Task ValidateAsync_WithMissingClientId_ShouldReturnNull()
    {
        // Arrange
        var emptyConfig = new ConfigurationBuilder().Build();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, emptyConfig);

        // Act
        var result = await service.ValidateAsync("some-token");

        // Assert
        result.Should().BeNull();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Google:ClientId not cofigured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidToken_ShouldReturnNull()
    {
        // Arrange
        var config = BuildConfiguration();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        // Act - invalid JWT будет отклонен библиотекой Google
        var result = await service.ValidateAsync("invalid-jwt-token");

        // Assert
        result.Should().BeNull();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Not valid google token")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExchangeCodeForTokensAsync_WithEmptyCode_ShouldStillSendRequest()
    {
        // Arrange
        var config = BuildConfiguration();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        _fakeHttpHandler.AddResponse(
            HttpStatusCode.BadRequest,
            JsonSerializer.Serialize(new { error = "invalid_request" }));

        // Act
        var act = async () => await service.ExchangeCodeForTokensAsync(string.Empty, "verifier");

        // Assert
        await act.Should().ThrowAsync<Exception>();
        _fakeHttpHandler.Requests.Should().ContainSingle();
    }

    [Fact]
    public async Task ExchangeCodeForTokensAsync_WithSpecialCharacters_ShouldEncodeCorrectly()
    {
        // Arrange
        var config = BuildConfiguration();
        var httpClient = new HttpClient(_fakeHttpHandler);
        var service = new GoogleOAuthService(httpClient, _loggerMock.Object, config);

        var expectedResponse = new
        {
            access_token = "token",
            id_token = "id",
            expires_in = 3600,
        };

        _fakeHttpHandler.AddResponse(
            HttpStatusCode.OK,
            JsonSerializer.Serialize(expectedResponse));

        // Act
        await service.ExchangeCodeForTokensAsync("code+with/special=chars", "verifier&test");

        // Assert
        var request = _fakeHttpHandler.Requests[0];
        var content = await request.Content!.ReadAsStringAsync();

        content.Should().Contain("code=code%2Bwith%2Fspecial%3Dchars");
        content.Should().Contain("code_verifier=verifier%26test");
    }
}
