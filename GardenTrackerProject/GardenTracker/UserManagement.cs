using System;
using System.IO;
using Spectre.Console;

namespace GardenTracker.Management
{
    public class UserManagement
    {
        private readonly string _userFilePath;
        private string username = string.Empty;
        private string password = string.Empty;

        public UserManagement(string userFilePath = "user_details_db.txt")
        {
            _userFilePath = userFilePath;
        }

        public bool Login()
        {
            if (!File.Exists(_userFilePath))
            {
                Register();
            }

            AnsiConsole.MarkupLine("[bold cyan]Please log in to continue.[/]");

            string inputUsername = AnsiConsole.Ask<string>("[bold green]Username: [/]");
            string inputPassword = AnsiConsole.Ask<string>("[bold green]Password: [/]");

            string[] userDetails = File.ReadAllLines(_userFilePath);
            username = userDetails[0];
            password = userDetails[1];

            if (inputUsername == username && inputPassword == password)
            {
                AnsiConsole.MarkupLine("[bold green]Login successful![/]");
                return true;
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Invalid username or password.[/]");
                return false;
            }
        }

        private void Register()
        {
            AnsiConsole.MarkupLine("[bold yellow]User not found. Please register.[/]");

            username = AnsiConsole.Ask<string>("[bold green]Enter username: [/]");
            password = AnsiConsole.Ask<string>("[bold green]Enter password: [/]");

            File.WriteAllLines(_userFilePath, new[] { username, password });

            AnsiConsole.MarkupLine("[bold green]Registration success![/]");
        }
    }
}
