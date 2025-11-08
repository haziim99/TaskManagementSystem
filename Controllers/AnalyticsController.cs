using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;
using MyTaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalTasks = await _context.Tasks.CountAsync();
            var completedTasks = await _context.Tasks.CountAsync(t => t.Status == MyTaskStatus.Completed);
            var inProgressTasks = await _context.Tasks.CountAsync(t => t.Status == MyTaskStatus.InProgress);
            var todoTasks = await _context.Tasks.CountAsync(t => t.Status == MyTaskStatus.Todo);
            var overdueTasks = await _context.Tasks.CountAsync(t => t.DueDate < DateTime.Now && t.Status != MyTaskStatus.Completed);

            var tasksByPriority = new Dictionary<string, int>
            {
                { "Low", await _context.Tasks.CountAsync(t => t.Priority == TaskPriority.Low) },
                { "Medium", await _context.Tasks.CountAsync(t => t.Priority == TaskPriority.Medium) },
                { "High", await _context.Tasks.CountAsync(t => t.Priority == TaskPriority.High) },
                { "Urgent", await _context.Tasks.CountAsync(t => t.Priority == TaskPriority.Urgent) }
            };

            var tasksByMonth = new Dictionary<string, int>();
            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var monthName = month.ToString("MMM yyyy");
                var count = await _context.Tasks.CountAsync(t => t.CreatedDate.Month == month.Month && t.CreatedDate.Year == month.Year);
                tasksByMonth.Add(monthName, count);
            }

            var completedThisWeek = await _context.Tasks
                .CountAsync(t => t.Status == MyTaskStatus.Completed && t.CreatedDate >= DateTime.Now.AddDays(-7));

            var completedTasksWithDueDate = await _context.Tasks
            .Where(t => t.Status == MyTaskStatus.Completed && t.DueDate.HasValue)
            .ToListAsync();

            double avgCompletionTime = completedTasksWithDueDate.Any()
                ? completedTasksWithDueDate.Average(t => (t.DueDate!.Value - t.CreatedDate).TotalDays)
                : 0;

            var completedTasksList = await _context.Tasks
            .Where(t => t.Status == MyTaskStatus.Completed)
            .Select(t => t.CreatedDate)
            .ToListAsync();

            var mostProductiveDay = completedTasksList
                .GroupBy(d => d.DayOfWeek)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key.ToString())
                .FirstOrDefault() ?? "N/A";

            var analytics = new AnalyticsViewModel
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                InProgressTasks = inProgressTasks,
                TodoTasks = todoTasks,
                OverdueTasks = overdueTasks,
                CompletionRate = totalTasks > 0 ? Math.Round((double)completedTasks / totalTasks * 100, 1) : 0,
                TasksByPriority = tasksByPriority,
                TasksByMonth = tasksByMonth,
                AverageCompletionTime = avgCompletionTime,
                TasksCompletedThisWeek = completedThisWeek,
                MostProductiveDay = mostProductiveDay ?? "N/A"
            };

            return View(analytics);
        }
    }
}
