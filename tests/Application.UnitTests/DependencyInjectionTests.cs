using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using NUnit.Framework;
using System.Linq;
using CloudStorageAPICleanArchitecture.Application.Common.Mappings; // Asegúrate de incluir la extensión
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries; // Para GetLogsConversacionesQueryValidator

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.DependencyInjectionTests
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        [Test]
        public void AddApplicationServices_RegistersExpectedServices()
        {
            // Arrange: Creamos un WebApplicationBuilder (implementa IHostApplicationBuilder)
            var builder = WebApplication.CreateBuilder();

            // Act: Llamamos al método de extensión para registrar los servicios
            builder.AddApplicationServices();

            // Assert: Verificamos que algunos servicios se han registrado en la colección.
            var services = builder.Services;

            // Verifica que AutoMapper esté registrado.
            services.Any(s => s.ServiceType == typeof(AutoMapper.IMapper)).Should().BeTrue("porque se debe registrar AutoMapper.");

            // Verifica que MediatR esté registrado.
            services.Any(s => s.ServiceType == typeof(IMediator)).Should().BeTrue("porque se debe registrar MediatR.");

            // Verifica que se hayan registrado comportamientos de pipeline.
            services.Any(s => s.ServiceType.IsGenericType && s.ServiceType.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
                .Should().BeTrue("porque se deben registrar los pipeline behaviors (UnhandledExceptionBehaviour, AuthorizationBehaviour, etc.).");

            // Verifica que se hayan registrado validadores (por ejemplo, GetLogsConversacionesQueryValidator).
            services.Any(s => s.ImplementationType != null
                && s.ImplementationType.Assembly == typeof(GetLogsConversacionesQueryValidator).Assembly)
                .Should().BeTrue("porque se deben registrar los validadores desde el ensamblado.");
        }
    }
}
