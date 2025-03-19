using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Web.FunctionalTests.Endpoints
{
    [TestFixture]
    public class AuthControllerIntegrationTests
    {
        private CustomWebApplicationFactory _factory = null!;
        private HttpClient _client = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task GenerateToken_WithValidApiKey_ShouldReturnToken()
        {
            // Arrange: Agrega el header "ApiKey" con el valor "valid-api-key"
            _client.DefaultRequestHeaders.Add("ApiKey", "valid-api-key");

            // Act: Realiza la llamada POST al endpoint
            var response = await _client.PostAsync("/api/Auth/token", null);

            // Assert: Verifica que la respuesta sea 200 OK y contenga el token esperado.
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            tokenResponse.Should().NotBeNull();
            tokenResponse!.Token.Should().Be("fake-jwt-token");
        }

        [Test]
        public async Task GenerateToken_WithInvalidApiKey_ShouldReturnUnauthorized()
        {
            // Arrange: Establece el header "ApiKey" con valor "invalid-api-key"
            _client.DefaultRequestHeaders.Remove("ApiKey");
            _client.DefaultRequestHeaders.Add("ApiKey", "invalid-api-key");

            // Act
            var response = await _client.PostAsync("/api/Auth/token", null);

            // Assert: La respuesta debe ser 401 Unauthorized y el error debe indicar falta de permisos.
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Contain("No tienes permisos");
        }

        [Test]
        public async Task GenerateToken_WithMissingApiKey_ShouldReturnUnauthorized()
        {
            // Arrange: No se agrega el header "ApiKey"
            _client.DefaultRequestHeaders.Remove("ApiKey");

            // Act
            var response = await _client.PostAsync("/api/Auth/token", null);

            // Assert: Se espera un 401 Unauthorized
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Contain("No tienes permisos para acceder a este recurso.");
        }



        [Test]
        public async Task GenerateToken_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange: Para provocar error, se envía "error" como apiKey
            _client.DefaultRequestHeaders.Remove("ApiKey");
            _client.DefaultRequestHeaders.Add("ApiKey", "error");

            // Act
            var response = await _client.PostAsync("/api/Auth/token", null);

            // Assert: La respuesta debe ser 500 Internal Server Error y contener el mensaje adecuado.
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            errorResponse.Should().NotBeNull();
            errorResponse!.Message.Should().Contain("Ocurrió un error inesperado");
        }
    }

    // Modelos para la prueba (deben coincidir con los definidos en la aplicación)
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorId { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public string? Details { get; set; }
    }
}
