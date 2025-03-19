using System.Net;
using System.Text.Json;
using CloudStorageAPICleanArchitecture.Web.Models;
using FluentValidation;

namespace CloudStorageAPICleanArchitecture.Web.Middlewares;

/// <summary>
/// Middleware global para manejar excepciones y estructurar las respuestas de error.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorId = Guid.NewGuid().ToString();
        _logger.LogError(exception, "❌ ERROR [{ErrorId}] - {ExceptionMessage}", errorId, exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            ErrorId = errorId,
            Message = "Ocurrió un error inesperado en la API."
        };

        Exception actualException = exception;

        // 🔹 Si la excepción es del tipo ApplicationException, revisamos la InnerException para obtener la original.
        if (exception is ApplicationException && exception.InnerException != null)
        {
            actualException = exception.InnerException;
        }

        switch (actualException)
        {
            case CloudStorageAPICleanArchitecture.Application.Common.Exceptions.ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "Error de validación en la solicitud.";
                errorResponse.Errors = validationException.Errors.SelectMany(kvp => kvp.Value).ToList();
                break;

            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "No tienes permisos para acceder a este recurso.";
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "El recurso solicitado no fue encontrado.";
                break;

            case ApplicationException appEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = appEx.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "Ocurrió un error inesperado en la API.";
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { WriteIndented = true });
        await response.WriteAsync(result);
    }

}
