using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;

        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
