using System.Reflection;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CloudStorageAPICleanArchitecture.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ITokenInfo _tokenInfo;

    public AuthorizationBehaviour(ITokenInfo tokenInfo)
    {
        _tokenInfo = tokenInfo;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any() && !_tokenInfo.HasValidToken)
        {
            throw new UnauthorizedAccessException("Acceso no autorizado. Se requiere un token válido en la cabecera Authorization.");
        }

        return await next();
    }
}
