using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using NUnit.Framework;
using CloudStorageAPICleanArchitecture.Web.Models;
using CloudStorageAPICleanArchitecture.Web.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Web.UnitTests.Middlewares
{
    // Dummy SchemaGenerator que implementa la firma actualizada de ISchemaGenerator
    public class DummySchemaGenerator : ISchemaGenerator
    {
        public OpenApiSchema GenerateSchema(Type modelType, SchemaRepository schemaRepository, MemberInfo? memberInfo = null, ParameterInfo? parameterInfo = null, ApiParameterRouteInfo? routeInfo = null)
        {
            // Para la prueba, devolvemos un esquema simple con el título del tipo.
            return new OpenApiSchema { Title = modelType.Name };
        }
    }

    [TestFixture]
    public class GlobalErrorResponseOperationFilterTests
    {
        private GlobalErrorResponseOperationFilter _filter = null!;
        private OperationFilterContext _context = null!;
        private OpenApiOperation _operation = null!;

        [SetUp]
        public void Setup()
        {
            _filter = new GlobalErrorResponseOperationFilter();

            // Creamos una instancia mínima de ApiDescription para el contexto.
            var apiDescription = new Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription();

            // Creamos el SchemaRepository y el DummySchemaGenerator.
            var schemaRepository = new SchemaRepository();
            var schemaGenerator = new DummySchemaGenerator();

            // Necesitamos un MethodInfo dummy para el constructor de OperationFilterContext.
            // Creamos un método privado dummy y obtenemos su MethodInfo.
            var methodInfo = GetType().GetMethod(nameof(DummyMethod), BindingFlags.NonPublic | BindingFlags.Instance)!;

            // Creamos el OperationFilterContext con todos los parámetros.
            _context = new OperationFilterContext(apiDescription, schemaGenerator, schemaRepository, methodInfo);

            _operation = new OpenApiOperation();
        }

        // Método dummy para obtener un MethodInfo
        private static void DummyMethod()
        {
            // Este método está intencionalmente vacío.
            // Se utiliza únicamente para obtener un MethodInfo válido en los tests.
        }


        [Test]
        public void Apply_Should_Add_ErrorResponses_To_Operation()
        {
            // Act: Aplica el filtro
            _filter.Apply(_operation, _context);

            // Assert: Verifica que se hayan agregado las respuestas para 400, 401, 403, 404 y 500.
            _operation.Responses.Should().NotBeNull();
            _operation.Responses.Should().ContainKey("400");
            _operation.Responses.Should().ContainKey("401");
            _operation.Responses.Should().ContainKey("403");
            _operation.Responses.Should().ContainKey("404");
            _operation.Responses.Should().ContainKey("500");

            // Verifica los mensajes de descripción
            _operation.Responses["400"].Description.Should().Contain("Solicitud inválida");
            _operation.Responses["401"].Description.Should().Contain("No autorizado");
            _operation.Responses["403"].Description.Should().Contain("Prohibido");
            _operation.Responses["404"].Description.Should().Contain("No encontrado");
            _operation.Responses["500"].Description.Should().Contain("Error interno");
        }
    }
}
