using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, string title, string message, string type, int? taskId = null);
        Task<List<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
        Task DeleteNotificationAsync(int notificationId);
        Task CheckAndCreateTaskNotificationsAsync();
    }
}