using GardenTracker.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GardenTracker.Management
{
    public class ReminderManagement
    {
        private readonly ActivityService _activityService;

        public ReminderManagement(ActivityService activityService)
        {
            _activityService = activityService;
        }

        public void ViewReminders()
        {
            int days = AnsiConsole.Ask<int>("[bold green]Enter the number of days to view reminders:[/]");

            var sortedActivities = GetReminders(days);
            DateTime todayDate = DateTime.UtcNow;
            DateTime remindDate = DateTime.UtcNow.AddDays(days);
            int reminderCount = 0;

            AnsiConsole.MarkupLine($"[bold cyan]Today (UTC): {todayDate:yyyy-MM-dd HH:mm:ss}, Remind Until (UTC): {remindDate:yyyy-MM-dd HH:mm:ss}[/]");

            foreach (var activity in sortedActivities)
            {
                AnsiConsole.MarkupLine($"[bold yellow]{activity.Plant.Name}[/] - [bold green]{activity.Type}[/] on [bold yellow]{(activity.ReminderDate.HasValue ? activity.ReminderDate.Value.ToUniversalTime().ToString("yyyy-MM-dd") : "N/A")}[/] - Note: {activity.Note}");
                reminderCount++;
            }

            if (reminderCount == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No reminders for the given period![/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[bold green]Displayed {reminderCount} reminders.[/]");
            }
        }

        public List<Activity> GetReminders(int days)
        {
            var activities = _activityService.GetActivities();
            DateTime remindDate = DateTime.UtcNow.AddDays(days);
            DateTime todayDate = DateTime.UtcNow;

            return activities
                .Where(activity => activity.ReminderDate.HasValue &&
                                   activity.ReminderDate.Value.ToUniversalTime() >= todayDate &&
                                   activity.ReminderDate.Value.ToUniversalTime() <= remindDate)
                .OrderBy(activity => activity.ReminderDate ?? DateTime.MaxValue)
                .ToList();
        }
    }
}
