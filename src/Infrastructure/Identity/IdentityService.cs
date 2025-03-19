using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Data.Tables;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace CloudStorageAPICleanArchitecture.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityService> _logger; // ✅ Agregamos logging

        public IdentityService(TableServiceClient tableServiceClient, IConfiguration configuration, ILogger<IdentityService> logger)
        {
            _tableServiceClient = tableServiceClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateJwtTokenAsync(string apiKey)
        {
            try
            {
                ValidateApiKey(apiKey); // ✅ Separamos la validación de API Key
                var claims = CreateClaims();
                var token = GenerateToken(claims);

                return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized API key attempt: {ApiKey}", apiKey);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token");
                throw;
            }
        }

        private void ValidateApiKey(string apiKey)
        {
            var storedApiKey = _configuration["ApiKeyChatBot"];

            if (string.IsNullOrEmpty(storedApiKey))
                throw new ApplicationException("API key is not configured.");

            if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(storedApiKey, StringComparison.Ordinal))
                throw new UnauthorizedAccessException("Invalid API key.");

        }

        private JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
        {
            var jwtKey = _configuration["Jwt:Key"];
            ValidateJwtKey(jwtKey);

            if (jwtKey == null)
            {
                throw new ApplicationException("JWT key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);
        }

        private void ValidateJwtKey(string? jwtKey)
        {
            if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 16)
                throw new ApplicationException("JWT key is not configured or is too short. It must be at least 16 characters.");
        }

        private List<Claim> CreateClaims()
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "apiKeyUser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }
    }
}
