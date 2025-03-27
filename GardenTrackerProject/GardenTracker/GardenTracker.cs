using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GardenTracker
{
    public class GardenTracker
    {
        private List<Plant> plants;
        private List<Activity> activities;
        private const string PlantsFilePath = "plants_db.txt";
        private const string ActivitiesFilePath = "activities_db.txt";

        public GardenTracker()
        {
            plants = LoadPlants();
            activities = LoadActivities();
        }

        public void LogActivity()
        {
            Console.WriteLine("Select a plant:");
            DisplayPlants();
            int plantIndex = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("Select an activity:");
            Console.WriteLine("1. Planting");
            Console.WriteLine("2. Pruning");
            Console.WriteLine("3. Watering");
            Console.WriteLine("4. Harvesting");
            Console.WriteLine("5. Fertilizing");
            int activityType = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter your custom note for this activity:");
            string note = Console.ReadLine();

            Console.WriteLine("Do you want to set a reminder for this activity? (Y/N)");
            string setReminder = Console.ReadLine().ToLower();

            DateTime activityDate = DateTime.Now;
            DateTime? reminderDate = null;

            if (setReminder == "y")
            {
               Console.WriteLine("Enter number of days to be reminded in: ");
               string reminderDaysInput = Console.ReadLine();
               if (int.TryParse(reminderDaysInput, out int reminderDays))
               {
                   reminderDate = DateTime.Now.AddDays(reminderDays);
               }
               else
               {
                   Console.WriteLine("Invalid number of days. Setting reminder for one week from today.");
                   reminderDate = DateTime.Now.AddDays(7);
               }
            }

            Activity activity = new Activity(plants[plantIndex], (ActivityType)activityType, activityDate, note, reminderDate);
            activities.Add(activity);
            SaveActivities();

            Console.WriteLine("Activity logged successfully.");
        }

        private void DisplayPlants()
        {
            for (int i = 0; i < plants.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {plants[i].Name}");
            }
        }

        private List<Plant> LoadPlants()
        {
            if (File.Exists(PlantsFilePath))
            {
                string json = File.ReadAllText(PlantsFilePath);
                return JsonSerializer.Deserialize<List<Plant>>(json);
            }
            return new List<Plant>();
        }

        private List<Activity> LoadActivities()
        {
            if (File.Exists(ActivitiesFilePath))
            {
                string json = File.ReadAllText(ActivitiesFilePath);
                return JsonSerializer.Deserialize<List<Activity>>(json);
            }
            return new List<Activity>();
        }

        private void SaveActivities()
        {
            string json = JsonSerializer.Serialize(activities);
            File.WriteAllText(ActivitiesFilePath, json);
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