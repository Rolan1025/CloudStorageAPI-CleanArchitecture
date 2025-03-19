using Swashbuckle.AspNetCore.Annotations;

namespace CloudStorageAPICleanArchitecture.Web.Models
{
    /// <summary>
    /// Representa la respuesta de error estándar de la API.
    /// </summary>
    [SwaggerSchema(Title = "ErrorResponse", Description = "Estructura de la respuesta en caso de error.")]
    public class ErrorResponse
    {
        /// <summary>
        /// Código de estado HTTP asociado al error.
        /// </summary>
        [SwaggerSchema(Description = "Código HTTP del error (ejemplo: 400 para solicitudes inválidas, 500 para errores internos).")]
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensaje descriptivo del error.
        /// </summary>
        [SwaggerSchema(Description = "Mensaje general del error ocurrido.")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Código único para rastrear el error en logs.
        /// </summary>
        [SwaggerSchema(Description = "Identificador único que ayuda a rastrear el error en los registros del sistema.")]
        public string ErrorId { get; set; } = string.Empty;

        /// <summary>
        /// Lista de errores detallados en caso de validaciones fallidas.
        /// </summary>
        [SwaggerSchema(Description = "Lista opcional de errores específicos en caso de validaciones fallidas.")]
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Información adicional sobre el error.
        /// </summary>
        [SwaggerSchema(Description = "Detalles técnicos adicionales del error para depuración.")]
        public string? Details { get; set; }
    }
}
