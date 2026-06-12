namespace SelfServicePortal.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Priority { get; set; } = "Medium";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int? AssignedTo { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
        public List<AuditLog> AuditLogs { get; set; } = new();
    }
}
