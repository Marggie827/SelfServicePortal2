namespace SelfServicePortal.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Request> Requests { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
    }
}