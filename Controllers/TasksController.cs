using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using TaskManagementSystem.Services;
using MyTaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public TasksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        private async Task<string> GetCurrentUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return user?.Id ?? string.Empty;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string searchString, MyTaskStatus? status, TaskPriority? priority)
        {
            var userId = await GetCurrentUserIdAsync();
            var tasks = _context.Tasks.Where(t => t.AssignedToUserId == userId);

            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchString) ||
                                         (t.Description != null && t.Description.Contains(searchString)));
            }

            if (status.HasValue)
            {
                tasks = tasks.Where(t => t.Status == status.Value);
            }

            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Priority == priority.Value);
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["StatusFilter"] = status;
            ViewData["PriorityFilter"] = priority;

            return View(await tasks.OrderByDescending(t => t.CreatedDate).ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = await GetCurrentUserIdAsync();
            var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
                return NotFound();

            return View(taskItem);
        }

        // GET: Tasks/Create
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            var taskItem = new TaskItem
            {
                Status = MyTaskStatus.Todo,
                Priority = TaskPriority.Medium,
                IsCompleted = false
            };

            // ??? ???? ?????????? ???????
            var users = await _userManager.Users
                .Select(u => new { u.Id, u.FullName, u.Email })
                .ToListAsync();

            ViewBag.Users = users;
            return View(taskItem);
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Status,Priority,AssignedToUserId")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = await GetCurrentUserIdAsync();
                taskItem.CreatedByUserId = currentUserId; // ?? ??? ??????
                taskItem.UserId = currentUserId; // ??????? ?? ????? ??????
                taskItem.CreatedDate = DateTime.Now;

                _context.Add(taskItem);
                await _context.SaveChangesAsync();

                // ????? ????? ?????? ??
                if (!string.IsNullOrEmpty(taskItem.AssignedToUserId))
                {
                    await _notificationService.CreateNotificationAsync(
                        taskItem.AssignedToUserId,
                        "New Task Assigned",
                        $"You have been assigned: '{taskItem.Title}'",
                        "info",
                        taskItem.Id
                    );
                }

                TempData["Success"] = "Task created and assigned successfully!";
                return RedirectToAction(nameof(Index));
            }

            // ?? ???? ?????? ????? ????? ???????
            var users = await _userManager.Users
                .Select(u => new { u.Id, u.FullName, u.Email })
                .ToListAsync();
            ViewBag.Users = users;

            return View(taskItem);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = await GetCurrentUserIdAsync();
            var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
                return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedDate,DueDate,Status,Priority,IsCompleted,UserId")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
                return NotFound();

            var userId = await GetCurrentUserIdAsync();
            if (taskItem.UserId != userId)
                return Forbid();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Task updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var userId = await GetCurrentUserIdAsync();
            var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
                return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem != null)
            {
                _context.Tasks.Remove(taskItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Task deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Tasks/ToggleComplete/5
        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
                return NotFound();

            taskItem.IsCompleted = !taskItem.IsCompleted;
            taskItem.Status = taskItem.IsCompleted ? MyTaskStatus.Completed : MyTaskStatus.InProgress;

            _context.Update(taskItem);
            await _context.SaveChangesAsync();

            if (taskItem.IsCompleted)
            {
                await _notificationService.CreateNotificationAsync(
                    userId,
                    "Task Completed",
                    $"You completed '{taskItem.Title}' successfully",
                    "success",
                    taskItem.Id
                );
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Toggle Priority
        [HttpPost]
        public async Task<IActionResult> TogglePriority(int id)
        {
            var userId = await GetCurrentUserIdAsync();
            var taskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
                return NotFound();

            taskItem.Priority = taskItem.Priority switch
            {
                TaskPriority.Low => TaskPriority.Medium,
                TaskPriority.Medium => TaskPriority.High,
                TaskPriority.High => TaskPriority.Urgent,
                TaskPriority.Urgent => TaskPriority.Low,
                _ => TaskPriority.Medium
            };

            _context.Update(taskItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = await GetCurrentUserIdAsync();
            var tasks = _context.Tasks.Where(t => t.UserId == userId);

            var totalTasks = await tasks.CountAsync();
            var completedTasks = await tasks.CountAsync(t => t.Status == MyTaskStatus.Completed);
            var inProgressTasks = await tasks.CountAsync(t => t.Status == MyTaskStatus.InProgress);
            var overdueTasks = await tasks.CountAsync(t => t.DueDate < DateTime.Now && t.Status != MyTaskStatus.Completed);

            ViewBag.TotalTasks = totalTasks;
            ViewBag.CompletedTasks = completedTasks;
            ViewBag.InProgressTasks = inProgressTasks;
            ViewBag.OverdueTasks = overdueTasks;

            return View(await tasks.OrderByDescending(t => t.CreatedDate).ToListAsync());
        }

        private bool TaskItemExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}