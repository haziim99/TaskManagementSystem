using System;
using System.Collections.Generic;

namespace TaskManagementSystem.ViewModels
{
    public class AnalyticsViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int TodoTasks { get; set; }
        public int OverdueTasks { get; set; }
        public double CompletionRate { get; set; }

        public Dictionary<string, int> TasksByPriority { get; set; } = new();
        public Dictionary<string, int> TasksByMonth { get; set; } = new();

        public double AverageCompletionTime { get; set; }
        public int TasksCompletedThisWeek { get; set; }
        public string MostProductiveDay { get; set; } = string.Empty;
    }
}
