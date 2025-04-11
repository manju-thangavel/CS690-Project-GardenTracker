using GardenTracker.Models;
using Spectre.Console;
using System.Collections.Generic;
using System;

// This module focuses on the Activities management
// User can log the activities for the selected plant, set a reminder and note

namespace GardenTracker.Management
{
    public class ActivityManagement
    {
        private readonly ActivityService _activityService;

        public ActivityManagement(ActivityService activityService)
        {
            _activityService = activityService;
        }

        public void LogActivity(Plant plant, ActivityType activityType, string note, DateTime? reminderDate = null)
        {
            if (plant == null)
            {
                throw new ArgumentNullException(nameof(plant), "Plant cannot be null.");
            }

            var activity = new Activity(plant, activityType, DateTime.UtcNow, note, reminderDate);
            _activityService.AddActivity(activity);
        }

        public void LogActivity(List<Plant> plants)
        {
            if (plants.Count == 0)
            {
                AnsiConsole.MarkupLine("[bold red]No plants available. Please add plants first.[/]");
                return;
            }

            var plant = AnsiConsole.Prompt(
                new SelectionPrompt<Plant>()
                    .Title("[bold green]Select a plant:[/]")
                    .AddChoices(plants)
                    .UseConverter(p => p.Name));

            var activityType = AnsiConsole.Prompt(
                new SelectionPrompt<ActivityType>()
                    .Title("[bold green]Select an activity:[/]")
                    .AddChoices(ActivityType.Planting, ActivityType.Pruning, ActivityType.Watering, ActivityType.Harvesting, ActivityType.Fertilizing));

            string note = AnsiConsole.Ask<string>("[bold green]Enter your custom note for this activity:[/]");

            bool setReminder = AnsiConsole.Confirm("[bold green]Do you want to set a reminder for this activity?[/]");

            DateTime? reminderDate = null;
            if (setReminder)
            {
                int reminderDays = AnsiConsole.Ask<int>("[bold green]Enter number of days to be reminded in:[/]");
                reminderDate = DateTime.UtcNow.AddDays(reminderDays);
            }

            LogActivity(plant, activityType, note, reminderDate);

            AnsiConsole.MarkupLine("[bold green]Activity logged successfully![/]");
        }
    }
}
