using System;
using NConsole.Internal;

namespace NConsole
{
    /// <summary>
    /// Controls the registeration of commands and execution of a console application.
    /// </summary>
    public class ConsoleController
    {
        private readonly ICommandRegistry commandRegistry;
        private readonly ICommandFactory commandFactory;

        /// <summary>
        /// Creates a new <see cref="ConsoleController"/>.
        /// </summary>
        public ConsoleController()
            : this(new DefaultCommandFactory())
        {
        }

        /// <summary>
        /// Creates a new <see cref="ConsoleController"/> with a custom <see cref="ICommandFactory"/>.
        /// </summary>
        /// <param name="commandFactory">A custom <see cref="ICommandFactory"/>.</param>
        public ConsoleController(ICommandFactory commandFactory)
            : this(new CommandRegistry(), commandFactory)
        {
        }

        private ConsoleController(ICommandRegistry commandRegistry, ICommandFactory commandFactory)
        {
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");

            this.commandRegistry = commandRegistry;
            this.commandFactory = commandFactory;
        }

        //TODO
        //public ArgumentMode Mode { get; set; }

        /// <summary>
        /// Registers a command so that it can be inspected.
        /// </summary>
        /// <param name="commandType">The type of the command to register.</param>
        public void Register(Type commandType)
        {
            if (commandType == null) throw new ArgumentNullException("commandType");

            commandRegistry.Register(commandType);
        }

        /// <summary>
        /// Configures the default command of the application.
        /// </summary>
        /// <param name="commandType">The type of the default command.</param>
        public void SetDefaultCommand(Type commandType)
        {
            if (commandType == null) throw new ArgumentNullException("commandType");

            commandRegistry.SetDefaultCommand(commandType);
        }

        /// <summary>
        /// Executes the appropriate command for the specified command line arguments.
        /// </summary>
        /// <param name="args">The Command line arguments.</param>
        /// <returns>The exit code that should be used when the process terminates.</returns>
        public int Execute(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");

            string commandName = string.Empty;
            if (args.Length >= 1)
            {
                commandName = args[0];
            }

            CommandDescriptor commandDescriptor = commandRegistry.GetDescriptor(commandName);
            if (commandDescriptor == null)
            {
                Console.Error.WriteLine("Command '{0}' is not recognized.", commandName);
                return 1;
            }

            ICommand command = commandFactory.Create(commandDescriptor.CommandType);

            //TODO: parse the args and poke them in

            command.Execute();

            return 0;
        }
    }
}