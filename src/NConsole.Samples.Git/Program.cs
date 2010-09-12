using System;

namespace NConsole.Samples.Git
{
    class Program
    {
        static int Main(string[] args)
        {
            ConsoleController controller = new ConsoleController();
            controller.Register(typeof(CloneCommand));
            controller.Register(typeof(CommitCommand));
            controller.SetDefaultCommand(typeof(CloneCommand));
            return controller.Execute(args);

            //TODO
            // With this:
            //     clone --quiet http://example.com/app.git
            // It will:
            //     command = new CloneCommand();
            //     command.Quiet = true;
            //     command.Repository = "...";
            //     command.Execute();
        }
    }

    internal class CloneCommand : ICommand
    {
        [Argument(Position = 0/*, Required = true*/)]
        public string/*Uri*/ Repository { get; set; }

        //[Argument]
        //public DirectoryInfo Directory { get; set; }

        [Argument(ShortName = "q", LongName = "quiet")]
        public bool Quiet { get; set; }

        public void Execute()
        {
            Console.WriteLine("Clone {{ Repository={0}, Quiet={1} }}", Repository, Quiet);
        }
    }

    public class CommitCommand : ICommand
    {
        [Argument]
        public string Message { get; set; }

        public void Execute()
        {
            Console.WriteLine("Commit {{ Message={0} }}", Message);
        }
    }

//    [Command(DefaultSubCommand = typeof(SourcesListCommand))]
//    internal class SourcesCommand : SubCommandUsageBase
//    {
//    }
//
//    [SubCommand(typeof(SourcesCommand), Names = new[] { "list" })]
//    internal class SourcesListCommand
//    {
//    }
//
//    [SubCommand(typeof(SourcesCommand), Name = "add")]
//    internal class SourcesAddCommand
//    {
//    }
}