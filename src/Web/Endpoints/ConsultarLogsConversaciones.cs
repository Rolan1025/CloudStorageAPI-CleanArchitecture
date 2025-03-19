using CloudStorageAPICleanArchitecture.Application.Common.Models;
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;
using CloudStorageAPICleanArchitecture.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations; // ✅ Importar las anotaciones de Swashbuckle

namespace CloudStorageAPICleanArchitecture.Web.Endpoints
{
    /// <summary>
    /// Controlador para gestionar logs de conversaciones almacenados en Table Storage.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultarLogsConversaciones : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultarLogsConversaciones(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Obtiene los logs de conversaciones filtrados.
        /// </summary>
        /// <remarks>
        /// Este endpoint recupera los logs de conversaciones según los filtros proporcionados.
        /// </remarks>
        /// <param name="partitionKey">Documento de identidad del usuario (Obligatorio).</param>
        /// <param name="fechaDesde">Fecha de inicio en formato ISO 8601 (yyyy-MM-ddTHH:mm:ss).</param>
        /// <param name="fechaHasta">Fecha de fin en formato ISO 8601 (yyyy-MM-ddTHH:mm:ss).</param>
        /// <param name="channel">Canal de comunicación (Ejemplo: "WHATSAPP").</param>
        /// <param name="pageNumber">Número de página para la paginación. Si no se envía, devuelve todos los registros.</param>
        /// <param name="pageSize">Tamaño de página para la paginación. Si no se envía, devuelve todos los registros.</param>
        /// <returns>Lista paginada de logs de conversaciones.</returns>
        [HttpGet("ConsultarLogsConversaciones")]
        [SwaggerOperation(
            Summary = "Obtiene los logs de conversaciones filtrados",
            Description = "Este endpoint recupera los logs de conversaciones según los filtros proporcionados, como el documento de identidad, fechas y canal.",
            OperationId = "GetLogsConversaciones"
        )]
        [SwaggerResponse(200, "Registros obtenidos correctamente", typeof(PaginatedList<LogsConversacionDto>))]
        [SwaggerResponse(400, "Solicitud inválida. Verifica los parámetros enviados.", typeof(ErrorResponse))]
        [SwaggerResponse(401, "No autorizado. Falta el token de autenticación.", typeof(ErrorResponse))]
        [SwaggerResponse(403, "Acceso prohibido. No tienes permisos para esta acción.", typeof(ErrorResponse))]
        [SwaggerResponse(404, "No encontrado. No se encontraron registros con los criterios especificados.", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Error interno del servidor. Ocurrió un problema inesperado.", typeof(ErrorResponse))]
        public async Task<IActionResult> GetLogsConversaciones(
            [FromQuery, SwaggerParameter("Documento de identidad del usuario. Es obligatorio.")] string partitionKey,
            [FromQuery, SwaggerParameter("Fecha de inicio en formato ISO 8601 (yyyy-MM-ddTHH:mm:ss). Ejemplo: 2025-02-19T16:05:58")] string? fechaDesde = null,
            [FromQuery, SwaggerParameter("Fecha de fin en formato ISO 8601 (yyyy-MM-ddTHH:mm:ss). Ejemplo: 2025-02-19T16:07:00")] string? fechaHasta = null,
            [FromQuery, SwaggerParameter("Canal de comunicación. Ejemplo: 'WHATSAPP'")] string? channel = null,
            [FromQuery, SwaggerParameter("Número de página para la paginación. Si no se envía, devuelve todos los registros.")] int? pageNumber = null,
            [FromQuery, SwaggerParameter("Tamaño de página para la paginación. Si no se envía, devuelve todos los registros.")] int? pageSize = null
        )
        {
            var query = new GetLogsConversacionesQuery(partitionKey, fechaDesde, fechaHasta, channel, pageNumber, pageSize);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
