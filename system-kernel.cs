using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;

namespace bfdios2763test
{
    public class Kernel : Sys.Kernel
    {

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
        }

        protected override void Run()
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (input == "about")
            {
                Console.WriteLine("bfbOS v0.1/nMade by FSVAnim/nBFB by Jacknjellify");
            }
            else if (input == "exit")
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            }
            else if (input == "help")
            {
                Console.WriteLine("Commands:\nhelp: shows the help\nabout: shows the version of the OS\nexit: shutdown the OS");
            }
            else
            {
                Console.WriteLine(input + " is not a command");
            }
            Console.Write("Text typed: ");
            Console.WriteLine(input);
        }
    }
}
