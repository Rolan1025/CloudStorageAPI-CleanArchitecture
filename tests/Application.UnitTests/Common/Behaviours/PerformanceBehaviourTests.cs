using CloudStorageAPICleanArchitecture.Application.Common.Behaviours;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Behaviours
{
    [TestFixture]
    public class PerformanceBehaviourTests
    {
        private Mock<ILogger<TestRequest>> _loggerMock = null!;
        private Mock<ITokenInfo> _tokenInfoMock = null!;
        private Mock<RequestHandlerDelegate<string>> _nextMock = null!;
        private PerformanceBehaviour<TestRequest, string> _performanceBehaviour = null!;
        private TestRequest _request = null!;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TestRequest>>();
            _tokenInfoMock = new Mock<ITokenInfo>();
            _nextMock = new Mock<RequestHandlerDelegate<string>>();
            _nextMock.Setup(x => x()).ReturnsAsync("TestResult");

            _request = new TestRequest();
            _performanceBehaviour = new PerformanceBehaviour<TestRequest, string>(_loggerMock.Object, _tokenInfoMock.Object);
        }

        [Test]
        public async Task Handle_WhenRequestIsFast_ShouldNotLogWarning()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.RawToken).Returns("abcdefghijklmnoprstuv12345");

            // Act
            var result = await _performanceBehaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            result.Should().Be("TestResult");
            _loggerMock.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }

        [Test]
        public async Task Handle_WhenRequestIsSlow_ShouldLogWarningWithMaskedToken()
        {
            // Arrange
            var token = "abcdefghijklmnopqrstuvwxyz1234567890";
            _tokenInfoMock.Setup(x => x.RawToken).Returns(token);

            _nextMock.Setup(x => x()).Returns(async () =>
            {
                await Task.Delay(510); // más de 500 ms
                return "TestResult";
            });

            var expectedMaskedToken = $"{token[..5]}...{token[^5..]}";

            // Act
            var result = await _performanceBehaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            result.Should().Be("TestResult");

            _loggerMock.Verify(x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) =>
                    state.ToString()!.Contains("ChatBotAPI Long Running Request") &&
                    state.ToString()!.Contains(nameof(TestRequest)) &&
                    state.ToString()!.Contains(expectedMaskedToken) &&
                    state.ToString()!.Contains("Tiempo:")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_WhenRequestIsSlowAndNoToken_ShouldLogWarningWithSinToken()
        {
            // Arrange
            _tokenInfoMock.Setup(x => x.RawToken).Returns(string.Empty);

            _nextMock.Setup(x => x()).Returns(async () =>
            {
                await Task.Delay(510); // Más de 500 ms
                return "TestResult";
            });

            // Act
            var result = await _performanceBehaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            result.Should().Be("TestResult");
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) =>
                        state.ToString()!.Contains("ChatBotAPI Long Running Request") &&
                        state.ToString()!.Contains(nameof(TestRequest)) &&
                        state.ToString()!.Contains("Sin token") &&
                        state.ToString()!.Contains("Tiempo:")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        #region Test Classes

        public class TestRequest : IRequest<string> { }

        #endregion

    }
}
