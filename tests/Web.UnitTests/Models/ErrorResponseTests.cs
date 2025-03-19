using FluentAssertions;
using CloudStorageAPICleanArchitecture.Web.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace Web.UnitTests.Models
{
    [TestFixture]
    public class ErrorResponseTests
    {
        [Test]
        public void DefaultConstructor_ShouldInitializeProperties()
        {
            // Arrange & Act: Crear una instancia usando el constructor por defecto
            var errorResponse = new ErrorResponse();

            // Assert: Verificar que las propiedades tengan los valores por defecto
            errorResponse.StatusCode.Should().Be(0, "porque no se asigna ningún valor por defecto a StatusCode");
            errorResponse.Message.Should().BeEmpty("porque Message se inicializa como cadena vacía");
            errorResponse.ErrorId.Should().BeEmpty("porque ErrorId se inicializa como cadena vacía");
            errorResponse.Errors.Should().BeNull("porque Errors no se inicializa");
            errorResponse.Details.Should().BeNull("porque Details no se inicializa");
        }

        [Test]
        public void CanSetAndGetProperties()
        {
            // Arrange: Crear una instancia
            var errorResponse = new ErrorResponse();

            // Act: Asignar valores a las propiedades
            errorResponse.StatusCode = 400;
            errorResponse.Message = "Bad request error";
            errorResponse.ErrorId = "ABC123";
            errorResponse.Errors = new List<string> { "Error1", "Error2" };
            errorResponse.Details = "Some technical details";

            // Assert: Verificar que se retornen los valores asignados
            errorResponse.StatusCode.Should().Be(400);
            errorResponse.Message.Should().Be("Bad request error");
            errorResponse.ErrorId.Should().Be("ABC123");
            errorResponse.Errors.Should().BeEquivalentTo(new List<string> { "Error1", "Error2" });
            errorResponse.Details.Should().Be("Some technical details");
        }
    }
}
