using System;
using System.IO;

namespace NConsole.Samples.Ls
{
    class Program : ICommand
    {
        static int Main(string[] args)
        {
            return ConsoleApplication.Run(args, typeof(Program));
        }

        public void Execute()
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory());

            // Directories
            using (new ConsoleColorScope(ConsoleColor.Yellow))
            {
                foreach (DirectoryInfo directory in dir.GetDirectories())
                {
                    Console.WriteLine(directory.Name);
                }
            }

            // Files
            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (new ConsoleColorScope(ConsoleColor.Green))
                    {
                        Console.WriteLine(file.Name);
                    }
                }
                else
                {
                    Console.WriteLine(file.Name);
                }
            }
        }

        [Argument(Position = 0)]
        public string[] Paths { get; set; }

//        [Argument(ShortName = "l")]
//        public bool LongListingFormat { get; set; }
//
//        [Argument(ShortName = "h", LongName = "human-readable")]
//        public bool HumanReadableSizes { get; set; }
//
//        [Argument(ShortName = "S")]
//        public bool SortByFileSize { get; set; }
//
//        [Argument(ShortName = "I", LongName = "ignore")]
//        public string[] IgnorePatterns { get; set; }
    }

    internal class ConsoleColorScope : IDisposable
    {
        public ConsoleColorScope(ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
        }

        public void Dispose()
        {
            Console.ResetColor();
        }
    }
}