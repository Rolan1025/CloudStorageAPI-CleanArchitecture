using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Models;
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;
using CloudStorageAPICleanArchitecture.Web.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Web.UnitTests.Endpoints
{
    [TestFixture]
    public class ConsultarLogsConversacionesTests
    {
        private Mock<IMediator> _mediatorMock = null!;
        private ConsultarLogsConversaciones _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ConsultarLogsConversaciones(_mediatorMock.Object);
        }

        [Test]
        public async Task GetLogsConversaciones_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var partitionKey = "12345678";
            string? fechaDesde = "2025-02-19T10:00:00";
            string? fechaHasta = "2025-02-20T10:00:00";
            string? channel = "WHATSAPP";
            int? pageNumber = 1;
            int? pageSize = 10;

            // Creamos una lista dummy de LogsConversacionDto
            var dummyLogs = new List<LogsConversacionDto>
            {
                new LogsConversacionDto
                {
                    PartitionKey = partitionKey,
                    RowKey = "row1",
                    Channel = channel,
                    DateTime = System.DateTime.Now,
                    Direction = "INBOUND",
                    From = "111",
                    To = "222",
                    Text = "Test",
                    SentBy = "User",
                    Type = "TEXT",
                    ConversationID = "conv1",
                    NumeroCel = "123"
                },
                new LogsConversacionDto
                {
                    PartitionKey = partitionKey,
                    RowKey = "row2",
                    Channel = channel,
                    DateTime = System.DateTime.Now,
                    Direction = "OUTBOUND",
                    From = "333",
                    To = "444",
                    Text = "Test2",
                    SentBy = "User",
                    Type = "TEXT",
                    ConversationID = "conv2",
                    NumeroCel = "456"
                }
            };

            // Usamos el método Create para construir una lista paginada dummy.
            var dummyPaginatedList = PaginatedList<LogsConversacionDto>.Create(dummyLogs, pageNumber.Value, pageSize.Value);

            // Configuramos el mediator para que retorne el dummyPaginatedList cuando se envíe cualquier GetLogsConversacionesQuery.
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetLogsConversacionesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dummyPaginatedList);

            // Act
            var result = await _controller.GetLogsConversaciones(partitionKey, fechaDesde, fechaHasta, channel, pageNumber, pageSize);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(dummyPaginatedList);
        }
    }
}
