using System;
using System.Collections.Generic;

namespace bfbOS
{
    // Very small command registry — register commands with a name, description and action
    public class CommandManager
    {
        private readonly Dictionary<string, Command> commands = new Dictionary<string, Command>();

        public void Register(string name, string description, Action<string[]> action)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name");
            commands[name.ToLower()] = new Command { Name = name, Description = description, Action = action };
        }

        public bool Execute(string name, string[] args)
        {
            if (commands.TryGetValue(name.ToLower(), out var cmd))
            {
                try
                {
                    cmd.Action(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Command error: " + ex.Message);
                }
                return true;
            }
            return false;
        }

        public void PrintHelp()
        {
            Console.WriteLine("Available commands:");
            foreach (var kv in commands)
            {
                Console.WriteLine($"  {kv.Value.Name} - {kv.Value.Description}");
            }
        }

        private class Command
        {
            public string Name;
            public string Description;
            public Action<string[]> Action;
        }
    }
}