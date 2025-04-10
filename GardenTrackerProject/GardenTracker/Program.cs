using GardenTracker.Management;
using GardenTracker.Models;
using System;
using Spectre.Console;

namespace GardenTracker;

public class Program
{
    static void Main(string[] args)
    {
        var userManagement = new UserManagement();
        if (!userManagement.Login())
        {
            AnsiConsole.Markup("[bold red]Login failed. Exiting application.[/]");
            return;
        }

        var plantsManagement = new PlantsManagement();
        var activityManagement = new ActivityManagement();

        bool exit = false;
        while (!exit)
        {
            AnsiConsole.MarkupLine("[bold cyan]\nHi there! Please choose an option:[/]");
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Select an option:[/]")
                    .AddChoices(new[]
                    {
                        "Log Activity",
                        "View Reminders",
                        "Manage Plants",
                        "View History",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Log Activity":
                    activityManagement.LogActivity(plantsManagement.LoadPlants());
                    break;
                case "Manage Plants":
                    plantsManagement.ManagePlants();
                    break;
                case "Exit":
                    AnsiConsole.MarkupLine("[bold yellow]You entered option to Exit.[/]");
                    exit = true;
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold red]Invalid entry. Try again.[/]");
                    break;
            }
        }

        AnsiConsole.MarkupLine("[bold green]Thank you, Bye![/]");
    }
}
