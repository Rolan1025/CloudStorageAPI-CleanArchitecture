namespace CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

public interface ITokenInfo
{
    /// <summary>
    /// Token JWT crudo enviado en el header "Authorization: Bearer ...".
    /// </summary>
    string? RawToken { get; }

    /// <summary>
    /// Indica si el token existe y es no vacío.
    /// </summary>
    bool HasValidToken { get; }
}
