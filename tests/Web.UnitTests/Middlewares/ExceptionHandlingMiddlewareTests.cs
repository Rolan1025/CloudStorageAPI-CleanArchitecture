using System.Net;
using System.Text.Json;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Exceptions;
using CloudStorageAPICleanArchitecture.Web.Middlewares;
using CloudStorageAPICleanArchitecture.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Linq;

namespace Web.UnitTests.Middlewares
{
    [TestFixture]
    public class ExceptionHandlingMiddlewareTests
    {
        private ExceptionHandlingMiddleware _middleware = null!;
        private DefaultHttpContext _httpContext = null!;

        [SetUp]
        public void Setup()
        {
            // Inicializamos el middleware con un RequestDelegate dummy (que luego se reemplazará en cada test)
            RequestDelegate next = context => Task.CompletedTask;
            _middleware = new ExceptionHandlingMiddleware(next, NullLogger<ExceptionHandlingMiddleware>.Instance);
            _httpContext = new DefaultHttpContext();
            // Aseguramos que la respuesta se escriba en un MemoryStream para poder leerla después.
            _httpContext.Response.Body = new MemoryStream();
        }

        /// <summary>
        /// Método auxiliar para invocar el middleware con una excepción.
        /// Se reemplaza el RequestDelegate para lanzar la excepción especificada.
        /// </summary>
        /// <param name="ex">La excepción a lanzar.</param>
        /// <returns>El ErrorResponse deserializado.</returns>
        private async Task<ErrorResponse> InvokeMiddlewareWithException(Exception ex)
        {
            // Reemplazamos el middleware con un RequestDelegate que lanza la excepción
            _middleware = new ExceptionHandlingMiddleware(ctx => throw ex, NullLogger<ExceptionHandlingMiddleware>.Instance);
            await _middleware.Invoke(_httpContext);
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var json = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return errorResponse!;
        }

        [Test]
        public async Task Invoke_WithValidationException_ReturnsBadRequest()
        {
            // Arrange: Creamos una ValidationException con un error de validación.
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Field", "Error message")
            };
            var validationException = new CloudStorageAPICleanArchitecture.Application.Common.Exceptions.ValidationException(validationFailures);

            // Act
            var errorResponse = await InvokeMiddlewareWithException(validationException);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            errorResponse.Message.Should().Be("Error de validación en la solicitud.");
            errorResponse.Errors.Should().Contain("Error message");
        }

        [Test]
        public async Task Invoke_WithUnauthorizedAccessException_ReturnsUnauthorized()
        {
            // Arrange
            var ex = new UnauthorizedAccessException("Unauthorized access");

            // Act
            var errorResponse = await InvokeMiddlewareWithException(ex);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
            errorResponse.Message.Should().Be("No tienes permisos para acceder a este recurso.");
        }

        [Test]
        public async Task Invoke_WithKeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            var ex = new KeyNotFoundException("Resource not found");

            // Act
            var errorResponse = await InvokeMiddlewareWithException(ex);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            errorResponse.Message.Should().Be("El recurso solicitado no fue encontrado.");
        }

        [Test]
        public async Task Invoke_WithApplicationException_ReturnsBadRequest()
        {
            // Arrange
            var ex = new ApplicationException("Bad request error");

            // Act
            var errorResponse = await InvokeMiddlewareWithException(ex);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            errorResponse.Message.Should().Be("Bad request error");
        }

        [Test]
        public async Task Invoke_WithUnknownException_ReturnsInternalServerError()
        {
            // Arrange
            var ex = new Exception("Unknown error");

            // Act
            var errorResponse = await InvokeMiddlewareWithException(ex);

            // Assert
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            errorResponse.Should().NotBeNull();
            errorResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            errorResponse.Message.Should().Be("Ocurrió un error inesperado en la API.");
        }
    }
}
