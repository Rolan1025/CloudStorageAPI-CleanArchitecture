using System.Text.Json;
using CloudStorageAPICleanArchitecture.Application.Common.Exceptions;
using CloudStorageAPICleanArchitecture.Web.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using System.IO;
using Ardalis.GuardClauses;

namespace Web.UnitTests.Infrastructure
{
    [TestFixture]
    public class CustomExceptionHandlerTests
    {
        private CustomExceptionHandler _exceptionHandler = null!;
        private DefaultHttpContext _httpContext = null!;

        [SetUp]
        public void Setup()
        {
            _exceptionHandler = new CustomExceptionHandler();
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
        }

        [Test]
        public async Task TryHandleAsync_WithValidationException_ShouldReturnBadRequest()
        {
            var exception = new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new("Property1", "Error message")
            });

            var result = await _exceptionHandler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            result.Should().BeTrue();
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            responseBody.Should().Contain("Error message");
        }

        [Test]
        public async Task TryHandleAsync_WithNotFoundException_ShouldReturnNotFound()
        {
            // 🔹 Se proporciona el key y el nombre del objeto como lo requiere Ardalis.GuardClauses
            var exception = new NotFoundException("123", "Resource");

            var result = await _exceptionHandler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            result.Should().BeTrue();
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            responseBody.Should().Contain("Queried object Resource was not found, Key: 123");
        }


        [Test]
        public async Task TryHandleAsync_WithUnauthorizedAccessException_ShouldReturnUnauthorized()
        {
            var exception = new UnauthorizedAccessException("Unauthorized access");

            var result = await _exceptionHandler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            result.Should().BeTrue();
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            responseBody.Should().Contain("Unauthorized access");
        }

        [Test]
        public async Task TryHandleAsync_WithForbiddenAccessException_ShouldReturnForbidden()
        {
            var exception = new ForbiddenAccessException(); // ✅ No se pasa argumento

            var result = await _exceptionHandler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            result.Should().BeTrue();
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);

            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(_httpContext.Response.Body).ReadToEndAsync();

            // 🔹 Verificar si el JSON generado tiene la estructura esperada
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseBody);

            problemDetails.Should().NotBeNull();
            problemDetails!.Status.Should().Be(StatusCodes.Status403Forbidden);
            problemDetails.Title.Should().Be("Forbidden");
            problemDetails.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.3");
        }


        [Test]
        public async Task TryHandleAsync_WithUnknownException_ShouldReturnFalse()
        {
            var exception = new Exception("Unknown error");

            var result = await _exceptionHandler.TryHandleAsync(_httpContext, exception, CancellationToken.None);

            result.Should().BeFalse();
            _httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.OK); // No cambia el código de estado
        }



    }
}
