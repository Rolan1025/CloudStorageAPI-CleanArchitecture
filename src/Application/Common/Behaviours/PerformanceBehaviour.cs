using System.Diagnostics;
using Microsoft.Extensions.Logging;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace CloudStorageAPICleanArchitecture.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly ITokenInfo _tokenInfo;

    public PerformanceBehaviour(ILogger<TRequest> logger, ITokenInfo tokenInfo)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _tokenInfo = tokenInfo;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        // 🔹 Enmascarar el token antes de registrarlo
        var maskedToken = _tokenInfo.RawToken?.Length > 10
            ? $"{_tokenInfo.RawToken.Substring(0, 5)}...{_tokenInfo.RawToken[^5..]}"  // Mostrar solo los primeros y últimos 5 caracteres
            : "Sin token";

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogWarning("⚠️ ChatBotAPI Long Running Request: {RequestName} | Token (parcial): {MaskedToken} | Tiempo: {ElapsedMilliseconds}ms | Detalles: {@Request}",
                requestName, maskedToken, elapsedMilliseconds, request);
        }

        return response;
    }
}
