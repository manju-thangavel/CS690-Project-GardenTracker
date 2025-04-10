using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using GardenTracker.Management;
using GardenTracker.Models;

namespace GardenTracker.Tests
{
    public class ActivityManagementTests : IDisposable
    {
        private const string TestActivitiesFilePath = "test_activities_db.txt";
        private ActivityService _activityService;

        public ActivityManagementTests()
        {
            if (File.Exists(TestActivitiesFilePath))
            {
                File.Delete(TestActivitiesFilePath);
            }

            _activityService = new ActivityService(TestActivitiesFilePath);
        }

        [Fact]
        public void LogActivity_ShouldAddActivity_WhenValidDataProvided()
        {
            var activityManagement = new ActivityManagement(_activityService);
            var plant = new Plant("Tomato");
            var activityType = ActivityType.Watering;
            var note = "Watered the plant.";
            DateTime? reminderDate = DateTime.UtcNow.AddDays(2);

            activityManagement.LogActivity(plant, activityType, note, reminderDate);

            var activities = _activityService.GetActivities();
            Assert.Single(activities);
            Assert.Equal(plant, activities[0].Plant);
            Assert.Equal(activityType, activities[0].Type);
            Assert.Equal(note, activities[0].Note);
            Assert.Equal(reminderDate.Value.ToUniversalTime(), activities[0].ReminderDate.Value.ToUniversalTime());
        }

        [Fact]
        public void LogActivity_ShouldThrowException_WhenPlantIsNull()
        {
            var activityManagement = new ActivityManagement(_activityService);
            Plant plant = null;
            var activityType = ActivityType.Watering;
            var note = "Watered the plant.";
            DateTime? reminderDate = DateTime.UtcNow.AddDays(2);

            var exception = Assert.Throws<ArgumentNullException>(() => activityManagement.LogActivity(plant, activityType, note, reminderDate));
            Assert.Equal("Plant cannot be null. (Parameter 'plant')", exception.Message);
        }

        [Fact]
        public void LogActivity_ShouldAddActivityWithoutReminder_WhenReminderIsNotSet()
        {
            var activityManagement = new ActivityManagement(_activityService);
            var plant = new Plant("Basil");
            var activityType = ActivityType.Pruning;
            var note = "Pruned the plant.";

            activityManagement.LogActivity(plant, activityType, note);

            var activities = _activityService.GetActivities();
            Assert.Single(activities);
            Assert.Equal(plant, activities[0].Plant);
            Assert.Equal(activityType, activities[0].Type);
            Assert.Equal(note, activities[0].Note);
            Assert.Null(activities[0].ReminderDate);
        }

        [Fact]
        public void LogActivity_ShouldHandleMultipleActivitiesForSamePlant()
        {
            var activityManagement = new ActivityManagement(_activityService);
            var plant = new Plant("Tomato");

            activityManagement.LogActivity(plant, ActivityType.Watering, "Watered the plant.");
            activityManagement.LogActivity(plant, ActivityType.Pruning, "Pruned the plant.");

            var activities = _activityService.GetActivities();
            Assert.Equal(2, activities.Count);
            Assert.Contains(activities, a => a.Type == ActivityType.Watering && a.Note == "Watered the plant.");
            Assert.Contains(activities, a => a.Type == ActivityType.Pruning && a.Note == "Pruned the plant.");
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
