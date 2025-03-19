using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using CloudStorageAPICleanArchitecture.Web.Services;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace Web.UnitTests.Services
{
    [TestFixture]
    public class CurrentTokenInfoTests
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;
        private CurrentTokenInfo _currentTokenInfo = null!;

        [SetUp]
        public void Setup()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        [Test]
        public void RawToken_WhenHttpContextIsNull_ReturnsNull()
        {
            // Arrange: Simulamos que no hay HttpContext
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);
            _currentTokenInfo = new CurrentTokenInfo(_httpContextAccessorMock.Object);

            // Act
            var token = _currentTokenInfo.RawToken;

            // Assert
            token.Should().BeNull();
            _currentTokenInfo.HasValidToken.Should().BeFalse();
        }

        [Test]
        public void RawToken_WhenAuthorizationHeaderNotSet_ReturnsNull()
        {
            // Arrange: Creación de HttpContext sin el header "Authorization"
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
            _currentTokenInfo = new CurrentTokenInfo(_httpContextAccessorMock.Object);

            // Act
            var token = _currentTokenInfo.RawToken;

            // Assert
            token.Should().BeNull();
            _currentTokenInfo.HasValidToken.Should().BeFalse();
        }

        [Test]
        public void RawToken_WhenAuthorizationHeaderDoesNotStartWithBearer_ReturnsNull()
        {
            // Arrange: HttpContext con header "Authorization" que no comienza con "Bearer "
            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = "Basic abcdef";
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
            _currentTokenInfo = new CurrentTokenInfo(_httpContextAccessorMock.Object);

            // Act
            var token = _currentTokenInfo.RawToken;

            // Assert
            token.Should().BeNull();
            _currentTokenInfo.HasValidToken.Should().BeFalse();
        }

        [Test]
        public void RawToken_WhenAuthorizationHeaderIsProper_ReturnsTokenAndHasValidTokenIsTrue()
        {
            // Arrange: HttpContext con header "Authorization" correcto
            var context = new DefaultHttpContext();
            var expectedToken = "my-jwt-token";
            context.Request.Headers["Authorization"] = "Bearer " + expectedToken;
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);
            _currentTokenInfo = new CurrentTokenInfo(_httpContextAccessorMock.Object);

            // Act
            var token = _currentTokenInfo.RawToken;

            // Assert
            token.Should().Be(expectedToken);
            _currentTokenInfo.HasValidToken.Should().BeTrue();
        }
    }
}
