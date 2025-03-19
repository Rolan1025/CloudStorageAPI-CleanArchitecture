<!DOCTYPE html>
<html lang="es">
<head>
  <meta charset="UTF-8">
</head>
<body>
  <h1>CloudStorageAPI-CleanArchitecture</h1>
  <p>API en .NET 9 basada en Clean Architecture, con principios SOLID y CQRS. Emplea Azure Table Storage y Blob Storage como persistencia, Azure Key Vault para la gestión de secretos, FluentValidation para validaciones, e Inyección de Dependencias (DI). Se incluyen ejemplos de logging con Serilog a archivos locales (en desarrollo) y a Blob Storage (en producción).</p>
  <p>El proyecto fue generado usando la Clean.Architecture.Solution.Template (v9.0.8) (<a href="https://github.com/jasontaylordev/CleanArchitecture">https://github.com/jasontaylordev/CleanArchitecture</a>).</p>
  <hr>

  <h2>Tabla de Contenidos</h2>
  <ol>
    <li><a href="#caracteristicas-principales">Características Principales</a></li>
    <li><a href="#requerimientos-previos">Requerimientos Previos</a></li>
    <li><a href="#estructura-del-proyecto">Estructura del Proyecto</a></li>
    <li><a href="#tecnologias-y-paquetes">Tecnologías y Paquetes</a></li>
    <li><a href="#build">Build</a></li>
    <li><a href="#run">Run</a></li>
    <li><a href="#code-styles--formatting">Code Styles &amp; Formatting</a></li>
    <li><a href="#code-scaffolding">Code Scaffolding</a></li>
    <li><a href="#configuracion-y-ejecucion-local-alternativo">Configuración y Ejecución Local (Alternativo)</a></li>
    <li><a href="#ejemplos-de-ejecucion-curl-y-swagger">Ejemplos de Ejecución (cURL y Swagger)</a></li>
    <li><a href="#pruebas-unitarias-y-cobertura">Pruebas Unitarias y Cobertura</a></li>
    <li><a href="#despliegue-en-azure">Despliegue en Azure</a></li>
    <li><a href="#contribucion">Contribución</a></li>
    <li><a href="#licencia">Licencia</a></li>
    <li><a href="#ayuda">Ayuda</a></li>
    <li><a href="#CloudStorageAPI-CleanArchitecture-extras">CloudStorageAPI-CleanArchitecture Extras</a></li>
  </ol>
  <hr>

  <h2 id="caracteristicas-principales">Características Principales</h2>
  <ul>
    <li><strong>Clean Architecture</strong>: Separación en capas (Domain, Application, Infrastructure, Web).</li>
    <li><strong>CQRS</strong> con MediatR (<a href="https://github.com/jbogard/MediatR">https://github.com/jbogard/MediatR</a>).</li>
    <li><strong>Persistencia en la nube</strong>:
      <ul>
        <li>Azure Table Storage para entidades NoSQL.</li>
        <li>Azure Blob Storage para logs en producción.</li>
      </ul>
    </li>
    <li><strong>Azure Key Vault</strong>: Protección de secretos y configuración sensible (opcional).</li>
    <li><strong>FluentValidation</strong>: Validaciones robustas de consultas y comandos (Handlers).</li>
    <li><strong>Serilog</strong>: Logging configurable en archivos locales (desarrollo) y Blob Storage (producción).</li>
    <li><strong>JWT</strong> para autenticación (endpoint AuthController para generar tokens).</li>
    <li><strong>OpenAPI/Swagger</strong>: Documentación automática de la API.</li>
  </ul>
  <hr>

  <h2 id="requerimientos-previos">Requerimientos Previos</h2>
  <ul>
    <li>.NET 9 SDK instalado localmente.</li>
    <li>Azure Subscription (si se despliega y usan recursos de Table/Blob/Key Vault).</li>
    <li>Cuenta de almacenamiento en Azure con contenedor de logs (opcional).</li>
    <li>Key Vault (opcional) si deseas almacenar tus secretos.</li>
    <li>(Opcional) EditorConfig compatible o IDE con soporte para la configuración de estilos de la plantilla.</li>
  </ul>
  <hr>

  <h2 id="estructura-del-proyecto">Estructura del Proyecto</h2>
  <pre><code>└─ src
   ├─ Domain                       # Entidades y lógica de dominio
   ├─ Application                  # Casos de uso (CQRS), Validaciones
   ├─ Infrastructure               # Implementaciones (Table Storage, Key Vault, etc.)
   └─ Web                          # Capa de presentación (Controllers, Program.cs, Middlewares)

└─ tests
   ├─ Application.UnitTests        # Pruebas unitarias de la capa Application
   ├─ Application.FunctionalTests  # Pruebas funcionales/integración
   ├─ Domain.UnitTests             # Pruebas de dominio
   ├─ Infrastructure.UnitTests     # Pruebas unitarias de la capa Infrastructure
   └─ Web.UnitTests                # Pruebas unitarias de la capa Web
  </code></pre>
  <p>Este estilo de Clean Architecture promueve un bajo acoplamiento y alta mantenibilidad.</p>
  <hr>

  <h2 id="tecnologias-y-paquetes">Tecnologías y Paquetes</h2>
  <ul>
    <li>.NET 9</li>
    <li>MediatR (CQRS)</li>
    <li>FluentValidation</li>
    <li>Azure.Data.Tables, Azure.Data.Blobs</li>
    <li>Serilog (Sinks para File y Azure Blob)</li>
    <li>Swashbuckle.AspNetCore (Swagger)</li>
  </ul>
  <hr>

  <h2 id="build">Build</h2>
  <p>Para compilar la solución:</p>
  <pre><code>dotnet build -tl</code></pre>
  <p>Se restauran y compilan los proyectos con la configuración por defecto (Release/Debug).</p>
  <hr>

  <h2 id="run">Run</h2>
  <p>Para ejecutar la aplicación Web:</p>
  <pre><code>cd ./src/Web/
dotnet watch run</code></pre>
  <p>Navega en <a href="https://localhost:5001">https://localhost:5001</a>. La aplicación se recargará automáticamente si se cambian los archivos de origen.</p>
  <hr>

  <h2 id="code-styles--formatting">Code Styles &amp; Formatting</h2>
  <p>Este proyecto incluye un <code>.editorconfig</code> que mantiene estilos de codificación consistentes para múltiples desarrolladores (basado en la plantilla de Jason Taylor). Asegúrate de que tu editor/IDE respete el <code>.editorconfig</code>.</p>
  <hr>

  <h2 id="code-scaffolding">Code Scaffolding</h2>
  <p>La plantilla de Clean Architecture permite crear comandos y queries nuevos:</p>
  <h3>Crear un Command</h3>
  <pre><code>dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int</code></pre>
  <h3>Crear un Query</h3>
  <pre><code>dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm</code></pre>
  <p>Si recibes el error: "No templates or subcommands found matching: 'ca-usecase'.", instala la plantilla y vuelve a intentarlo:</p>
  <pre><code>dotnet new install Clean.Architecture.Solution.Template::9.0.8</code></pre>
  <hr>

  <h2 id="configuracion-y-ejecucion-local-alternativo">Configuración y Ejecución Local (Alternativo)</h2>
  <ol>
    <li>
      <p>Clonar el repositorio:</p>
      <pre><code>git clone https://github.com/tuUser/CloudStorageAPI-CleanArchitecture.git
cd CloudStorageAPI-CleanArchitecture</code></pre>
    </li>
    <li>
      <p>Instalar .NET 9 (Preview):</p>
      <p><a href="https://dotnet.microsoft.com/download/dotnet/9.0">https://dotnet.microsoft.com/download/dotnet/9.0</a></p>
    </li>
    <li>
      <p>Configurar <code>appsettings.json</code>:</p>
      <ul>
        <li>Copiar <code>appsettings.json.template</code> a <code>appsettings.json</code>.</li>
        <li>Rellenar la cadena de conexión a Table Storage, Blob Storage y tu JWT Key.</li>
      </ul>
    </li>
    <li>
      <p>Restaurar paquetes y compilar:</p>
      <pre><code>dotnet restore
dotnet build</code></pre>
    </li>
    <li>
      <p>Ejecutar:</p>
      <pre><code>dotnet run --project src/Web/CloudStorageAPICleanArchitecture.Web.csproj</code></pre>
      <p>Luego abre <a href="https://localhost:5001">https://localhost:5001</a>.</p>
    </li>
  </ol>
  <hr>

  <h2 id="ejemplos-de-ejecucion-curl-y-swagger">Ejemplos de Ejecución (cURL y Swagger)</h2>
  <h3>1. Generar Token (Endpoint AuthController)</h3>
  <pre><code>curl -X POST "https://localhost:5001/api/Auth/token" \
  -H "ApiKey: MiClaveApiSuperSecreta" \
  -d ""</code></pre>
  <p>Ejemplo Respuesta (HTTP 200):</p>
  <pre><code>{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}</code></pre>
  <h3>2. Obtener Logs de Conversaciones (Endpoint ConsultarLogsConversaciones)</h3>
  <pre><code>curl -X GET "https://localhost:5001/api/ConsultarLogsConversaciones/ConsultarLogsConversaciones?partitionKey=12345678&fechaDesde=2025-02-19T16:05:58" \
  -H "Authorization: Bearer &lt;tu-JWT&gt;"</code></pre>
  <p>Ejemplo Respuesta (HTTP 200):</p>
  <pre><code>{
  "items": [
    {
      "partitionKey": "12345678",
      "rowKey": "20250303120000123",
      "channel": "WHATSAPP",
      "dateTime": "2025-03-03T12:00:00",
      ...
    }
  ],
  "pageNumber": 1,
  "totalPages": 1,
  "totalCount": 1
}</code></pre>
  <hr>

  <h2 id="pruebas-unitarias-y-cobertura">Pruebas Unitarias y Cobertura</h2>
  <ol>
    <li>
      <p>Ejecutar pruebas:</p>
      <pre><code>dotnet test</code></pre>
      <p>Las pruebas se hallan en la carpeta <code>tests/</code>.</p>
    </li>
    <li>
      <p>Cobertura de código:</p>
      <pre><code>dotnet test --collect:"XPlat Code Coverage"</code></pre>
      <p>Se generarán los archivos de cobertura en <code>TestResults/</code>. Puedes usar ReportGenerator (<a href="https://github.com/danielpalme/ReportGenerator">ReportGenerator</a>) o SonarQube para visualizarla.</p>
    </li>
  </ol>
  <hr>

  <h2 id="despliegue-en-azure">Despliegue en Azure</h2>
  <ol>
    <li>
      <p>Crear recursos en Azure (Storage Account, App Service, Key Vault, ...).</p>
    </li>
    <li>
      <p>Definir variables de entorno en App Service → Configuration → Application Settings:</p>
      <ul>
        <li>ConnectionStrings:AzureTableStorage</li>
        <li>ConnectionStrings:AzureBlobLogs</li>
        <li>AZURE_KEY_VAULT_ENDPOINT (opcional)</li>
        <li>Jwt:Key, Jwt:Issuer, etc.</li>
      </ul>
    </li>
    <li>
      <p>Publicar vía CLI o Visual Studio (o crear un pipeline de Azure DevOps).</p>
    </li>
    <li>
      <p>Ver logs en tu Blob Storage (contenedor “logs” o el que definiste) y verifica que las entidades se guarden en Table Storage.</p>
    </li>
  </ol>
  <hr>

  <h2 id="contribucion">Contribución</h2>
  <ol>
    <li>
      <p>Haz un fork de este repositorio.</p>
    </li>
    <li>
      <p>Crea una rama para tu feature o fix:</p>
      <pre><code>git checkout -b feature/my-improvement</code></pre>
    </li>
    <li>
      <p>Haz commits con mensajes claros.</p>
    </li>
    <li>
      <p>Abre un Pull Request a la rama principal.</p>
    </li>
  </ol>
  <hr>

  <h2 id="licencia">Licencia</h2>
  <p>Este proyecto está bajo la MIT License (<code>LICENSE</code>). Siéntete libre de usarlo, modificarlo y redistribuirlo, manteniendo las referencias a los autores originales si reutilizas partes significativas.</p>
  <hr>

  <h2 id="ayuda">Ayuda</h2>
  <p>Para aprender más sobre la plantilla Clean.Architecture.Solution.Template y sus capacidades:</p>
  <ul>
    <li>Visita el repositorio oficial (<a href="https://github.com/jasontaylordev/CleanArchitecture">https://github.com/jasontaylordev/CleanArchitecture</a>) de Jason Taylor.</li>
    <li>Allí encontrarás guías, foros de discusión, plantillas actualizadas y más detalles sobre la arquitectura limpia en .NET.</li>
  </ul>
  <p>Además, si tienes dudas específicas sobre .NET 9, Azure Storage o Key Vault, visita la <a href="https://docs.microsoft.com/en-us/dotnet/azure/">documentación oficial de Microsoft</a> o revisa los repositorios en GitHub de Azure.</p>
  <hr>

  <h2 id="CloudStorageAPI-CleanArchitecture-extras">CloudStorageAPI-CleanArchitecture Extras</h2>
  <p>El proyecto fue generado usando la Clean.Architecture.Solution.Template (<a href="https://github.com/jasontaylordev/CleanArchitecture">https://github.com/jasontaylordev/CleanArchitecture</a>) versión 9.0.8.</p>
  <h3>Build</h3>
  <pre><code>dotnet build -tl</code></pre>
  <h3>Run</h3>
  <pre><code>cd ./src/Web/
dotnet watch run</code></pre>
  <p>Navega a <a href="https://localhost:5001">https://localhost:5001</a>. La aplicación se recargará automáticamente si cambias algún archivo de origen.</p>
  <h3>Code Styles &amp; Formatting</h3>
  <p>La plantilla incluye <a href="https://editorconfig.org/">EditorConfig</a> para mantener estilos de codificación consistentes. El archivo <code>.editorconfig</code> define las reglas de formato aplicables a esta solución.</p>
  <h3>Code Scaffolding</h3>
  <pre><code>dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int</code></pre>
  <pre><code>dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm</code></pre>
  <p>Si ves el error "No templates or subcommands found matching: 'ca-usecase'.", instala la plantilla:</p>
  <pre><code>dotnet new install Clean.Architecture.Solution.Template::9.0.8</code></pre>
  <h3>Test</h3>
  <pre><code>dotnet test</code></pre>
  <h3>Help</h3>
  <p>Para aprender más sobre la plantilla, visita el proyecto oficial (<a href="https://github.com/jasontaylordev/CleanArchitecture">https://github.com/jasontaylordev/CleanArchitecture</a>). Allí podrás encontrar más orientación, solicitar nuevas características, reportar errores y discutir la plantilla con otros usuarios.</p>
</body>
</html>
