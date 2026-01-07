using System;
using System.IO;
using Cosmos.System;

namespace bfbOS
{
    public class Kernel : Cosmos.System.Kernel
    {
        private CommandManager cmd;

        protected override void BeforeRun()
        {
            Console.Clear();

            // Load boot message: prefer 0:\BootMessage.txt, fallback to BootConfig.DefaultBootMessage
            string bootMessage = BootConfig.DefaultBootMessage;
            try
            {
                // If you have a mounted drive in Cosmos, it's usually accessible like "0:\"
                if (File.Exists(@"0:\BootMessage.txt"))
                {
                    bootMessage = File.ReadAllText(@"0:\BootMessage.txt");
                }
            }
            catch (Exception)
            {
                // ignore; keep default
            }

            // Print boot message line by line
            foreach (var line in bootMessage.Replace("\r", "").Split('\n'))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();

            // Initialize command manager and register commands
            cmd = new CommandManager();

            // Built-in commands (examples)
            cmd.Register("help", "Show available commands", args => { cmd.PrintHelp(); });
            cmd.Register("echo", "Echo arguments back", args =>
            {
                if (args.Length > 0) Console.WriteLine(string.Join(" ", args));
            });
            cmd.Register("cls", "Clear the screen", args => { Console.Clear(); });
            cmd.Register("time", "Show current date & time", args => { Console.WriteLine(DateTime.Now.ToString()); });
            cmd.Register("reboot", "Reboot the machine", args =>
            {
                try
                {
                    Cosmos.System.Power.Reboot();
                }
                catch (Exception)
                {
                    Console.WriteLine("Reboot not available in this environment.");
                }
            });
            cmd.Register("fourfetch", "Gets system information.", args => { Console.WriteLine("SYSTEM: bfbOS v0.2/nFOURFETCH VER: 0.1beta"); });

            // Example: how to add your own command
            // cmd.Register("greet", "Greet the user", args => { Console.WriteLine("Hello from bfbOS!"); });
        }

        protected override void Run()
        {
            Console.Write("bfbOS> ");
            string line = Console.ReadLine();
            if (line == null) return;

            var parts = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            var name = parts[0].ToLower();
            var args = new string[parts.Length - 1];
            for (int i = 1; i < parts.Length; i++) args[i - 1] = parts[i];

            if (!cmd.Execute(name, args))
            {
                Console.WriteLine("Unknown command. Type 'help'.");
            }
        }
    }
}
