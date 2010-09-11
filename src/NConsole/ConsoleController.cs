using System;
using NConsole.Internal;

namespace NConsole
{
    //TODO: Make the execute method catch all exceptions and handle writing to stderr

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
            command.Execute();

            //Type commandType = DefaultCommand;

//            foreach (string arg in args)
//            {
//                int endOfName = arg.IndexOf('=');
//
//                string argName = endOfName >= 0 ? arg.Substring(0, endOfName) : arg;
//
                // Build up a list of arguments that will later be used for parsing
//                PropertyInfo[] properties = commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
//                foreach (PropertyInfo propertyInfo in properties)
//                {
//                    if (propertyInfo.IsDefined(typeof(ArgumentAttribute), true))
//                    {
//                        var attribute = (ArgumentAttribute)Attribute.GetCustomAttribute(
//                            propertyInfo, typeof(ArgumentAttribute), true);
//
                        //TODO: Build an ArgumentDescriptor collecting metadata from the property and ArgumentAttribute
//
//                        if (argName == "-" + attribute.ShortName ||
//                            argName == "--" + attribute.LongName)
//                        {
//                            if (propertyInfo.PropertyType == typeof(bool))
//                            {
//                                propertyInfo.SetValue(command, true, null);
//                            }
//                            else if (propertyInfo.PropertyType == typeof(string[]))
//                            {
//                                List<string> values = new List<string>();
//
//                                object currentValue = propertyInfo.GetValue(command, null);
//                                if (currentValue != null)
//                                {
//                                    values.AddRange(((string[])currentValue));
//                                }
//
//                                values.Add(arg.Substring(endOfName + 1));
//
//                                propertyInfo.SetValue(command, values.ToArray(), null);
//                            }
//                            else
//                            {
//                                throw new NotSupportedException("unsupported argument type " +
//                                    propertyInfo.PropertyType.FullName);
//                            }
//
                            //if (propertyInfo.CanWrite/* || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)*/)
//                        }
//                    }
//                }
//            }

            return 0;
        }
    }
}