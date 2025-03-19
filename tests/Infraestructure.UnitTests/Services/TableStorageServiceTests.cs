using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using CloudStorageAPICleanArchitecture.Infrastructure.Services;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace CloudStorageAPICleanArchitecture.Infrastructure.UnitTests.Services
{
    // Entidad dummy que implementa ITableEntity
    public class DummyEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = string.Empty;
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

    [TestFixture]
    public class TableStorageServiceTests
    {
        private Mock<TableServiceClient> _tableServiceClientMock = null!;
        private Mock<TableClient> _tableClientMock = null!;
        private TableStorageService _service = null!;
        private List<DummyEntity> _dummyData = null!;

        [SetUp]
        public void Setup()
        {
            _dummyData = new List<DummyEntity>
            {
                new DummyEntity { PartitionKey = "123", RowKey = "1" },
                new DummyEntity { PartitionKey = "123", RowKey = "2" }
            };

            _tableClientMock = new Mock<TableClient>(MockBehavior.Strict);
            _tableServiceClientMock = new Mock<TableServiceClient>(MockBehavior.Strict);

            // 1) Setup para GetEntitiesAsync => filter == null
            _tableClientMock
                .Setup(x => x.QueryAsync<DummyEntity>(
                    It.Is<string?>(filter => filter == null),               // string filter
                    It.IsAny<int?>(),                                       // maxPerPage
                    It.IsAny<IEnumerable<string>>(),                        // select
                    It.IsAny<CancellationToken>()))
                .Returns((string? filter, int? maxPerPage, IEnumerable<string> select, CancellationToken ct) =>
                {
                    // Sin filtro => devolvemos _dummyData entera
                    return CreateAsyncPageable(_dummyData);
                });

            // 2) Setup para GetEntitiesByPartitionKeyAsync => "PartitionKey eq '{partitionKey}'"
            _tableClientMock
                .Setup(x => x.QueryAsync<DummyEntity>(
                    It.Is<string?>(filter => filter != null && filter.Contains("PartitionKey eq")),
                    It.IsAny<int?>(),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<CancellationToken>()))
                .Returns((string filter, int? maxPerPage, IEnumerable<string> select, CancellationToken ct) =>
                {
                    // Ej: filter = "PartitionKey eq '123'"
                    string pk = "";
                    var parts = filter.Split('\'');
                    if (parts.Length >= 2)
                    {
                        pk = parts[1];
                    }

                    // Filtramos por pk
                    var filtered = _dummyData.Where(e => e.PartitionKey == pk);
                    return CreateAsyncPageable(filtered);
                });

            _tableServiceClientMock
                .Setup(sc => sc.GetTableClient(It.IsAny<string>()))
                .Returns(_tableClientMock.Object);

            // Instanciamos el servicio
            _service = new TableStorageService(_tableServiceClientMock.Object);
        }

        [Test]
        public async Task GetEntitiesAsync_ShouldReturnAll()
        {
            // Arrange
            var tableName = "dummyTable";

            // Act => Llama QueryAsync<T>(filter = null, ...)
            var result = await _service.GetEntitiesAsync<DummyEntity>(tableName);

            // Assert
            result.Should().HaveCount(2);
            result.Select(e => e.RowKey).Should().Contain("1");
            result.Select(e => e.RowKey).Should().Contain("2");
        }

        [Test]
        public async Task GetEntitiesByPartitionKeyAsync_ShouldFilterByPartitionKey()
        {
            // Arrange
            var tableName = "dummyTable";
            var partitionKey = "123";

            // Act => Llama QueryAsync<T>("PartitionKey eq '123'", ...)
            var result = await _service.GetEntitiesByPartitionKeyAsync<DummyEntity>(tableName, partitionKey);

            // Assert
            result.Should().HaveCount(2);
            result.All(e => e.PartitionKey == partitionKey).Should().BeTrue();
        }

        #region Helpers
        private static AsyncPageable<T> CreateAsyncPageable<T>(IEnumerable<T> items) where T : notnull
        {
            var page = Page<T>.FromValues(items.ToList(), null, CreateDummyResponse());
            return AsyncPageable<T>.FromPages(new[] { page });
        }

        private static Response CreateDummyResponse()
        {
            var responseMock = new Mock<Response>();
            return responseMock.Object;
        }
        #endregion
    }
}
