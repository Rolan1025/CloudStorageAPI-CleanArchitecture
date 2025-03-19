using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace CloudStorageAPICleanArchitecture.Web.Services;

public class CurrentTokenInfo : ITokenInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTokenInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Obtiene el token JWT crudo desde la cabecera "Authorization".
    /// </summary>
    public string? RawToken
    {
        get
        {
            // Leer el header "Authorization"
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            // Verificar si existe el header y si empieza con "Bearer "
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            // Retornar la parte después de "Bearer "
            return authHeader.Substring("Bearer ".Length).Trim();
        }
    }

    /// <summary>
    /// Determina si hay un token válido en la solicitud.
    /// </summary>
    public bool HasValidToken => !string.IsNullOrEmpty(RawToken);
}
