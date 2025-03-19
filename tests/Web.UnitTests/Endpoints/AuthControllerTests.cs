using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Web.UnitTests.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IIdentityService> _identityServiceMock = null!;
        private AuthController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _identityServiceMock = new Mock<IIdentityService>();
            _controller = new AuthController(_identityServiceMock.Object);
        }

        [Test]
        public async Task GenerateToken_WithValidApiKey_ReturnsOk()
        {
            // Arrange
            var apiKey = "valid-key";
            var expectedToken = "jwt-token-123";
            _identityServiceMock.Setup(s => s.GenerateJwtTokenAsync(apiKey))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.GenerateToken(apiKey) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var tokenResponse = result.Value as TokenResponse;
            tokenResponse.Should().NotBeNull();
            tokenResponse!.Token.Should().Be(expectedToken);
        }

        [Test]
        public void GenerateToken_WithInvalidApiKey_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var invalidKey = "invalid-key";
            _identityServiceMock.Setup(s => s.GenerateJwtTokenAsync(invalidKey))
                .ThrowsAsync(new UnauthorizedAccessException("API key inválida"));

            // Act
            Func<Task> act = async () => await _controller.GenerateToken(invalidKey);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("API key inválida");
        }

        [Test]
        public void GenerateToken_WithMissingApiKey_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            string? nullKey = null;
            _identityServiceMock.Setup(s => s.GenerateJwtTokenAsync(nullKey!))
                .ThrowsAsync(new UnauthorizedAccessException("API key inválida"));

            // Act
            Func<Task> act = async () => await _controller.GenerateToken(nullKey!);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("API key inválida");
        }

        [Test]
        public void GenerateToken_WhenServiceThrowsGenericException_ThrowsSameException()
        {
            // Arrange
            var apiKey = "valid-key";
            _identityServiceMock.Setup(s => s.GenerateJwtTokenAsync(apiKey))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            Func<Task> act = async () => await _controller.GenerateToken(apiKey);

            // Assert
            act.Should().ThrowAsync<Exception>()
                .WithMessage("Unexpected error");
        }
    }
}
