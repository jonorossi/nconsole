﻿using System;

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
        [Argument(Position = 0/*, Required = true*/)]
        public string Repository { get; set; }

        [Argument]
        public bool Quiet { get; set; }

        public void Execute()
        {
            Console.WriteLine("Clone {{ Repository={0}, Quiet={1} }}", Repository, Quiet);
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