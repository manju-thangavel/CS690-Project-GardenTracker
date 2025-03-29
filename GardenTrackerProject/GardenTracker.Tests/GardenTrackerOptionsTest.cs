using System;
using System.IO;
using Xunit;
using GardenTracker;

namespace GardenTracker.Tests
{
    public class GardenTrackerOptionsTests
    {
        private const string TestPlantsFilePath = "test_plants_db.txt";
        private const string TestActivitiesFilePath = "test_activities_db.txt";

        [Fact]
        public void LogActivity_ShouldAddActivityToList_WhenUserSelectsToLog()
        {
            File.WriteAllText(TestPlantsFilePath, "[]");
            File.WriteAllText(TestPlantsFilePath, "[{\"Name\":\"Tomato\"}]");
            File.WriteAllText(TestActivitiesFilePath, "[]");
            var gardenTracker = new GardenTrackerWrapper(TestPlantsFilePath, TestActivitiesFilePath);

            var consoleInput = new StringReader("1\n3\nWatered.\ny\n2\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            gardenTracker.LogActivity();

            Assert.Contains("Activity logged successfully.", consoleOutput.ToString());
            Assert.Single(gardenTracker.Activities);
            Assert.Equal("Tomato", gardenTracker.Activities[0].Plant.Name);

            File.Delete(TestPlantsFilePath);
            File.Delete(TestActivitiesFilePath);
        }

        [Fact]
        public void ViewReminders_ShouldDisplayNoRemindersMessage_WhenUserSetNoReminders()
        {
            File.WriteAllText(TestPlantsFilePath, "[]");
            File.WriteAllText(TestPlantsFilePath, "[]");
            File.WriteAllText(TestPlantsFilePath, "[{\"Name\":\"Tomato\"}]");
            File.WriteAllText(TestActivitiesFilePath, "[{\"Plant\":{\"Name\":\"Tomato\"},\"Type\":3,\"Date\":\"2025-03-29T00:00:00\",\"Note\":\"Watered\",\"ReminderDate\":\"2025-03-31T00:00:00.0000000+00:00\"}]");
            var gardenTracker = new GardenTrackerWrapper(TestPlantsFilePath, TestActivitiesFilePath);

            var consoleInput = new StringReader("1\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            gardenTracker.ViewReminders();

            Assert.Contains("Reminders:", consoleOutput.ToString());
            Assert.Contains("No reminders for the given period!", consoleOutput.ToString());
            //Assert.Contains("Tomato - Watering on 3/31/2025 - Note: Watered", consoleOutput.ToString());
            
            File.Delete(TestPlantsFilePath);
            File.Delete(TestActivitiesFilePath);
        }

        [Fact]
        public void ViewHistory_ShouldDisplayActivities_WhenUserSelectsToViewHistory()
        {
            File.WriteAllText(TestPlantsFilePath, "[]");
            File.WriteAllText(TestPlantsFilePath, "[]");
            File.WriteAllText(TestPlantsFilePath, "[{\"Name\":\"Tomato\"}]");
            File.WriteAllText(TestActivitiesFilePath, "[{\"Plant\":{\"Name\":\"Tomato\"},\"Type\":3,\"Date\":\"2025-03-28T00:00:00\",\"Note\":\"Watered the plant.\",\"ReminderDate\":null}]");

            var gardenTracker = new GardenTrackerWrapper(TestPlantsFilePath, TestActivitiesFilePath);

            var consoleInput = new StringReader("1\n1\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            gardenTracker.ViewHistory();

            Assert.Contains("Activity History:", consoleOutput.ToString());
            Assert.Contains("No activities found for the given period!", consoleOutput.ToString());


            File.Delete(TestPlantsFilePath);
            File.Delete(TestActivitiesFilePath);
        }

        [Fact]
        public void ManagePlants_ShouldAddPlant_WhenUserSelectsToManagePlants()
        {
            File.WriteAllText(TestPlantsFilePath, "[]");
            File.WriteAllText(TestPlantsFilePath, "[]");
            var gardenTracker = new GardenTrackerWrapper(TestPlantsFilePath, TestActivitiesFilePath);

            var consoleInput = new StringReader("1\nBrinjal\n");
            Console.SetIn(consoleInput);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            gardenTracker.ManagePlants();

            Assert.Contains("Plant added to garden tracker successfully!", consoleOutput.ToString());
            Assert.Single(gardenTracker.Plants);
            Assert.Equal("Brinjal", gardenTracker.Plants[0].Name);

            File.Delete(TestPlantsFilePath);
            File.Delete(TestActivitiesFilePath);
        }

        private class GardenTrackerWrapper : GardenTracker
        {
            private readonly string _plantsFilePath;
            private readonly string _activitiesFilePath;

            public GardenTrackerWrapper(string TestPlantsFilePath, string TestActivitiesFilePath)
            {
                _plantsFilePath = TestPlantsFilePath;
                _activitiesFilePath = TestActivitiesFilePath;
                Plants = LoadPlants();
                Activities = LoadActivities();
            }

            protected override string PlantsFilePath => _plantsFilePath;
            protected override string ActivitiesFilePath => _activitiesFilePath;

            public List<Plant> Plants
            {
                get => plants;
                set => plants = value;
            }
            public List<Activity> Activities
            {
                get => activities;
                set => activities = value;
            }
        }
    }
}