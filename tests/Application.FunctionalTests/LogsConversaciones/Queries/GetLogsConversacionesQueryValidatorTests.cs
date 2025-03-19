using System;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.LogsConversaciones.Queries
{
    [TestFixture]
    public class GetLogsConversacionesQueryValidatorTests
    {
        private GetLogsConversacionesQueryValidator _validator = null!;

        [SetUp]
        public void Setup()
        {
            _validator = new GetLogsConversacionesQueryValidator();
        }

        [Test]
        public void Should_Have_Error_When_PartitionKey_Is_Empty()
        {
            // Arrange
            var query = new GetLogsConversacionesQuery("", null, null, null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PartitionKey" &&
                                                 e.ErrorMessage.Contains("obligatorio"));
        }

        [Test]
        public void Should_Have_Error_When_PartitionKey_Is_Too_Short()
        {
            // Arrange: usamos una cadena de 6 caracteres, ya que MinimumLength(7) requiere al menos 7 caracteres.
            var query = new GetLogsConversacionesQuery("123456", null, null, null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PartitionKey" &&
                                                 e.ErrorMessage.Contains("al menos 7 caracteres"));
        }


        [Test]
        public void Should_Have_Error_When_PartitionKey_Is_Not_Numeric()
        {
            // Arrange: PartitionKey con letras
            var query = new GetLogsConversacionesQuery("ABC12345", null, null, null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "PartitionKey" &&
                                                 e.ErrorMessage.Contains("únicamente números"));
        }

        [Test]
        public void Should_Not_Have_Error_For_Valid_PartitionKey()
        {
            // Arrange: PartitionKey válida (solo números y con longitud suficiente)
            var query = new GetLogsConversacionesQuery("12345678", null, null, null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void Should_Have_Error_When_FechaDesde_Format_Is_Invalid()
        {
            // Arrange: FechaDesde con formato inválido
            var query = new GetLogsConversacionesQuery("12345678", "invalid-date", null, null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FechaDesde" &&
                                                 e.ErrorMessage.Contains("incorrecto"));
        }

        [Test]
        public void Should_Have_Error_When_FechaHasta_Format_Is_Invalid()
        {
            // Arrange: FechaHasta con formato inválido
            var query = new GetLogsConversacionesQuery("12345678", null, "invalid-date", null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "FechaHasta" &&
                                                 e.ErrorMessage.Contains("incorrecto"));
        }

        [Test]
        public void Should_Have_Error_When_FechaDesde_Is_Greater_Than_FechaHasta()
        {
            // Arrange: Fechas válidas pero en orden incorrecto (FechaDesde > FechaHasta)
            var query = new GetLogsConversacionesQuery("12345678", "2025-02-20T10:00:00", "2025-02-19T10:00:00", null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("La fechaDesde no puede ser mayor que la fechaHasta"));
        }

        [Test]
        public void Should_Not_Have_Error_When_FechaRange_Is_Valid()
        {
            // Arrange: Fechas válidas en orden correcto
            var query = new GetLogsConversacionesQuery("12345678", "2025-02-19T10:00:00", "2025-02-20T10:00:00", null, 1, 10);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
