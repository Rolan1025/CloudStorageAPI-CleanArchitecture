using CloudStorageAPICleanArchitecture.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Exceptions
{
    [TestFixture]
    public class ValidationExceptionTests
    {
        [Test]
        public void DefaultConstructor_ShouldInitializeEmptyErrors()
        {
            // Act
            var exception = new ValidationException();

            // Assert
            exception.Message.Should().Be("One or more validation failures have occurred.");
            exception.Errors.Should().BeEmpty();
        }

        [Test]
        public void Constructor_WithValidationFailures_ShouldGroupErrorsByProperty()
        {
            // Arrange
            var failures = new List<ValidationFailure>
            {
                new("Nombre", "El nombre es requerido."),
                new("Nombre", "El nombre debe tener máximo 50 caracteres."),
                new("Edad", "La edad debe ser mayor que cero.")
            };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            exception.Message.Should().Be("One or more validation failures have occurred.");
            exception.Errors.Should().HaveCount(2);

            exception.Errors.Should().ContainKey("Nombre")
                .WhoseValue.Should().BeEquivalentTo(
                    "El nombre es requerido.",
                    "El nombre debe tener máximo 50 caracteres.");

            exception.Errors.Should().ContainKey("Edad")
                .WhoseValue.Should().BeEquivalentTo(
                    "La edad debe ser mayor que cero.");
        }

        [Test]
        public void Constructor_WithEmptyValidationFailures_ShouldHaveEmptyErrors()
        {
            // Arrange
            var failures = Enumerable.Empty<ValidationFailure>();

            // Act
            var exception = new ValidationException(failures);

            // Assert
            exception.Message.Should().Be("One or more validation failures have occurred.");
            exception.Errors.Should().BeEmpty();
        }

    }
}
