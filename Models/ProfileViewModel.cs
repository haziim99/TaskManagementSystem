using System;

namespace TaskManagementSystem.Models
{
    public class ProfileViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public int TotalTasks { get; set; } = 0;
        public int CompletedTasks { get; set; } = 0;
        public string ProfileImage { get; set; } = string.Empty;

        // Navigation property

        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
