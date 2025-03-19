// tests/Web.FunctionalTests/CustomWebApplicationFactory.cs
using System.Linq;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Web.FunctionalTests.Fakes;

namespace Web.FunctionalTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remueve la implementación real de IIdentityService, si existe.
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IIdentityService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                // Registra la implementación falsa.
                services.AddSingleton<IIdentityService, FakeIdentityService>();
            });
        }
    }
}
