using Microsoft.Extensions.Logging;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace CloudStorageAPICleanArchitecture.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ITokenInfo _tokenInfo;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger, ITokenInfo tokenInfo)
    {
        _logger = logger;
        _tokenInfo = tokenInfo;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var errorId = Guid.NewGuid().ToString(); // Identificador único

            var maskedToken = _tokenInfo.RawToken?.Length > 10
                ? $"{_tokenInfo.RawToken.Substring(0, 5)}...{_tokenInfo.RawToken[^5..]}"
                : "Sin token";

            _logger.LogError(ex, "❌ ERROR [{ErrorId}] - ChatBotAPI Request: {RequestName} | Token (parcial): {MaskedToken} | Request: {@Request} | Exception: {ExceptionMessage}",
                errorId, requestName, maskedToken, request, ex.Message);

            throw; // ✅ Relanza la excepción original en lugar de una nueva
        }
    }
}
