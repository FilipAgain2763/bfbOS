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

            string bootMessage = BootConfig.DefaultBootMessage;
            try
            {
                if (File.Exists(@"0:\BootMessage.txt"))
                {
                    bootMessage = File.ReadAllText(@"0:\BootMessage.txt");
                }
            }
            catch (Exception)
            {
            }

            foreach (var line in bootMessage.Replace("\r", "").Split('\n'))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();
            
            cmd = new CommandManager();

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
