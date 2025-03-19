using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using CloudStorageAPICleanArchitecture.Web.Models;

namespace CloudStorageAPICleanArchitecture.Web.Swagger
{
    /// <summary>
    /// Filtro que agrega las respuestas de error estándar (400, 401, 403, 404, 500) a todos los endpoints en Swagger.
    /// </summary>
    public class GlobalErrorResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
            {
                operation.Responses = new OpenApiResponses();
            }

            // Generar el esquema para ErrorResponse
            var errorSchema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResponse), context.SchemaRepository);

            var errorContent = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType { Schema = errorSchema }
            };

            // 🔹 Agregar respuestas de error comunes
            operation.Responses.TryAdd("400", new OpenApiResponse
            {
                Description = "Solicitud inválida (errores de validación u otros problemas con la entrada del cliente).",
                Content = errorContent
            });

            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "No autorizado (el usuario no tiene acceso o falta el token JWT).",
                Content = errorContent
            });

            operation.Responses.TryAdd("403", new OpenApiResponse
            {
                Description = "Prohibido (el usuario no tiene permisos para este recurso).",
                Content = errorContent
            });

            operation.Responses.TryAdd("404", new OpenApiResponse
            {
                Description = "No encontrado (el recurso solicitado no existe).",
                Content = errorContent
            });

            operation.Responses.TryAdd("500", new OpenApiResponse
            {
                Description = "Error interno del servidor.",
                Content = errorContent
            });
        }
    }
}
