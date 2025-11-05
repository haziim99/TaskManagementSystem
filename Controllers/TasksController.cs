using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using MyTaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string searchString, MyTaskStatus? status, TaskPriority? priority)
        {
            var tasks = from t in _context.Tasks select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
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

            var taskItem = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
                return NotFound();

            return View(taskItem);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            var taskItem = new TaskItem
            {
                Status = MyTaskStatus.Todo,
                Priority = TaskPriority.Medium,
                IsCompleted = false
            };
            return View(taskItem);
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DueDate,Status,Priority")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.CreatedDate = DateTime.Now;
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Task added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
                return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedDate,DueDate,Status,Priority,IsCompleted")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
                return NotFound();

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

            var taskItem = await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null)
                return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
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
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null)
                return NotFound();

            taskItem.IsCompleted = !taskItem.IsCompleted;
            taskItem.Status = taskItem.IsCompleted ? MyTaskStatus.Completed : MyTaskStatus.InProgress;

            _context.Update(taskItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Toggle Priority
        [HttpPost]
        public async Task<IActionResult> TogglePriority(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null) return NotFound();

            taskItem.Priority = taskItem.Priority switch
            {
                TaskPriority.Low => TaskPriority.Medium,
                TaskPriority.Medium => TaskPriority.High,
                TaskPriority.High => TaskPriority.Low,
                _ => TaskPriority.Medium
            };

            _context.Update(taskItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var totalTasks = await _context.Tasks.CountAsync();
            var completedTasks = await _context.Tasks.CountAsync(t => t.Status == MyTaskStatus.Completed);
            var inProgressTasks = await _context.Tasks.CountAsync(t => t.Status == MyTaskStatus.InProgress);
            var overdueTasks = await _context.Tasks.CountAsync(t => t.DueDate < DateTime.Now && t.Status != MyTaskStatus.Completed);

            ViewBag.TotalTasks = totalTasks;
            ViewBag.CompletedTasks = completedTasks;
            ViewBag.InProgressTasks = inProgressTasks;
            ViewBag.OverdueTasks = overdueTasks;

            return View(await _context.Tasks.OrderByDescending(t => t.CreatedDate).ToListAsync());
        }

        private bool TaskItemExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
