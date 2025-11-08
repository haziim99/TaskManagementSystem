using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(string userId, string title, string message, string type, int? taskId = null)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                TaskId = taskId,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId);

            if (unreadOnly)
                query = query.Where(n => !n.IsRead);

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var n in notifications)
            {
                n.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CheckAndCreateTaskNotificationsAsync()
        {
            var now = DateTime.UtcNow;

            // Tasks due tomorrow
            var tasksDueTomorrow = await _context.Tasks
                .Where(t => !t.IsCompleted &&
                            t.DueDate.HasValue &&
                            t.DueDate.Value.Date == now.AddDays(1).Date)
                .ToListAsync();

            foreach (var task in tasksDueTomorrow)
            {
                if (string.IsNullOrEmpty(task.UserId)) continue;

                var exists = await _context.Notifications
                    .AnyAsync(n => n.TaskId == task.Id && n.Type == "DueTomorrow");

                if (!exists)
                {
                    await CreateNotificationAsync(
                        task.UserId,
                        "Task Due Tomorrow",
                        $"Your task '{task.Title}' is due tomorrow!",
                        "DueTomorrow",
                        task.Id
                    );
                }
            }

            // Overdue tasks
            var overdueTasks = await _context.Tasks
                .Where(t => !t.IsCompleted &&
                            t.DueDate.HasValue &&
                            t.DueDate.Value.Date < now.Date)
                .ToListAsync();

            foreach (var task in overdueTasks)
            {
                if (string.IsNullOrEmpty(task.UserId)) continue;

                var exists = await _context.Notifications
                    .AnyAsync(n => n.TaskId == task.Id && n.Type == "Overdue");

                if (!exists)
                {
                    await CreateNotificationAsync(
                        task.UserId,
                        "Task Overdue",
                        $"Your task '{task.Title}' is overdue!",
                        "Overdue",
                        task.Id
                    );
                }
            }

            // Completed tasks
            var completedTasks = await _context.Tasks
                .Where(t => t.IsCompleted &&
                            !_context.Notifications.Any(n => n.TaskId == t.Id && n.Type == "Completed"))
                .ToListAsync();

            foreach (var task in completedTasks)
            {
                if (string.IsNullOrEmpty(task.UserId)) continue;

                await CreateNotificationAsync(
                    task.UserId,
                    "Task Completed",
                    $"You completed the task '{task.Title}'!",
                    "Completed",
                    task.Id
                );
            }
        }
    }
}