using GardenTracker.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GardenTracker.Management
{
    public class ActivityService
    {
        private readonly string _activitiesFilePath;
        private List<Activity> _activities;

        public ActivityService(string activitiesFilePath = "activities_db.txt")
        {
            _activitiesFilePath = activitiesFilePath;
            _activities = LoadActivities();
        }

        public List<Activity> GetActivities()
        {
            return _activities;
        }
// Add new activity to list
        public void AddActivity(Activity activity)
        {
            _activities.Add(activity);
            SaveActivities();
        }
// Load activity from file
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
// Save activity to list
        private void SaveActivities()
        {
            string json = JsonSerializer.Serialize(_activities);
            File.WriteAllText(_activitiesFilePath, json);
        }
    }
}
