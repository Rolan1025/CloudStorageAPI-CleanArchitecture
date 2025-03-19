using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using CloudStorageAPICleanArchitecture.Application.Common.Behaviours;
using CloudStorageAPICleanArchitecture.Application.Common.Exceptions;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = CloudStorageAPICleanArchitecture.Application.Common.Exceptions.ValidationException;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Behaviours
{
    [TestFixture]
    public class ValidationBehaviourTests
    {
        private Mock<IValidator<TestRequest>> _validatorMock = null!;
        private Mock<RequestHandlerDelegate<string>> _nextMock = null!;
        private ValidationBehaviour<TestRequest, string> _behaviour = null!;
        private TestRequest _request = null!;

        [SetUp]
        public void Setup()
        {
            _validatorMock = new Mock<IValidator<TestRequest>>();
            _nextMock = new Mock<RequestHandlerDelegate<string>>();
            _behaviour = new ValidationBehaviour<TestRequest, string>(new[] { _validatorMock.Object });
            _request = new TestRequest();
        }

        [Test]
        public async Task Handle_WhenValidationSucceeds_ShouldCallNext()
        {
            // Arrange
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _nextMock.Setup(n => n()).ReturnsAsync("Result");

            // Act
            var result = await _behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            result.Should().Be("Result");
            _nextMock.Verify(n => n(), Times.Once);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var validationFailure = new ValidationFailure("Field", "Field es requerido");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            Func<Task> act = async () => await _behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ValidationException>()
                .Where(e => e.Errors.ContainsKey("Field"))
                .WithMessage("*Se han producido uno o más errores de validación*");

            _nextMock.Verify(n => n(), Times.Never);
        }

        [Test]
        public async Task Handle_WhenNoValidators_ShouldCallNextDirectly()
        {
            // Arrange
            var behaviour = new ValidationBehaviour<TestRequest, string>(Array.Empty<IValidator<TestRequest>>());
            _nextMock.Setup(n => n()).ReturnsAsync("Result");

            // Act
            var result = await behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            result.Should().Be("Result");
            _nextMock.Verify(n => n(), Times.Once);
        }

        [Test]
        public void Handle_WhenMultipleValidationFailures_ShouldThrowValidationExceptionWithAllErrors()
        {
            // Arrange
            var failures = new[]
            {
                new ValidationFailure("Field1", "Field1 es requerido"),
                new ValidationFailure("Field2", "Field2 es inválido")
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            Func<Task> act = async () => await _behaviour.Handle(_request, _nextMock.Object, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<ValidationException>()
                .Where(e => e.Errors.ContainsKey("Field1") && e.Errors.ContainsKey("Field2"));

            _nextMock.Verify(n => n(), Times.Never);
        }

        #region Clases internas para pruebas

        public class TestRequest : IRequest<string> { }

        #endregion
    }
}
