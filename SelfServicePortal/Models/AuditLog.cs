namespace SelfServicePortal.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string OldStatus { get; set; } = "";
        public string NewStatus { get; set; } = "";
        public DateTime ChangedAt { get; set; } = DateTime.Now;
        public int RequestId { get; set; }
        public Request Request { get; set; } = null!;
        public int ChangedBy { get; set; }
    }
}
