using Microsoft.AspNetCore.Builder;

namespace CloudStorageAPICleanArchitecture.Web.Middlewares;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
