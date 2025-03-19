using CloudStorageAPICleanArchitecture.Application.Common.Behaviours;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Behaviours
{
    [TestFixture]
    public class AuthorizationBehaviourTests
    {
        private Mock<ITokenInfo> _tokenInfoMock;
        private Mock<RequestHandlerDelegate<string>> _nextMock;
        private AuthorizationBehaviour<AuthorizedRequest, string> _authorizedBehaviour;
        private AuthorizationBehaviour<UnauthorizedRequest, string> _unauthorizedBehaviour;
        private AuthorizationBehaviour<MultipleAuthorizeRequest, string> _multipleAuthorizeBehaviour;
        private CancellationToken _cancellationToken;

        [SetUp]
        public void Setup()
        {
            _tokenInfoMock = new Mock<ITokenInfo>();
            _nextMock = new Mock<RequestHandlerDelegate<string>>();
            _nextMock.Setup(x => x()).ReturnsAsync("TestResult");
            _cancellationToken = CancellationToken.None;

            _authorizedBehaviour = new AuthorizationBehaviour<AuthorizedRequest, string>(_tokenInfoMock.Object);
            _unauthorizedBehaviour = new AuthorizationBehaviour<UnauthorizedRequest, string>(_tokenInfoMock.Object);
            _multipleAuthorizeBehaviour = new AuthorizationBehaviour<MultipleAuthorizeRequest, string>(_tokenInfoMock.Object);
        }

        [Test]
        public void Handle_WithAuthorizeAttributeAndNoValidToken_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.HasValidToken).Returns(false);

            // Act
            Func<Task> act = async () => await _authorizedBehaviour.Handle(
                new AuthorizedRequest(),
                _nextMock.Object,
                _cancellationToken);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Acceso no autorizado. Se requiere un token válido en la cabecera Authorization.");

            _nextMock.Verify(x => x(), Times.Never);
        }

        [Test]
        public async Task Handle_WithAuthorizeAttributeAndValidToken_CallsNext()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.HasValidToken).Returns(true);

            // Act
            var result = await _authorizedBehaviour.Handle(
                new AuthorizedRequest(),
                _nextMock.Object,
                _cancellationToken);

            // Assert
            result.Should().Be("TestResult");
            _nextMock.Verify(x => x(), Times.Once);
        }

        [Test]
        public async Task Handle_WithNoAuthorizeAttribute_CallsNext()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.HasValidToken).Returns(false);

            // Act
            var result = await _unauthorizedBehaviour.Handle(
                new UnauthorizedRequest(),
                _nextMock.Object,
                _cancellationToken);

            // Assert
            result.Should().Be("TestResult");
            _nextMock.Verify(x => x(), Times.Once);
        }

        [Test]
        public void Handle_WithMultipleAuthorizeAttributesAndNoValidToken_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.HasValidToken).Returns(false);

            // Act
            Func<Task> act = async () => await _multipleAuthorizeBehaviour.Handle(
                new MultipleAuthorizeRequest(),
                _nextMock.Object,
                _cancellationToken);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Acceso no autorizado. Se requiere un token válido en la cabecera Authorization.");

            _nextMock.Verify(x => x(), Times.Never);
        }

        [Test]
        public async Task Handle_ReturnsExpectedResponse()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.HasValidToken).Returns(true);
            var expectedResponse = "TestResult";

            // Act
            var result = await _authorizedBehaviour.Handle(
                new AuthorizedRequest(),
                _nextMock.Object,
                _cancellationToken);

            // Assert
            result.Should().Be(expectedResponse);
        }

        [Test]
        public void Handle_WhenNextThrowsException_PropagatesException()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.HasValidToken).Returns(true);
            var exception = new InvalidOperationException("Test exception");
            _nextMock.Setup(x => x()).ThrowsAsync(exception);

            // Act
            Func<Task> act = async () => await _authorizedBehaviour.Handle(
                new AuthorizedRequest(),
                _nextMock.Object,
                _cancellationToken);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Test exception");
        }

        #region Test Request Classes

        [Authorize]
        private class AuthorizedRequest : IRequest<string> { }

        private class UnauthorizedRequest : IRequest<string> { }

        [Authorize]
        [Authorize(Roles = "Admin")]
        private class MultipleAuthorizeRequest : IRequest<string> { }

        #endregion
    }
}
