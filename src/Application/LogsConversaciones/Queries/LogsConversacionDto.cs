using CloudStorageAPICleanArchitecture.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace CloudStorageAPICleanArchitecture.Application.LogsConversaciones.Queries
{
    /// <summary>
    /// DTO que representa un mensaje de conversación almacenado en Table Storage.
    /// </summary>
    [SwaggerSchema(Title = "Logs de Conversación", Description = "Representa un mensaje de una conversación en Table Storage.")]
    public class LogsConversacionDto
    {
        /// <summary>
        /// Clave de partición (Documento de identidad).
        /// </summary>
        [SwaggerSchema(Description = "Clave de partición, representa el documento de identidad del usuario.")]
        public required string PartitionKey { get; set; }

        /// <summary>
        /// Clave de fila (Identificador único del mensaje).
        /// </summary>
        [SwaggerSchema(Description = "Clave de fila, representa el identificador único del mensaje.")]
        public required string RowKey { get; set; }

        /// <summary>
        /// Marca de tiempo del mensaje.
        /// </summary>
        [SwaggerSchema(Description = "Fecha y hora en que se registró el mensaje en Table Storage.")]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Canal de comunicación (Ejemplo: WHATSAPP).
        /// </summary>
        [SwaggerSchema(Description = "Canal por el cual se envió el mensaje.")]
        public required string Channel { get; set; }

        /// <summary>
        /// Fecha y hora en que se envió el mensaje.
        /// </summary>
        [SwaggerSchema(Description = "Fecha y hora en que se generó el mensaje en formato ISO 8601.")]
        public required DateTime DateTime { get; set; }

        /// <summary>
        /// Dirección del mensaje (INBOUND para entrada, OUTBOUND para salida).
        /// </summary>
        [SwaggerSchema(Description = "Dirección del mensaje: INBOUND (entrada) o OUTBOUND (salida).")]
        public required string Direction { get; set; }

        /// <summary>
        /// Número de teléfono del remitente.
        /// </summary>
        [SwaggerSchema(Description = "Número de teléfono del remitente del mensaje.")]
        public required string From { get; set; }

        /// <summary>
        /// Identificación del remitente.
        /// </summary>
        [SwaggerSchema(Description = "Identificación del remitente del mensaje.")]
        public required string SentBy { get; set; }

        /// <summary>
        /// Contenido del mensaje.
        /// </summary>
        [SwaggerSchema(Description = "Texto del mensaje enviado o recibido.")]
        public required string Text { get; set; }

        /// <summary>
        /// Número de teléfono del destinatario.
        /// </summary>
        [SwaggerSchema(Description = "Número de teléfono del destinatario del mensaje.")]
        public required string To { get; set; }

        /// <summary>
        /// Tipo de mensaje (Ejemplo: BOT).
        /// </summary>
        [SwaggerSchema(Description = "Tipo de mensaje enviado o recibido.")]
        public required string Type { get; set; }

        /// <summary>
        /// Identificador de la conversación.
        /// </summary>
        [SwaggerSchema(Description = "ID único que identifica la conversación a la que pertenece el mensaje.")]
        public required string ConversationID { get; set; }

        /// <summary>
        /// Número de celular relacionado con el mensaje.
        /// </summary>
        [SwaggerSchema(Description = "Número de celular vinculado con la conversación.")]
        public required string NumeroCel { get; set; }

        /// <summary>
        /// Mapeo entre `LogsConversacion` y `LogsConversacionDto`.
        /// </summary>
        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<LogsConversacion, LogsConversacionDto>();
            }
        }
    }
}
