using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Azure.Data.Tables;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System.Text;

namespace CloudStorageAPICleanArchitecture.Infrastructure.UnitTests.Identity
{
    [TestFixture]
    public class IdentityServiceTests
    {
        private IConfiguration _configuration = null!;
        private ILogger<IdentityService> _logger = null!;
        private TableServiceClient _tableServiceClient = null!;
        private IIdentityService _identityService = null!;

        [SetUp]
        public void Setup()
        {
            // Usar Dictionary<string, string?> para cumplir con el requerimiento de nulabilidad.
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string?>
            {
                {"ApiKeyChatBot", "valid-api-key"},
                {"Jwt:Key", new string('A', 32)},  // 32 caracteres válidos
                {"Jwt:Issuer", "https://example.com"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Instancia dummy de TableServiceClient (cadena de conexión ficticia)
            _tableServiceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=fake;AccountKey=fake;EndpointSuffix=core.windows.net");

            // Usamos un logger nulo para evitar la salida en consola durante las pruebas
            _logger = NullLogger<IdentityService>.Instance;

            _identityService = new IdentityService(_tableServiceClient, _configuration, _logger);
        }

        [Test]
        public async Task GenerateJwtTokenAsync_WithValidApiKey_ReturnsToken()
        {
            // Arrange
            var apiKey = "valid-api-key";

            // Act
            var tokenString = await _identityService.GenerateJwtTokenAsync(apiKey);

            // Assert
            tokenString.Should().NotBeNullOrEmpty();
            var handler = new JwtSecurityTokenHandler();
            handler.CanReadToken(tokenString).Should().BeTrue("porque se debe generar un token JWT válido");
        }

        [Test]
        public void GenerateJwtTokenAsync_WithInvalidApiKey_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var invalidKey = "invalid-api-key";

            // Act
            Func<Task> act = async () => await _identityService.GenerateJwtTokenAsync(invalidKey);

            // Assert
            act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid API key.");
        }

        [Test]
        public void GenerateJwtTokenAsync_WithMissingConfiguredApiKey_ThrowsApplicationException()
        {
            // Arrange: configuración sin API key (cadena vacía)
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string?>
            {
                {"ApiKeyChatBot", ""},
                {"Jwt:Key", new string('A', 32)},
                {"Jwt:Issuer", "https://example.com"}
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var identityService = new IdentityService(_tableServiceClient, config, _logger);
            var apiKey = "any-key";

            // Act
            Func<Task> act = async () => await identityService.GenerateJwtTokenAsync(apiKey);

            // Assert
            act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("API key is not configured.");
        }

        [Test]
        public void GenerateJwtTokenAsync_WithShortJwtKey_ThrowsApplicationException()
        {
            // Arrange: configuración con JWT key demasiado corta
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string?>
            {
                {"ApiKeyChatBot", "valid-api-key"},
                {"Jwt:Key", "short"}, // clave demasiado corta
                {"Jwt:Issuer", "https://example.com"}
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var identityService = new IdentityService(_tableServiceClient, config, _logger);
            var apiKey = "valid-api-key";

            // Act
            Func<Task> act = async () => await identityService.GenerateJwtTokenAsync(apiKey);

            // Assert
            act.Should().ThrowAsync<ApplicationException>()
                .WithMessage("JWT key no está configurada o es demasiado corta. It must be at least 16 characters.");
        }
    }
}
