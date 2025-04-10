using GardenTracker.Models;
using Spectre.Console;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace GardenTracker.Management
{
    public class PlantsManagement
    {
        private readonly string _plantsFilePath;
        private List<Plant> plants;

        public PlantsManagement(string plantsFilePath = "plants_db.txt")
        {
            _plantsFilePath = plantsFilePath;
            plants = LoadPlants();
        }

        public void ManagePlants()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Manage Plants:[/]")
                    .AddChoices("Add Plant", "Remove Plant"));

            if (choice == "Add Plant")
            {
                string name = AnsiConsole.Ask<string>("[bold green]Enter plant name to add to the garden tracker:[/]").Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    AnsiConsole.MarkupLine("[bold red]Invalid plant name. Please try again.[/]");
                    return;
                }

                if (!PlantExists(name))
                {
                    plants.Add(new Plant(name));
                    SavePlants();
                    AnsiConsole.MarkupLine("[bold green]Plant added to garden tracker successfully![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[bold red]Plant already exists in the garden tracker.[/]");
                }
            }
            else if (choice == "Remove Plant")
            {
                if (plants.Count == 0)
                {
                    AnsiConsole.MarkupLine("[bold red]No plants to remove.[/]");
                    return;
                }

                var plant = AnsiConsole.Prompt(
                    new SelectionPrompt<Plant>()
                        .Title("[bold green]Select a plant to remove from the garden tracker:[/]")
                        .AddChoices(plants)
                        .UseConverter(p => p.Name));

                plants.Remove(plant);
                SavePlants();
                AnsiConsole.MarkupLine("[bold green]Plant removed successfully.[/]");
            }
        }

        public List<Plant> LoadPlants()
        {
            if (File.Exists(_plantsFilePath))
            {
                string json = File.ReadAllText(_plantsFilePath);
                var plantsList = JsonSerializer.Deserialize<List<Plant>>(json);
                return plantsList ?? new List<Plant>();
            }
            return new List<Plant>();
        }

        public void SavePlants()
        {
            string json = JsonSerializer.Serialize(plants);
            File.WriteAllText(_plantsFilePath, json);
        }

        public bool PlantExists(string name)
        {
            foreach (var plant in plants)
            {
                if (plant.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
