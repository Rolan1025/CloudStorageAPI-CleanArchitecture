using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.DependencyInjectionTests
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        private WebApplicationBuilder CreateBuilderWithConfiguration(Action<IConfigurationBuilder>? configure = null)
        {
            // Usamos WebApplication.CreateBuilder() para obtener un builder que implemente IHostApplicationBuilder.
            var builder = WebApplication.CreateBuilder();
            // Opcional: agregar configuración adicional.
            configure?.Invoke(builder.Configuration as IConfigurationBuilder);
            return builder;
        }

        [Test]
        public void AddWebServices_RegistersExpectedServices()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();

            // Act: invocamos el método de extensión para registrar los servicios de la capa Web.
            builder.AddWebServices();

            // Assert: verificamos que se hayan registrado algunos servicios clave.
            // 1. ITokenInfo debe estar registrado con la implementación CurrentTokenInfo.
            var tokenInfoDescriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(ITokenInfo));
            tokenInfoDescriptor.Should().NotBeNull("porque se debe registrar ITokenInfo");
            tokenInfoDescriptor!.ImplementationType.Should().Be(typeof(CurrentTokenInfo));

            // 2. ApiBehaviorOptions se debe haber configurado para suprimir el filtro por defecto.
            var optionsProvider = builder.Services.BuildServiceProvider();
            var apiBehaviorOptions = optionsProvider.GetRequiredService<IOptions<ApiBehaviorOptions>>().Value;
            apiBehaviorOptions.SuppressModelStateInvalidFilter.Should().BeTrue("porque se configuró para suprimir el filtro");

            // 3. SwaggerGen se debe haber agregado (verificamos que se registre el servicio de SwaggerGen).
            builder.Services.Any(s => s.ServiceType == typeof(ISwaggerProvider))
                .Should().BeTrue("porque se debe registrar SwaggerGen");
        }

        [Test]
        public void AddKeyVaultIfConfigured_WithEmptyEndpoint_DoesNotThrow()
        {
            // Arrange: creamos un builder y configuramos "AZURE_KEY_VAULT_ENDPOINT" vacío.
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"] = "";

            // Act & Assert: el método debe ejecutarse sin lanzar excepciones.
            Action act = () => builder.AddKeyVaultIfConfigured();
            act.Should().NotThrow();
        }

        [Test]
        public void AddKeyVaultIfConfigured_WithNonEmptyEndpoint_CatchesExceptions()
        {
            // Arrange: creamos un builder y configuramos "AZURE_KEY_VAULT_ENDPOINT" con un valor ficticio.
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"] = "https://fakevault.vault.azure.net/";

            // Act & Assert: al llamar a AddKeyVaultIfConfigured, se debería capturar (y escribir) cualquier error,
            // sin que se propague una excepción. (No podemos validar el Console, pero sí que no lance error).
            Action act = () => builder.AddKeyVaultIfConfigured();
            act.Should().NotThrow();
        }
    }
}
