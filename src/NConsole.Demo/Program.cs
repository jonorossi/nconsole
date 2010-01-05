using System;

namespace NConsole.Demo
{
    class Program
    {
        static int Main(string[] args)
        {
//            // Parse the command line arguments
//            CommandLineParser<AppCommand> parser = new CommandLineParser<AppCommand>();
//            AppCommand options;
//            try
//            {
//                options = parser.ParseArguments(args);
//            }
//            catch (CommandLineArgumentException claex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine("ERROR: " + claex.Message);
//                Console.ResetColor();
//
//                Console.WriteLine();
//                Console.WriteLine(parser.Usage);
//
//                return 1;
//            }
//
//            if (options.ShowHelp)
//            {
//                Console.WriteLine(parser.Usage);
//                return 0;
//            }
//
//            if (options.Mode == AppCommand.AppMode.All)
//            {
//                // ... Normal application operation ...
//            }

            return 0;
        }
    }

    internal class AppCommand : ICommand
    {
        internal enum AppMode { All, Mode1, Mode2 }

//        [Argument(Mandatory = true, Description = "Select the function for the program to perform")]
//        public AppMode Mode { get; set; }
//
//        [Argument(Description = "The server to connect to")]
//        public string Server { get; set; }
//
//        [Argument("db", Description = "The database to use")]
//        public string Database { get; set; }
//
//        [Argument(Description = "The total amount of time to watch for the operations to complete")]
//        public int? Timeout { get; set; }
//
//        [Argument(Description = "The database objects to include in the functions")]
//        public string[] DbObjects { get; set; }
//
//        [Argument("help", Exclusive = true, Description = "Display this help text")]
//        public bool ShowHelp { get; set; }

//        Console.WriteLine("  /nologo\t\tSuppress version and copyright message");
//        Console.WriteLine("  /version\t\tDisplay software and configuration versions");
//        Console.WriteLine("  /target:<targets>\t\tSpecify targets to run (Short form: /t)");

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}