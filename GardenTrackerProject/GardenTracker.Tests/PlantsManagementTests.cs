using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using GardenTracker.Management;
using GardenTracker.Models;

namespace GardenTracker.Tests
{
    public class PlantsManagementTests : IDisposable
    {
        private const string TestPlantsFilePath = "test_plants_db.txt";

        public PlantsManagementTests()
        {
            if (File.Exists(TestPlantsFilePath))
            {
                File.Delete(TestPlantsFilePath);
            }
        }

        [Fact]
        public void AddPlant_ShouldAddPlant_WhenPlantDoesNotExist()
        {
            var plantsManagement = new PlantsManagement(TestPlantsFilePath);
            plantsManagement.AddPlant("Tomato");
            var plants = plantsManagement.GetPlants();
            Assert.Single(plants);
            Assert.Equal("Tomato", plants[0].Name);
        }

        [Fact]
        public void AddPlant_ShouldThrowException_WhenPlantAlreadyExists()
        {
            File.WriteAllText(TestPlantsFilePath, JsonSerializer.Serialize(new List<Plant> { new Plant("Tomato") }));
            var plantsManagement = new PlantsManagement(TestPlantsFilePath);
            var exception = Assert.Throws<InvalidOperationException>(() => plantsManagement.AddPlant("Tomato"));
            Assert.Equal("Plant already exists.", exception.Message);
        }

        [Fact]
        public void RemovePlant_ShouldRemovePlant_WhenPlantExists()
        {
            File.WriteAllText(TestPlantsFilePath, JsonSerializer.Serialize(new List<Plant> { new Plant("Tomato") }));
            var plantsManagement = new PlantsManagement(TestPlantsFilePath);
            var plantToRemove = new Plant("Tomato");
            plantsManagement.RemovePlant(plantToRemove);
            var plants = plantsManagement.GetPlants();
            Assert.Empty(plants);
        }

        [Fact]
        public void RemovePlant_ShouldThrowException_WhenPlantDoesNotExist()
        {
            var plantsManagement = new PlantsManagement(TestPlantsFilePath);
            var plantToRemove = new Plant("Tomato");
            var exception = Assert.Throws<InvalidOperationException>(() => plantsManagement.RemovePlant(plantToRemove));
            Assert.Equal("Plant does not exist.", exception.Message);
        }

        public void Dispose()
        {
            if (File.Exists(TestPlantsFilePath))
            {
                File.Delete(TestPlantsFilePath);
            }
        }
    }
}
