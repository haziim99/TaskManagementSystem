using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.ViewModels
{
    public class UserManagementViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public int TaskCount { get; set; }
    }

    public class UserDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public string? Bio { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public List<TaskItem> CreatedTasks { get; set; } = new();
        public List<TaskItem> AssignedTasks { get; set; } = new();
    }

    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        [Display(Name = "Current Role")]
        public string CurrentRole { get; set; } = string.Empty;

        [Display(Name = "New Role")]
        public string NewRole { get; set; } = string.Empty;
    }

    public class AssignTaskViewModel
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Assign To")]
        public string AssignedToUserId { get; set; } = string.Empty;

        public List<UserSelectItem> AvailableUsers { get; set; } = new();
    }

    public class UserSelectItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}