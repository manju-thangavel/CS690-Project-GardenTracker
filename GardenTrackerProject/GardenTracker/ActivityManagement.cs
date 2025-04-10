using GardenTracker.Models;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;

namespace GardenTracker.Management
{
    public class ActivityManagement
    {
        private readonly string _activitiesFilePath;
        private List<Activity> activities;

        public ActivityManagement(string activitiesFilePath = "activities_db.txt")
        {
            _activitiesFilePath = activitiesFilePath;
            activities = LoadActivities();
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

            var activity = new Activity(plant, activityType, DateTime.UtcNow, note, reminderDate);
            activities.Add(activity);
            SaveActivities();

            AnsiConsole.MarkupLine("[bold green]Activity logged successfully![/]");
        }

        private List<Activity> LoadActivities()
        {
            if (File.Exists(_activitiesFilePath))
            {
                string json = File.ReadAllText(_activitiesFilePath);
                var activitiesList = JsonSerializer.Deserialize<List<Activity>>(json);
                return activitiesList ?? new List<Activity>();
            }
            return new List<Activity>();
        }

        private void SaveActivities()
        {
            string json = JsonSerializer.Serialize(activities);
            File.WriteAllText(_activitiesFilePath, json);
        }
    }
}
