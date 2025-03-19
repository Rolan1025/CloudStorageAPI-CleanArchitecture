using System.Globalization;
using CloudStorageAPICleanArchitecture.Application.Common.Interfaces;
using CloudStorageAPICleanArchitecture.Application.Common.Mappings;
using CloudStorageAPICleanArchitecture.Application.Common.Models;
using CloudStorageAPICleanArchitecture.Domain.Entities;

namespace CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries
{
    public class GetLogsConversacionesQueryHandler : IRequestHandler<GetLogsConversacionesQuery, PaginatedList<LogsConversacionDto>>
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly IMapper _mapper;

        public GetLogsConversacionesQueryHandler(ITableStorageService tableStorageService, IMapper mapper)
        {
            _tableStorageService = tableStorageService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<LogsConversacionDto>> Handle(GetLogsConversacionesQuery request, CancellationToken cancellationToken)
        {
            DateTime? fechaDesde = ParseDate(request.FechaDesde);
            DateTime? fechaHasta = ParseDate(request.FechaHasta);

            // 🔹 Obtener todos los registros del usuario (PartitionKey)
            var entities = await _tableStorageService.GetEntitiesByPartitionKeyAsync<LogsConversacion>("LogsConversaciones", request.PartitionKey);

            // 🔹 Filtrar por fecha y canal si están definidos
            var filteredEntities = entities.Where(e =>
            {
                DateTime fechaRowKey = ExtraerFechaDesdeRowKey(e.RowKey);
                bool isValid = true;

                if (fechaDesde.HasValue)
                    isValid &= fechaRowKey >= fechaDesde.Value;

                if (fechaHasta.HasValue)
                    isValid &= fechaRowKey <= fechaHasta.Value;

                if (!string.IsNullOrWhiteSpace(request.Channel))
                    isValid &= e.Channel.Equals(request.Channel, StringComparison.OrdinalIgnoreCase);

                return isValid;
            });

            var mappedEntities = _mapper.Map<List<LogsConversacionDto>>(filteredEntities)
                   .OrderBy(e => ExtraerFechaDesdeRowKey(e.RowKey))
                   .ThenBy(e => ExtraerIdDesdeRowKey(e.RowKey));

            if (!request.PageNumber.HasValue || !request.PageSize.HasValue)
            {
                return new PaginatedList<LogsConversacionDto>(
                    mappedEntities.ToList(), mappedEntities.Count(), 1, mappedEntities.Count());
            }

            return PaginatedList<LogsConversacionDto>.Create(
                mappedEntities,
                request.PageNumber.Value,
                request.PageSize.Value);

        }

        private DateTime? ParseDate(string? date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                return parsedDate;
            }
            return null;
        }

        private DateTime ExtraerFechaDesdeRowKey(string rowKey)
        {
            string fechaStr = rowKey.Substring(0, 14); // formato "yyyyMMddHHmmss"
            return DateTime.ParseExact(fechaStr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        }

        private int ExtraerIdDesdeRowKey(string rowKey)
        {
            string idStr = rowKey.Substring(14);
            if (int.TryParse(idStr, out int id))
                return id;

            return 0;
        }
    }
}
