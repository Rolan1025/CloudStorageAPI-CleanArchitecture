using Azure.Data.Tables;

namespace CloudStorageAPICleanArchitecture.Application.Common.Interfaces
{
    public interface ITableStorageService
    {
        Task<List<T>> GetEntitiesAsync<T>(string tableName) where T : class, ITableEntity, new();

        Task<List<T>> GetEntitiesByPartitionKeyAsync<T>(string tableName, string partitionKey) where T : class, ITableEntity, new();
    }
}
