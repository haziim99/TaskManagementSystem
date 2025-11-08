using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? CreatedByUserId { get; set; }
        public string? AssignedToUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser? CreatedBy { get; set; }

        [ForeignKey("AssignedToUserId")]
        public virtual ApplicationUser? AssignedTo { get; set; }

        [Required(ErrorMessage = "Task title is required")]
        [StringLength(200, ErrorMessage = "The title must not exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "The description must not exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? DueDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public bool IsCompleted { get; set; } = false;
    }

    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Completed = 2
    }

    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Urgent = 3
    }
}
