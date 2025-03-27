using System;

namespace GardenTracker
{
    public class Activity
    {
        public Plant Plant { get; set; }
        public ActivityType Type { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public DateTime? ReminderDate { get; set; }

        public Activity(Plant plant, ActivityType type, DateTime date, string note, DateTime? reminderDate = null)
        {
            Plant = plant;
            Type = type;
            Date = date;
            Note = note;
            ReminderDate = reminderDate;
        }
    }

    public enum ActivityType
    {
        Planting = 1,
        Pruning,
        Watering,
        Harvesting,
        Fertilizing
    }
}