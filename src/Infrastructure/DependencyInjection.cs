using System.Text;
using Azure.Data.Tables;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Infrastructure.Identity;
using CloudStorageAPICleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
        {
            var storageConnectionString = builder.Configuration.GetConnectionString("AzureTableStorage");
            Guard.Against.Null(storageConnectionString, message: "Connection string 'AzureTableStorage' not found.");

            // Configurar Azure Table Storage
            builder.Services.AddSingleton(sp =>
            {
                return new TableServiceClient(storageConnectionString);
            });

            builder.Services.AddSingleton<ITableStorageService, TableStorageService>();
            builder.Services.AddTransient<IIdentityService, IdentityService>();

            // 🔹 Configurar Autenticación con JWT
            var jwtKey = builder.Configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                jwtKey = Environment.GetEnvironmentVariable("Jwt__Key"); // 🔹 Si no está en configuración, buscar en variables de entorno
            }

            if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
            {
                throw new ArgumentException("JWT key no está configurada o es demasiado corta. Defina 'Jwt:Key' en appsettings.json o la variable de entorno 'Jwt__Key'.");
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            builder.Services.AddAuthorizationBuilder();
            builder.Services.AddSingleton(TimeProvider.System);
        }
    }
}
