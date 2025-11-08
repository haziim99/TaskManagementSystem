using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Models;
using MyTaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var profile = new ProfileViewModel
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "+1 234 567 8900",
                JobTitle = "Software Developer",
                Bio = "Passionate developer focused on creating efficient task management solutions.",
                JoinDate = new DateTime(2023, 1, 15),
                TotalTasks = await _context.Tasks.CountAsync(),
                CompletedTasks = await _context.Tasks.CountAsync(t => t.Status == MyTaskStatus.Completed),
                ProfileImage = "/images/default-avatar.png"
            };

            return View(profile);
        }
    }
}
