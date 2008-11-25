using System;

namespace NConsole.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandLineParser<CommandLineOptions>();
            CommandLineOptions options = parser.ParseArguments(args);

            Console.WriteLine("Mode: {0}", options.Mode);
            Console.WriteLine("Server: {0}", options.Server);
            Console.WriteLine("Database: {0}", options.Database);
            Console.WriteLine("Timeout: {0} (HasValue: {1})", options.Timeout.GetValueOrDefault(), options.Timeout.HasValue);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }

    internal class CommandLineOptions
    {
        internal enum AppMode
        {
            All, Mode1, Mode2
        }

        [CommandLineArgument(Required = true)]
        public AppMode Mode { get; set; }

        [CommandLineArgument]
        public string Server { get; set; }

        [CommandLineArgument("db")]
        public string Database { get; set; }

        [CommandLineArgument]
        public int? Timeout { get; set; }
    }
}