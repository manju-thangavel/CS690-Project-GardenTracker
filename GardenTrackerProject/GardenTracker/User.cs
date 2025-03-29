using System;
using System.IO;

namespace GardenTracker
{
    public class User
    {
        protected virtual string UserFilePath => "user_details_db.txt";
        private string username;
        private string password;

        public bool Login()
        {
            if (!File.Exists(UserFilePath))
            {
                Register();
            }

            Console.WriteLine("Please log in to continue.");

            Console.Write("Username: ");
            string inputUsername = Console.ReadLine();

            Console.Write("Password: ");
            string inputPassword = Console.ReadLine();

            string[] userDetails = File.ReadAllLines(UserFilePath);
            username = userDetails[0];
            password = userDetails[1];

            if (inputUsername == username && inputPassword == password)
            {
                Console.WriteLine("Login successful!");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
                return false;
            }
        }

        private void Register()
        {
            Console.WriteLine("User not found. Please register.");

            Console.Write("Enter username: ");
            username = Console.ReadLine();

            Console.Write("Enter password: ");
            password = Console.ReadLine();

            File.WriteAllLines(UserFilePath, new string[] { username, password });

            Console.WriteLine("Registration success! Please login now.");
        }
    }
}