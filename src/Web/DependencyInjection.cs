using Azure.Identity;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Web.Services;
using CloudStorageAPICleanArchitecture.Web.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<ITokenInfo, CurrentTokenInfo>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();


        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // 🔹 Deja esta línea si quieres que Swashbuckle explore los endpoints
        builder.Services.AddEndpointsApiExplorer();

        // 🔹 Ahora, en lugar de AddOpenApiDocument, usa AddSwaggerGen
        builder.Services.AddSwaggerGen(options =>
        {
            // Define un documento "v1"
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ChatBot API",
                Version = "v1",
                Description = "API para gestionar logs de conversaciones en ChatBot.",
                TermsOfService = new Uri("https://miapi.com/terminos"),
                Contact = new OpenApiContact
                {
                    Name = "Soporte",
                    Email = "soporte@miapi.com",
                    Url = new Uri("https://miapi.com/soporte")
                },
                License = new OpenApiLicense
                {
                    Name = "Licencia MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // 🔹 Configurar autenticación JWT en Swashbuckle
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

            options.EnableAnnotations();

            options.OperationFilter<GlobalErrorResponseOperationFilter>();
        });


    }

    public static void AddKeyVaultIfConfigured(this IHostApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            try
            {
                builder.Configuration.AddAzureKeyVault(
                    new Uri(keyVaultUri),
                    new DefaultAzureCredential());

            }
            catch (CredentialUnavailableException ex)
            {
                // Manejar el error de credenciales no disponibles
                Console.WriteLine($"Error de credenciales: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejar otros errores
                Console.WriteLine($"Error al acceder a Azure Key Vault: {ex.Message}");
            }
        }
    }

}
