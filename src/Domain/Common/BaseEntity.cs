using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudStorageAPICleanArchitecture.Domain.Common
{
    public abstract class BaseEntity : ITableEntity
    {
        /// <summary>
        /// Clave de partición en Table Storage (Ej: Documento de Identidad).
        /// </summary>
        public string PartitionKey { get; set; } = string.Empty;

        /// <summary>
        /// Clave de fila en Table Storage (Ej: Fecha + Consecutivo).
        /// </summary>
        public string RowKey { get; set; } = string.Empty;

        /// <summary>
        /// Marca de tiempo, manejada automáticamente por Table Storage.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Etiqueta de versión para concurrencia optimista.
        /// </summary>
        public ETag ETag { get; set; }

        /// <summary>
        /// Lista de eventos de dominio (no se almacena en la BD).
        /// </summary>
        [NotMapped] // 🔹 No se guarda en la base de datos.
        private readonly List<BaseEvent> _domainEvents = new();

        /// <summary>
        /// Obtener los eventos de dominio como solo lectura.
        /// </summary>
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Agregar un evento de dominio.
        /// </summary>
        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Eliminar un evento de dominio.
        /// </summary>
        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        /// <summary>
        /// Limpiar la lista de eventos de dominio.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
