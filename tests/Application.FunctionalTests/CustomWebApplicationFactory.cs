using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Domain.Entities;

namespace CloudStorageAPICleanArchitecture.Application.FunctionalTests
{

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly Mock<ITableStorageService> _tableStorageMock;

        public CustomWebApplicationFactory(Mock<ITableStorageService> tableStorageMock)
        {
            _tableStorageMock = tableStorageMock;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Elimina la implementación real de ITableStorageService
                services.RemoveAll<ITableStorageService>();

                // Registra nuestro mock
                services.AddScoped(_ => _tableStorageMock.Object);
            });
        }
    }

}



