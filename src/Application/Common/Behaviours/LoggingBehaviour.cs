using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace CloudStorageAPICleanArchitecture.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ITokenInfo _tokenInfo;

    public LoggingBehaviour(ILogger<TRequest> logger, ITokenInfo tokenInfo)
    {
        _logger = logger;
        _tokenInfo = tokenInfo;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        // 🔹 Si hay un token, mostrar solo una parte o el claim JTI en los logs
        var maskedToken = _tokenInfo.RawToken?.Length > 10
            ? $"{_tokenInfo.RawToken.Substring(0, 5)}...{_tokenInfo.RawToken[^5..]}"  // Mostrar solo los primeros y últimos 5 caracteres
            : "Sin token";

        _logger.LogInformation("📌 ChatBotAPI Request: {RequestName} | Token (parcial): {MaskedToken} | Detalles: {@Request}",
            requestName, maskedToken, request);

        return Task.CompletedTask;
    }
}
