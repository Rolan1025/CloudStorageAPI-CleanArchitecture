using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Web.FunctionalTests
{
    [TestFixture]
    public class ProgramTests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void Setup()
        {
            // Crea la instancia de la aplicación usando el entrypoint definido en Program.cs
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task HealthEndpoint_ReturnsOk()
        {
            // Act: Se consulta el endpoint de salud
            var response = await _client.GetAsync("/health");

            // Assert: Debe retornar 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task RootEndpoint_RedirectsToApi()
        {
            // Arrange: Crear un cliente que NO siga redirecciones automáticamente.
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act: Se consulta la raíz "/"
            var response = await client.GetAsync("/");

            // Assert: Se espera un redireccionamiento (302 Found) a "/api"
            response.StatusCode.Should().Be(HttpStatusCode.Found);
            response.Headers.Location.Should().NotBeNull("porque la respuesta debe incluir una ubicación de redirección");
            response.Headers.Location!.ToString().Should().Contain("/api");
        }


    }
}
