using Moq;
using FluentAssertions;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using CloudStorageAPICleanArchitecture.Application.Common.Behaviours;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Behaviours
{
    [TestFixture]
    public class LoggingBehaviourTests
    {
        private Mock<ILogger<TestRequest>> _loggerMock = null!;
        private Mock<ITokenInfo> _tokenInfoMock = null!;
        private LoggingBehaviour<TestRequest> _loggingBehaviour = null!;
        private TestRequest _request = null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TestRequest>>();
            _tokenInfoMock = new Mock<ITokenInfo>();
            _loggingBehaviour = new LoggingBehaviour<TestRequest>(_loggerMock.Object, _tokenInfoMock.Object);
            _request = new TestRequest();
        }

        [Test]
        public async Task Process_WithoutToken_LogsCorrectMessage()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.RawToken).Returns((string?)null);

            // Act
            await _loggingBehaviour.Process(_request, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Sin token")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public async Task Process_WithToken_ShouldLogInformationWithMaskedToken()
        {
            // Arrange
            const string token = "abcdefghijklmnopqrstuvwxyz1234567890";
            _tokenInfoMock.Setup(x => x.RawToken).Returns(token);

            var expectedMaskedToken = "abcde...67890";

            // Act
            await _loggingBehaviour.Process(_request, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) =>
                        state.ToString()!.Contains(expectedMaskedToken) &&
                        state.ToString()!.Contains(nameof(TestRequest))),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public async Task Process_WithShortToken_ShouldLogSinToken()
        {
            // Arrange
            var token = "short";
            _tokenInfoMock.Setup(x => x.RawToken).Returns(token);

            // Act
            await _loggingBehaviour.Process(_request, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Sin token")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        #region Test Request Class
        public class TestRequest : IRequest<string>
        {
            public string Name { get; set; } = "TestRequest";
        }
        #endregion
    }
}
