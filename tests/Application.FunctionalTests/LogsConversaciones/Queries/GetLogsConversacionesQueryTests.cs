using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Application.Common.Models;
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;
using CloudStorageAPICleanArchitecture.Domain.Entities;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Application.FunctionalTests.LogsConversaciones.Queries
{
    [TestFixture]
    public class GetLogsConversacionesQueryTests
    {
        private Mock<ITableStorageService> _tableStorageMock;
        private IMapper _mapper;
        private GetLogsConversacionesQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            // 🔹 Configuración del mock
            _tableStorageMock = new Mock<ITableStorageService>();

            // 🔹 Datos simulados de retorno
            var logs = new List<LogsConversacion>
            {
                new LogsConversacion
                {
                    PartitionKey = "12345678",
                    RowKey = "20250303120000123",
                    Channel = "WHATSAPP"
                },
                new LogsConversacion
                {
                    PartitionKey = "12345678",
                    RowKey = "20250303120000124",
                    Channel = "WHATSAPP"
                }
            };

            _tableStorageMock
                .Setup(x => x.GetEntitiesByPartitionKeyAsync<LogsConversacion>("LogsConversaciones", It.IsAny<string>()))
                .ReturnsAsync(logs);

            // 🔹 Configuración simple de Automapper para el test
            var configuration = new MapperConfiguration(cfg =>
                cfg.CreateMap<LogsConversacion, LogsConversacionDto>());

            _mapper = configuration.CreateMapper();

            // 🔹 Instancia del handler
            _handler = new GetLogsConversacionesQueryHandler(_tableStorageMock.Object, _mapper);
        }

        [Test]
        public async Task ShouldReturnAllLogs_WhenPageSizeNotSpecified()
        {
            // Arrange
            var query = new GetLogsConversacionesQuery("12345678", null, null, "WHATSAPP", null, null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.First().PartitionKey.Should().Be("12345678");
            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(1);
            result.TotalPages.Should().Be(1);
        }

        [Test]
        public async Task ShouldApplyPagination()
        {
            // Arrange
            var query = new GetLogsConversacionesQuery("12345678", null, null, null, pageNumber: 1, pageSize: 1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(2);
            result.TotalPages.Should().Be(2);
            result.PageNumber.Should().Be(1);
        }
    }
}
