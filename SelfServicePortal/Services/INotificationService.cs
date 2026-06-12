using SelfServicePortal.Models;

namespace SelfServicePortal.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUnreadNotifications(int userId);
        Task<int> GetUnreadCount(int userId);
        Task MarkAsRead(int notificationId);
        Task MarkAllAsRead(int userId);
    }
}