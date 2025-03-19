using CloudStorageAPICleanArchitecture.Application.Common.Models;
using CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries;
using Microsoft.AspNetCore.Authorization;


namespace CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries
{
    [Authorize]
    public class GetLogsConversacionesQuery : IRequest<PaginatedList<LogsConversacionDto>>
    {
        public string PartitionKey { get; set; }
        public string? FechaDesde { get; set; }
        public string? FechaHasta { get; set; }
        public string? Channel { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public GetLogsConversacionesQuery(string partitionKey, string? fechaDesde, string? fechaHasta, string? channel, int? pageNumber, int? pageSize)
        {
            PartitionKey = partitionKey;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            Channel = channel;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
