using GardenTracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

        public List<Plant> GetPlants()
        {
            return plants;
        }

        public void AddPlant(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Plant name cannot be empty or whitespace.", nameof(name));
            }

            if (!PlantExists(name))
            {
                plants.Add(new Plant(name));
                SavePlants();
            }
            else
            {
                throw new InvalidOperationException("Plant already exists.");
            }
        }

        public void RemovePlant(Plant plant)
        {
            if (plant == null)
            {
                throw new ArgumentNullException(nameof(plant), "Plant cannot be null.");
            }

            if (plants.Contains(plant))
            {
                plants.Remove(plant);
                SavePlants();
            }
            else
            {
                throw new InvalidOperationException("Plant does not exist.");
            }
        }

        private List<Plant> LoadPlants()
        {
            if (File.Exists(_plantsFilePath))
            {
                string json = File.ReadAllText(_plantsFilePath);
                var plantsList = JsonSerializer.Deserialize<List<Plant>>(json);
                return plantsList ?? new List<Plant>();
            }
            return new List<Plant>();
        }

        private void SavePlants()
        {
            string json = JsonSerializer.Serialize(plants);
            File.WriteAllText(_plantsFilePath, json);
        }

        private bool PlantExists(string name)
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
