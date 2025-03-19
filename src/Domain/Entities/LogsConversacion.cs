using CloudStorageAPICleanArchitecture.Domain.Common;

namespace CloudStorageAPICleanArchitecture.Domain.Entities
{
    public class LogsConversacion : BaseEntity
    {
        public string Channel { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string Direction { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string SentBy { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ConversationID { get; set; } = string.Empty;
        public string NumeroCel { get; set; } = string.Empty;
    }
}
