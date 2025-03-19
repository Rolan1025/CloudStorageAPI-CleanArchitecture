using System;
using Azure.Data.Tables;
using Ardalis.GuardClauses;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Infrastructure.Identity;
using CloudStorageAPICleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace CloudStorageAPICleanArchitecture.Infrastructure.UnitTests
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        /// <summary>
        /// Crea un builder con la configuración mínima requerida para que el método AddInfrastructureServices se ejecute sin errores.
        /// </summary>
        private static IHostApplicationBuilder CreateBuilderWithValidConfiguration()
        {
            var builder = WebApplication.CreateBuilder();
            // Configurar la cadena de conexión a Table Storage (usamos un valor ficticio o "UseDevelopmentStorage=true")
            builder.Configuration["ConnectionStrings:AzureTableStorage"] = "UseDevelopmentStorage=true";
            // Configurar una clave JWT válida (al menos 32 caracteres)
            builder.Configuration["Jwt:Key"] = new string('A', 32);
            // Configurar el Issuer (se usa tanto para ValidIssuer como para ValidAudience)
            builder.Configuration["Jwt:Issuer"] = "https://example.com";
            return builder;
        }

        [Test]
        public void AddInfrastructureServices_WithValidConfiguration_RegistersExpectedServices()
        {
            // Arrange
            var builder = CreateBuilderWithValidConfiguration();

            // Act: Invocar el método de extensión para registrar los servicios de infraestructura
            builder.AddInfrastructureServices();

            // Assert: Construir el ServiceProvider y verificar que se hayan registrado los servicios esperados.
            var provider = builder.Services.BuildServiceProvider();

            // 1. TableServiceClient
            provider.GetService<TableServiceClient>().Should().NotBeNull("porque se debe registrar TableServiceClient.");

            // 2. ITableStorageService se debe registrar como TableStorageService.
            provider.GetService<ITableStorageService>().Should().BeOfType<TableStorageService>("porque ITableStorageService se implementa en TableStorageService.");

            // 3. IIdentityService se debe registrar como IdentityService.
            provider.GetService<IIdentityService>().Should().BeOfType<IdentityService>("porque IIdentityService se implementa en IdentityService.");

            // 4. Se debe haber registrado la infraestructura de autenticación.
            provider.GetService<IAuthenticationSchemeProvider>().Should().NotBeNull("porque se configura la autenticación JWT.");

            // 5. TimeProvider se debe registrar.
            provider.GetService<TimeProvider>().Should().NotBeNull("porque TimeProvider.System se registra como singleton.");
        }

        [Test]
        public void AddInfrastructureServices_WithMissingOrShortJwtKey_ThrowsArgumentException()
        {
            // Arrange: Creamos un builder con una configuración inválida para Jwt:Key (clave corta).
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["ConnectionStrings:AzureTableStorage"] = "UseDevelopmentStorage=true";
            builder.Configuration["Jwt:Key"] = "short"; // Menos de 32 caracteres
            builder.Configuration["Jwt:Issuer"] = "https://example.com";

            // Act & Assert: Se debe lanzar ArgumentException.
            Action act = () => builder.AddInfrastructureServices();
            act.Should().Throw<ArgumentException>()
                .WithMessage("JWT key no está configurada o es demasiado corta. Defina 'Jwt:Key' en appsettings.json o la variable de entorno 'Jwt__Key'.");
        }
    }
}
