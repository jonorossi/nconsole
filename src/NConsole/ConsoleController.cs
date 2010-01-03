using System;

namespace NConsole
{
    //TODO: Make the execute method catch all exceptions and handle writing to stderr

    public class ConsoleController
    {
        private readonly ICommandFactory commandFactory;

        public ConsoleController()
//            : this(new DefaultCommandFactory())
        {
        }

        public ConsoleController(ICommandFactory commandFactory)
        {
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");

            this.commandFactory = commandFactory;
        }

//        public ICommandFactory CommandFactory { get; set; }
//        public Mode Mode { get; set; }
//        public Type DefaultCommand { get; set; }

        public int Execute(string[] args)
        {
            if (args.Length > 0)
            {
                commandFactory.Resolve(args[0]).Execute();
            }

            return 0;

            // With this:
            //     clone --quiet http://example.com/app.git
            // It will:
            //     command = new CloneCommand();
            //     command.Quiet = true;
            //     command.Repository = "...";
            //     command.Execute();
        }
    }
}