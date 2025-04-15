using GardenTracker.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

// This module focuses on the History management
// User can view the history of activities logged for the selected plant and period

namespace GardenTracker.Management
{
    public class HistoryManagement
    {
        private readonly ActivityService _activityService;

        public HistoryManagement(ActivityService activityService)
        {
            _activityService = activityService;
        }

// View History option
        public void ViewHistory(List<Plant> plants)
        {
            if (plants.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No plants available to view history.[/]");
                return;
            }

            var plant = AnsiConsole.Prompt(
                new SelectionPrompt<Plant>()
                    .Title("[bold green]Select a plant to view history:[/]")
                    .AddChoices(plants)
                    .UseConverter(p => p.Name));

            int days = AnsiConsole.Ask<int>("[bold green]Enter the number of days for viewing log history:[/]");

            var activities = GetHistory(plant, days);
            int historyCount = activities.Count;

            AnsiConsole.MarkupLine("[bold cyan]Activity History:[/]");
            foreach (var activity in activities)
            {
                AnsiConsole.MarkupLine($"[bold yellow]{activity.Date.ToUniversalTime():yyyy-MM-dd HH:mm:ss}[/] - [bold green]{activity.Type}[/] - Note: {activity.Note}");
            }

            if (historyCount == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No activities found for the given period![/]");
            }
        }

        public List<Activity> GetHistory(Plant plant, int days)
        {
            if (plant == null)
            {
                throw new ArgumentNullException(nameof(plant), "Plant cannot be null.");
            }

            var activities = _activityService.GetActivities();
            DateTime historyStartDate = DateTime.UtcNow.AddDays(-days);

            return activities
                .Where(activity => activity.Plant.Equals(plant) && activity.Date.ToUniversalTime() >= historyStartDate)
                .OrderBy(activity => activity.Date) //arranging in ascending order of activities based on the activity date
                .ToList();
        }
    }
}
