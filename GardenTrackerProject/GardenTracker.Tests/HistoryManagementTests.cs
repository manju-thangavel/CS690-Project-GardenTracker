using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using GardenTracker.Management;
using GardenTracker.Models;

namespace GardenTracker.Tests
{
    public class HistoryManagementTests : IDisposable
    {
        private const string TestActivitiesFilePath = "test_activities_db.txt";
        private ActivityService _activityService;

        public HistoryManagementTests()
        {
            if (File.Exists(TestActivitiesFilePath))
            {
                File.Delete(TestActivitiesFilePath);
            }

            _activityService = new ActivityService(TestActivitiesFilePath);
        }

        [Fact]
        public void GetHistory_ShouldReturnEmptyList_WhenNoActivitiesExistForPlant()
        {
            File.WriteAllText(TestActivitiesFilePath, JsonSerializer.Serialize(new List<Activity>
            {
                new Activity(new Plant("Basil"), ActivityType.Watering, DateTime.UtcNow.AddDays(-1), "Watered the basil.")
            }));

            var historyManagement = new HistoryManagement(_activityService);
            var plant = new Plant("Tomato");

            var activities = historyManagement.GetHistory(plant, 3);

            Assert.Empty(activities);
        }

        public void Dispose()
        {
            if (File.Exists(TestActivitiesFilePath))
            {
                File.Delete(TestActivitiesFilePath);
            }
        }
    }
}
