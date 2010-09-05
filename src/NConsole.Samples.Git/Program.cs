using System;

namespace NConsole.Samples.Git
{
    class Program
    {
        static int Main(string[] args)
        {
            ConsoleController controller = new ConsoleController();
            controller.Register(typeof(CloneCommand));
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
}