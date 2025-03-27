namespace GardenTracker;

class Program
{
    static void Main(string[] args)
    {
            User user = new User();
            if (!user.Login())
            {
                Console.WriteLine("Login failed. Exiting application.");
                return;
            }

            GardenTracker gardenTracker = new GardenTracker();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nHi there! Please choose an option:");
                Console.WriteLine("1. Log Activity");
                Console.WriteLine("2. View Reminders");
                Console.WriteLine("3. Manage Plants");
                Console.WriteLine("4. View History");
                Console.WriteLine("5. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        gardenTracker.LogActivity();
                        break;
                    case "2":
                        Console.WriteLine("You entered Option 2");     
                        // gardenTracker.ViewReminders();
                        break;
                    case "3":
                        Console.WriteLine("You entered Option 3");
                        // gardenTracker.ManagePlants();
                        break;
                    case "4":
                        Console.WriteLine("You entered Option 4");
                        // gardenTracker.ViewHistory();
                        break;
                    case "5":
                        Console.WriteLine("You entered Option 5");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid entry. Try again.");
                        break;
                }
            }

            Console.WriteLine("Thank you, Bye!");
    }
}