namespace SelfServicePortal.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Message { get; set; } = "";
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int RequestId { get; set; }
        public Request Request { get; set; } = null!;
    }
}
