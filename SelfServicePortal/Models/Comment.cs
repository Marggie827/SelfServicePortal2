namespace SelfServicePortal.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int RequestId { get; set; }
        public Request Request { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
