using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using CloudStorageAPICleanArchitecture.Web.Middlewares;

namespace Web.UnitTests.Middlewares
{
    [TestFixture]
    public class ExceptionHandlingExtensionsTests
    {
        private IApplicationBuilder _appBuilder;

        [SetUp]
        public void Setup()
        {
            // Creamos un ServiceProvider vacío (o con los servicios necesarios)
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            _appBuilder = new ApplicationBuilder(serviceProvider);
        }

        [Test]
        public void UseCustomExceptionHandler_ShouldReturnSameApplicationBuilder()
        {
            // Act: Llamamos a la extensión para usar el middleware de manejo de excepciones.
            var result = _appBuilder.UseCustomExceptionHandler();

            // Assert: Verificamos que el método retorna el mismo IApplicationBuilder.
            result.Should().BeSameAs(_appBuilder);
        }
    }
}
