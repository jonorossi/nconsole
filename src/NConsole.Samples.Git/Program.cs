using System;

namespace NConsole.Samples.Git
{
    class Program
    {
        static int Main(string[] args)
        {
            ConsoleController controller = new ConsoleController();
            controller.Register(typeof(CloneCommand));
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
        public void Execute()
        {
            Console.WriteLine("In clone command");
        }
    }

//    [Command(Default = typeof(SourcesListCommand))]
//    internal class SourcesCommand : SubCommandUsageBase
//    {
//    }
//
//    [SubCommand(typeof(SourcesCommand), Name = new[] { "list" })]
//    internal class SourcesListCommand
//    {
//    }
//
//    [SubCommand(typeof(SourcesCommand), Name = new[] { "add" })]
//    internal class SourcesAddCommand
//    {
//    }
}