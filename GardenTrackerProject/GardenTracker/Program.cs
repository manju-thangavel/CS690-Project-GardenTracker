using GardenTracker.Management;
using GardenTracker.Models;
using System;
using System.Collections.Generic;

namespace GardenTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            var consoleUI = new ConsoleUI();
            var userManagement = new UserManagement();

            if (!userManagement.Login())
            {
                consoleUI.PrintMessage("Login failed. Exiting application.", "red");
                return;
            }

            var plantsManagement = new PlantsManagement();
            var activityService = new ActivityService();
            var activityManagement = new ActivityManagement(activityService);
            var reminderManagement = new ReminderManagement(activityService);
            var historyManagement = new HistoryManagement(activityService);

            bool exit = false;
            while (!exit)
            {
                var choice = consoleUI.PromptSelection("Select an option:", new[]
                {
                    "Log Activity",
                    "View Reminders",
                    "View History",
                    "Manage Plants",
                    "Exit"
                });

                switch (choice)
                {
                    case "Log Activity":
                        var plants = plantsManagement.GetPlants();
                        if (plants.Count == 0)
                        {
                            consoleUI.PrintMessage("No plants available. Please add plants first.", "red");
                            break;
                        }

                        var selectedPlant = consoleUI.PromptSelection("Select a plant:", plants, p => p.Name);
                        var activityType = consoleUI.PromptSelection("Select an activity:", Enum.GetValues<ActivityType>());
                        var note = consoleUI.AskString("Enter your custom note for this activity:");
                        var setReminder = consoleUI.Confirm("Do you want to set a reminder for this activity?");
                        DateTime? reminderDate = null;

                        if (setReminder)
                        {
                            int reminderDays = consoleUI.AskInt("Enter number of days to be reminded in:");
                            reminderDate = DateTime.UtcNow.AddDays(reminderDays);
                        }

                        activityManagement.LogActivity(selectedPlant, activityType, note, reminderDate);
                        consoleUI.PrintMessage("Activity logged successfully!");
                        break;

                    case "View Reminders":
                        int days = consoleUI.AskInt("Enter the number of days to view reminders:");
                        var reminders = reminderManagement.GetReminders(days);
                        if (reminders.Count == 0)
                        {
                            consoleUI.PrintMessage("No reminders for the given period!", "red");
                        }
                        else
                        {
                            foreach (var reminder in reminders)
                            {
                                consoleUI.PrintMessage($"{reminder.Plant.Name} - {reminder.Type} on {reminder.ReminderDate.Value:yyyy-MM-dd} - Note: {reminder.Note}", "yellow");
                            }
                        }
                        break;

                    case "View History":
                        var allPlants = plantsManagement.GetPlants();
                        if (allPlants.Count == 0)
                        {
                            consoleUI.PrintMessage("No plants available to view history.", "red");
                            break;
                        }

                        var plantToView = consoleUI.PromptSelection("Select a plant to view history:", allPlants, p => p.Name);
                        int historyDays = consoleUI.AskInt("Enter the number of days to view history:");
                        var history = historyManagement.GetHistory(plantToView, historyDays);

                        if (history.Count == 0)
                        {
                            consoleUI.PrintMessage("No activities found for the given period!", "red");
                        }
                        else
                        {
                            consoleUI.PrintMessage("Activity History:", "cyan");
                            foreach (var activity in history)
                            {
                                consoleUI.PrintMessage($"{activity.Date.ToUniversalTime():yyyy-MM-dd HH:mm:ss} - {activity.Type} - Note: {activity.Note}", "yellow");
                            }
                        }
                        break;

                    case "Manage Plants":
                        var plantChoice = consoleUI.PromptSelection("Manage Plants:", new[] { "Add Plant", "Remove Plant" });
                        if (plantChoice == "Add Plant")
                        {
                            var plantName = consoleUI.AskString("Enter plant name to add:");
                            try
                            {
                                plantsManagement.AddPlant(plantName);
                                consoleUI.PrintMessage("Plant added successfully!");
                            }
                            catch (Exception ex)
                            {
                                consoleUI.PrintMessage(ex.Message, "red");
                            }
                        }
                        else if (plantChoice == "Remove Plant")
                        {
                            var plantsToRemove = plantsManagement.GetPlants();
                            if (plantsToRemove.Count == 0)
                            {
                                consoleUI.PrintMessage("No plants to remove.", "red");
                                break;
                            }

                            var plantToRemove = consoleUI.PromptSelection("Select a plant to remove:", plantsToRemove, p => p.Name);
                            try
                            {
                                plantsManagement.RemovePlant(plantToRemove);
                                consoleUI.PrintMessage("Plant removed successfully!");
                            }
                            catch (Exception ex)
                            {
                                consoleUI.PrintMessage(ex.Message, "red");
                            }
                        }
                        break;

                    case "Exit":
                        consoleUI.PrintMessage("Thank you, Bye!");
                        exit = true;
                        break;
                }
            }
        }
    }
}
