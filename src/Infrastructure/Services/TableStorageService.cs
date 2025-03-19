using Azure.Data.Tables;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;

namespace CloudStorageAPICleanArchitecture.Infrastructure.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableServiceClient _tableServiceClient;

        public TableStorageService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
        }

        public async Task<List<T>> GetEntitiesAsync<T>(string tableName) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            var entities = new List<T>();

            await foreach (var entity in tableClient.QueryAsync<T>())
            {
                entities.Add(entity);
            }

            return entities;
        }

        public async Task<List<T>> GetEntitiesByPartitionKeyAsync<T>(string tableName, string partitionKey)
                   where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);

            // 🔹 Construir la consulta con filtro por PartitionKey
            var queryResults = tableClient.QueryAsync<T>($"PartitionKey eq '{partitionKey}'");

            var entities = new List<T>();
            await foreach (var entity in queryResults)
            {
                entities.Add(entity);
            }

            return entities;
        }
    }
}
