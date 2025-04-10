using Spectre.Console;
using System;
using System.Collections.Generic;

namespace GardenTracker
{
    public class ConsoleUI
    {
        public T PromptSelection<T>(string title, IEnumerable<T> choices, Func<T, string> converter = null)
        {
            var prompt = new SelectionPrompt<T>()
                .Title($"[bold green]{title}[/]")
                .AddChoices(choices);

            if (converter != null)
            {
                prompt.UseConverter(converter);
            }

            return AnsiConsole.Prompt(prompt);
        }

        public string AskString(string question)
        {
            return AnsiConsole.Ask<string>($"[bold green]{question}[/]").Trim();
        }

        public int AskInt(string question)
        {
            return AnsiConsole.Ask<int>($"[bold green]{question}[/]");
        }

        public bool Confirm(string question)
        {
            return AnsiConsole.Confirm($"[bold green]{question}[/]");
        }

        public void PrintMessage(string message, string color = "green")
        {
            AnsiConsole.MarkupLine($"[bold {color}]{message}[/]");
        }
    }
}
