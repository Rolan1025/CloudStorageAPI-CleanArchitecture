using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations; // ✅ Importar Swashbuckle

namespace CloudStorageAPICleanArchitecture.Web.Controllers
{
    /// <summary>
    /// Controlador de autenticación para la generación de tokens JWT.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Genera un token de autenticación JWT.
        /// </summary>
        /// <remarks>
        /// Se requiere una **API Key válida** en el encabezado de la solicitud para obtener un token JWT.
        /// </remarks>
        /// <param name="apiKey">Clave de autenticación API proporcionada en el encabezado de la solicitud.</param>
        /// <returns>Token JWT en formato JSON.</returns>
        /// <response code="200">Token generado exitosamente.</response>
        /// <response code="400">Solicitud inválida. API Key faltante o incorrecta.</response>
        /// <response code="401">No autorizado. La API Key es inválida.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost("token")]
        [AllowAnonymous] // Permitir acceso anónimo a este endpoint
        [SwaggerOperation(
            Summary = "Genera un token JWT",
            Description = "Autentica la API Key y devuelve un token JWT para acceder a los endpoints protegidos."
        )]
        [SwaggerResponse(200, "Token generado exitosamente.", typeof(TokenResponse))]
        [SwaggerResponse(400, "Solicitud inválida. API Key faltante o incorrecta.", typeof(ErrorResponse))]
        [SwaggerResponse(401, "No autorizado. La API Key es inválida.", typeof(ErrorResponse))]
        [SwaggerResponse(500, "Error interno del servidor.", typeof(ErrorResponse))]
        public async Task<IActionResult> GenerateToken(
            [FromHeader(Name = "ApiKey"), SwaggerParameter("Clave de autenticación API.", Required = true)] string apiKey)
        {
            var token = await _identityService.GenerateJwtTokenAsync(apiKey);
            return Ok(new TokenResponse { Token = token });
        }
    }

    /// <summary>
    /// Representa la respuesta de autenticación con el token JWT.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Token de autenticación JWT.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
