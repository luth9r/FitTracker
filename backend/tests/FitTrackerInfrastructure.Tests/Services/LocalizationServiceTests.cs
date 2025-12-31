using FitTracker.Application.Interfaces;
using FitTracker.Infrastructure.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace FitTrackerInfrastructure.Tests.Services
{
    public sealed class LocalizationServiceTests
    {
        private readonly Mock<ILocalizationProvider> _providerMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<ILogger<LocalizationService>> _loggerMock = new();

        [Fact]
        public void GetAvailableCultures_ShouldReturnCulturesFromProvider()
        {
            // Arrange
            var expectedCultures = new[] { "en-US", "uk-UA" };
            _providerMock.Setup(x => x.GetAvailableCultures()).Returns(expectedCultures);

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetAvailableCultures();

            // Assert
            result.Should().BeEquivalentTo(expectedCultures);
            _providerMock.Verify(x => x.GetAvailableCultures(), Times.Once);
        }

        [Fact]
        public void GetString_WithKey_ShouldUseCurrentCultureFromHeader()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = "uk-UA";

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _providerMock.Setup(x => x.GetString("welcome", "uk-UA")).Returns("Ласкаво просимо");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("welcome");

            // Assert
            result.Should().Be("Ласкаво просимо");
            _providerMock.Verify(x => x.GetString("welcome", "uk-UA"), Times.Once);
        }

        [Fact]
        public void GetString_WithKeyAndCulture_ShouldUseSpecifiedCulture()
        {
            // Arrange
            _providerMock.Setup(x => x.GetString("goodbye", "de-DE")).Returns("Auf Wiedersehen");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("goodbye", "de-DE");

            // Assert
            result.Should().Be("Auf Wiedersehen");
            _providerMock.Verify(x => x.GetString("goodbye", "de-DE"), Times.Once);
        }

        [Fact]
        public void GetString_WithNullHttpContext_ShouldUseDefaultCulture()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(default(HttpContext));
            _providerMock.Setup(x => x.GetString("hello", "en-US")).Returns("Hello");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("hello");

            // Assert
            result.Should().Be("Hello");
            _providerMock.Verify(x => x.GetString("hello", "en-US"), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("HttpContext is null")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void GetString_WithAcceptLanguageWithQuality_ShouldParseCorrectly()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = "uk-UA;q=0.9,en-US;q=0.8";

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _providerMock.Setup(x => x.GetString("test", "uk-UA")).Returns("Тест");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("Тест");
            _providerMock.Verify(x => x.GetString("test", "uk-UA"), Times.Once);
        }

        [Fact]
        public void GetString_WithMultipleLanguagesInHeader_ShouldUseFirst()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = "fr-FR,en-US,de-DE";

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _providerMock.Setup(x => x.GetString("test", "fr-FR")).Returns("Test");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("Test");
            _providerMock.Verify(x => x.GetString("test", "fr-FR"), Times.Once);
        }

        [Fact]
        public void GetString_WithEmptyAcceptLanguageHeader_ShouldUseSystemCulture()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = string.Empty;

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var systemCulture = System.Globalization.CultureInfo.CurrentCulture.Name;
            _providerMock.Setup(x => x.GetString("test", systemCulture)).Returns("Test");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("Test");
            _providerMock.Verify(x => x.GetString("test", systemCulture), Times.Once);
        }

        [Fact]
        public void GetString_WithMissingAcceptLanguageHeader_ShouldUseSystemCulture()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // No Accept-Language header
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var systemCulture = System.Globalization.CultureInfo.CurrentCulture.Name;
            _providerMock.Setup(x => x.GetString("test", systemCulture)).Returns("Test");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("Test");
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Using system culture")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void GetString_WithWhitespaceInHeader_ShouldHandleCorrectly()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = "  en-US  ";

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _providerMock.Setup(x => x.GetString("test", "en-US")).Returns("Test");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("Test");
            _providerMock.Verify(x => x.GetString("test", "en-US"), Times.Once);
        }

        [Fact]
        public void GetString_WhenExceptionOccurs_ShouldUseDefaultCulture()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Throws(new InvalidOperationException("Test exception"));

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            _providerMock.Setup(x => x.GetString("test", "en-US")).Returns("Test");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("Test");
            _providerMock.Verify(x => x.GetString("test", "en-US"), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error determining current culture")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public void GetString_WithComplexAcceptLanguageHeader_ShouldParseCorrectly()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept-Language"] = "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7";

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _providerMock.Setup(x => x.GetString("test", "zh-CN")).Returns("测试");

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result = service.GetString("test");

            // Assert
            result.Should().Be("测试");
            _providerMock.Verify(x => x.GetString("test", "zh-CN"), Times.Once);
        }

        [Fact]
        public void GetAvailableCultures_WhenCalledMultipleTimes_ShouldReturnConsistentResults()
        {
            // Arrange
            var cultures = new[] { "en-US", "uk-UA" };
            _providerMock.Setup(x => x.GetAvailableCultures()).Returns(cultures);

            var service = new LocalizationService(
                _providerMock.Object,
                _httpContextAccessorMock.Object,
                _loggerMock.Object);

            // Act
            var result1 = service.GetAvailableCultures();
            var result2 = service.GetAvailableCultures();

            // Assert
            result1.Should().BeEquivalentTo(result2);
            _providerMock.Verify(x => x.GetAvailableCultures(), Times.Exactly(2));
        }
    }
}
