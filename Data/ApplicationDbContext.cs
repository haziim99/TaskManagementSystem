using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using MyTaskStatus = TaskManagementSystem.Models.TaskStatus;

namespace TaskManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add seed data
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Create Database",
                    Description = "Design and create the project database",
                    CreatedDate = DateTime.Now,
                    Status = MyTaskStatus.Completed,
                    Priority = TaskPriority.High,
                    IsCompleted = true
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Develop User Interface",
                    Description = "Design a professional and user-friendly interface",
                    CreatedDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(7),
                    Status = MyTaskStatus.InProgress,
                    Priority = TaskPriority.Medium,
                    IsCompleted = false
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Add Authentication System",
                    Description = "Implement login and registration system",
                    CreatedDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    Status = MyTaskStatus.Todo,
                    Priority = TaskPriority.High,
                    IsCompleted = false
                }
            );
        }
    }
}
