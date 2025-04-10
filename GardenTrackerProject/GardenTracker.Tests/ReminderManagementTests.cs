using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using GardenTracker.Management;
using GardenTracker.Models;

namespace GardenTracker.Tests
{
    public class ReminderManagementTests : IDisposable
    {
        private const string TestActivitiesFilePath = "test_activities_db.txt";
        private ActivityService _activityService;

        public ReminderManagementTests()
        {
            if (File.Exists(TestActivitiesFilePath))
            {
                File.Delete(TestActivitiesFilePath);
            }

            _activityService = new ActivityService(TestActivitiesFilePath);
        }

        [Fact]
        public void GetReminders_ShouldReturnRemindersWithinSpecifiedDays()
        {
            File.WriteAllText(TestActivitiesFilePath, JsonSerializer.Serialize(new List<Activity>
            {
            }));

            var reminderManagement = new ReminderManagement(_activityService);

            var reminders = reminderManagement.GetReminders(3);

            Assert.Empty(reminders);
        }

        [Fact]
        public void GetReminders_ShouldReturnEmptyList_WhenNoRemindersExistWithinSpecifiedDays()
        {
            File.WriteAllText(TestActivitiesFilePath, JsonSerializer.Serialize(new List<Activity>
            {
                new Activity(new Plant("Tomato"), ActivityType.Watering, DateTime.UtcNow, "Watered the plant.", DateTime.UtcNow.AddDays(5)),
                new Activity(new Plant("Basil"), ActivityType.Pruning, DateTime.UtcNow, "Pruned the plant.", DateTime.UtcNow.AddDays(7))
            }));

            var reminderManagement = new ReminderManagement(_activityService);

            var reminders = reminderManagement.GetReminders(3);

            Assert.Empty(reminders);
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
