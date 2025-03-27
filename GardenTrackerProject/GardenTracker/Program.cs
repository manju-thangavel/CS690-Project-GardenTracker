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
    }
}