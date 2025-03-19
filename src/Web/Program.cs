using CloudStorageAPICleanArchitecture.Web.Middlewares;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var isDevelopment = builder.Environment.IsDevelopment();

var loggerConfig = new LoggerConfiguration()
    .MinimumLevel.Information();

if (isDevelopment)
{
    // 🔹 En desarrollo, loggear en consola y en archivos locales
    loggerConfig.WriteTo.Console()
                .WriteTo.File("logs/logsApi-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7);
}
else
{
    var blobConnString = builder.Configuration["ConnectionStrings:AzureBlobLogs"];
    if (!string.IsNullOrWhiteSpace(blobConnString))
    {
        loggerConfig.WriteTo.AzureBlobStorage(
            connectionString: blobConnString,
            storageContainerName: "nombreblobstorage",  // Nombre del contenedor en tu cuenta de Storage donde se almacenaran los logs
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
        );
    }
}

// 🔹 Usar la configuración final de logs
Log.Logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Configure Middleware

app.UseCustomExceptionHandler();


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatBot API v1");
    c.RoutePrefix = "api"; // Para acceder a Swagger en /api
});


app.UseHsts();

app.UseAuthentication();
app.UseAuthorization();


app.UseHealthChecks("/health");

app.Map("/", () => Results.Redirect("/api"));

app.MapControllers();

app.Run();

public partial class Program { }
