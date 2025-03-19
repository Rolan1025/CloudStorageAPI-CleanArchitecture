using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace CloudStorageAPICleanArchitecture.Application.FunctionalTests
{
    public static class Testing
    {
        private static CustomWebApplicationFactory? _factory;
        private static IServiceScopeFactory? _scopeFactory;

        public static void Initialize(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        // Ejemplo: enviar un request de MediatR dentro del scope
        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory!.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            return await mediator.Send(request);
        }

        // O si quieres hacer peticiones HTTP:
        public static HttpClient CreateClient()
        {
            return _factory!.CreateClient();
        }
    }
}




