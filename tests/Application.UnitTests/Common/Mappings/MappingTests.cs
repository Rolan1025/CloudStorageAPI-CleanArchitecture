using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using CloudStorageAPICleanArchitecture.Application.Common.Models;
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;
using CloudStorageAPICleanArchitecture.Domain.Entities;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        // Usamos LogsConversacionDto como punto de referencia para encontrar los perfiles de mapeo
        // que están en el mismo ensamblado
        _configuration = new MapperConfiguration(config =>
            config.AddMaps(Assembly.GetAssembly(typeof(LogsConversacionDto))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(LogsConversacion), typeof(LogsConversacionDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    [Test]
    public void ShouldMapPaginatedListCorrectly()
    {
        // Este test verifica cualquier mapeo relacionado con PaginatedList si existe
        var items = new List<LogsConversacionDto>
        {
            new LogsConversacionDto
            {
                PartitionKey = "12345678",
                RowKey = "msg123",
                Channel = "WHATSAPP",
                DateTime = DateTime.Now,
                Direction = "INBOUND",
                From = "+1234567890",
                To = "+0987654321",
                Text = "Mensaje de prueba",
                SentBy = "Usuario",
                Type = "TEXT",
                ConversationID = "conv123",
                NumeroCel = "+1234567890"
            }
        };

        var paginatedList = PaginatedList<LogsConversacionDto>.Create(items, 1, 10);

        // Verificar que la paginación funciona correctamente
        Assert.That(paginatedList.Items.Count, Is.EqualTo(1));
        Assert.That(paginatedList.PageNumber, Is.EqualTo(1));
        Assert.That(paginatedList.TotalPages, Is.EqualTo(1));
        Assert.That(paginatedList.TotalCount, Is.EqualTo(1));
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Para tipos sin constructor sin parámetros
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
