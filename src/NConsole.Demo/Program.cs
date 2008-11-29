using System;

namespace NConsole.Demo
{
    class Program
    {
        static int Main(string[] args)
        {
            // Parse the command line arguments
            CommandLineParser<CommandLineOptions> parser = new CommandLineParser<CommandLineOptions>();
            CommandLineOptions options;
            try
            {
                options = parser.ParseArguments(args);
            }
            catch (CommandLineArgumentException claex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + claex.Message);
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine(parser.Usage);

                return 1;
            }

            if (options.ShowHelp)
            {
                Console.WriteLine(parser.Usage);
                return 0;
            }

            if (options.Mode == CommandLineOptions.AppMode.All)
            {
                // Normal application operation
            }

            return 0;
        }
    }

    internal class CommandLineOptions
    {
        internal enum AppMode { All, Mode1, Mode2 }

        [CommandLineArgument("help", Exclusive = true, Description = "Display this help text")]
        public bool ShowHelp { get; set; }

        [CommandLineArgument(Required = true)]
        public AppMode Mode { get; set; }

        [CommandLineArgument]
        public string Server { get; set; }

        [CommandLineArgument("db")]
        public string Database { get; set; }

        [CommandLineArgument]
        public int? Timeout { get; set; }

        [CommandLineArgument]
        public string[] DbObjects { get; set; }
    }
}