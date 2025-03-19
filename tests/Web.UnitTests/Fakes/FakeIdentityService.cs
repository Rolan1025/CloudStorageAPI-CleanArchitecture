// tests/Web.FunctionalTests/Fakes/FakeIdentityService.cs
using System;
using System.Threading.Tasks;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace Web.FunctionalTests.Fakes
{
    public class FakeIdentityService : IIdentityService
    {
        public Task<string> GenerateJwtTokenAsync(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new UnauthorizedAccessException("API key is missing");
            }
            if (apiKey == "valid-api-key")
            {
                return Task.FromResult("fake-jwt-token");
            }
            if (apiKey == "invalid-api-key")
            {
                throw new UnauthorizedAccessException("No autorizado");
            }
            if (apiKey == "error")
            {
                throw new Exception("Unexpected error");
            }
            // Por defecto, cualquier otro apiKey es considerado inválido.
            throw new UnauthorizedAccessException("No autorizado");
        }
    }
}
