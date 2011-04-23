using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        private readonly List<IArgumentParser> argumentParsers = new List<IArgumentParser>();

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

            InitializeArgumentParsers();
        }

        //TODO
        //public ArgumentMode Mode { get; set; }

        /// <summary>
        /// Gets or sets whether caught exceptions should be rethrown to ease debugging.
        /// </summary>
        public bool RethrowExceptions { get; set; }

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

            try
            {
                Execute(new List<string>(args));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.ResetColor();

                // Rethrow the exception if configured
                if (RethrowExceptions)
                {
                    throw;
                }

                return 1;
            }

            return 0;
        }

        private void Execute(List<string> args)
        {
            string commandName = string.Empty;
            if (args.Count >= 1 && Regex.IsMatch(args[0], "^[a-z]+$"))
            {
                commandName = args[0];
                args.RemoveAt(0); // Remove the argument we just used
            }

            // Attempt to find the command for the name
            CommandDescriptor commandDescriptor = commandRegistry.GetDescriptor(commandName, null);
            if (commandDescriptor == null)
            {
                throw new Exception(string.Format("Command '{0}' is not recognized.", commandName));
            }

            // Determine if this is a call for a sub command
            while (args.Count >= 1 && Regex.IsMatch(args[0], "^[a-z]+$"))
            {
                commandName = args[0];
                var subCommandDescriptor = commandRegistry.GetDescriptor(commandName, commandDescriptor);
                if (subCommandDescriptor == null)
                {
                    break;
                }

                commandDescriptor = subCommandDescriptor;
                args.RemoveAt(0); // Remove the argument we just used
            }

            // Create an instance of the required command
            ICommand command = commandFactory.Create(commandDescriptor.CommandType);

            // Parse the arguments and apply them to the command
            if (args.Count > 0)
            {
                foreach (IArgumentParser argumentParser in argumentParsers)
                {
                    argumentParser.Apply(args, command, commandDescriptor);
                }

                // Ensure that all the arguments have been parsed
                if (args.Count > 0)
                {
                    throw new Exception(string.Format("Unsupported argument '{0}'.", args[0]));
                }
            }

            // Execute the code of the command
            command.Execute();
        }

        private void InitializeArgumentParsers()
        {
            argumentParsers.Add(new NamedArgumentParser());
            argumentParsers.Add(new PositionalArgumentParser());
        }
    }
}