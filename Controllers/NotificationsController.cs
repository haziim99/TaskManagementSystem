using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id ?? string.Empty;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserIdAsync();
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            var unreadCount = notifications.Count(n => !n.IsRead);

            ViewBag.UnreadCount = unreadCount;
            return View(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = await GetCurrentUserIdAsync();
            await _notificationService.MarkAllAsReadAsync(userId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            TempData["Success"] = "Notification deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        // API endpoint for getting unread count (for navbar badge)
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = await GetCurrentUserIdAsync();
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Json(new { count });
        }
    }
}
