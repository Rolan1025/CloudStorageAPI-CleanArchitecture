using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Behaviours;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Behaviours
{
    [TestFixture]
    public class UnhandledExceptionBehaviourTests
    {
        private Mock<ILogger<TestRequest>> _loggerMock = null!;
        private Mock<ITokenInfo> _tokenInfoMock = null!;
        private Mock<RequestHandlerDelegate<string>> _nextMock = null!;
        private UnhandledExceptionBehaviour<TestRequest, string> _behaviour = null!;
        private TestRequest _request = null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TestRequest>>();
            _tokenInfoMock = new Mock<ITokenInfo>();
            _nextMock = new Mock<RequestHandlerDelegate<string>>();
            _request = new TestRequest();

            _behaviour = new UnhandledExceptionBehaviour<TestRequest, string>(
                _loggerMock.Object,
                _tokenInfoMock.Object);
        }

        [Test]
        public async Task Handle_WhenNoExceptionThrown_ReturnsResultWithoutLogging()
        {
            // Arrange
            _nextMock.Setup(x => x()).ReturnsAsync("Result");

            // Act
            var result = await _behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            result.Should().Be("Result");

            _loggerMock.Verify(log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }

        [Test]
        public void Handle_WhenExceptionThrown_LogsErrorAndRethrows()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception");
            _nextMock.Setup(x => x()).ThrowsAsync(exception);
            _tokenInfoMock.Setup(x => x.RawToken).Returns("abcdefghijklmnopqrstuvwxyz1234567890");

            // Act
            Func<Task> act = async () => await _behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Test exception");

            var expectedMaskedToken = "abcde...67890";

            _loggerMock.Verify(log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("❌ ERROR") &&
                    state.ToString()!.Contains(nameof(TestRequest)) &&
                    state.ToString()!.Contains(expectedMaskedToken) &&
                    state.ToString()!.Contains("Test exception")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void Handle_WhenExceptionThrownWithoutToken_LogsErrorWithSinTokenAndRethrows()
        {
            // Arrange
            var exception = new InvalidOperationException("Exception sin token");
            _nextMock.Setup(x => x()).ThrowsAsync(exception);
            _tokenInfoMock.Setup(x => x.RawToken).Returns(string.Empty);

            // Act
            Func<Task> act = async () => await _behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Exception sin token");

            _loggerMock.Verify(log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("❌ ERROR") &&
                    state.ToString()!.Contains(nameof(TestRequest)) &&
                    state.ToString()!.Contains("Sin token") &&
                    state.ToString()!.Contains("Exception sin token")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        #region Clases internas para pruebas

        public class TestRequest : IRequest<string> { }

        #endregion
    }
}
