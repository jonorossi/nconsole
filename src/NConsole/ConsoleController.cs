using System;
using System.Collections.Generic;
using System.Reflection;

namespace NConsole
{
    //TODO: Make the execute method catch all exceptions and handle writing to stderr

    public class ConsoleController
    {
        //private readonly List<CommandDescriptor> descriptors = new List<CommandDescriptor>();

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
        {
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");

            this.commandFactory = commandFactory;
        }

        //public ArgumentMode Mode { get; set; }

        public Type DefaultCommand { get; set; }

        /// <summary>
        /// Executes the appropriate command for the provided command line arguments.
        /// </summary>
        /// <param name="args">The Command line arguments.</param>
        /// <returns>The exit code that should be used when the process terminates.</returns>
        public int Execute(string[] args)
        {
            ICommand command = commandFactory.Create(DefaultCommand/*args[0]*/);

            Type commandType = DefaultCommand;

            foreach (string arg in args)
            {
                int endOfName = arg.IndexOf('=');

                string argName = endOfName >= 0 ? arg.Substring(0, endOfName) : arg;

                // Build up a list of arguments that will later be used for parsing
                PropertyInfo[] properties = commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.IsDefined(typeof(ArgumentAttribute), true))
                    {
                        var attribute = (ArgumentAttribute)Attribute.GetCustomAttribute(
                            propertyInfo, typeof(ArgumentAttribute), true);

                        //TODO: Build an ArgumentDescriptor collecting metadata from the property and ArgumentAttribute

                        if (argName == "-" + attribute.ShortName ||
                            argName == "--" + attribute.LongName)
                        {
                            if (propertyInfo.PropertyType == typeof(bool))
                            {
                                propertyInfo.SetValue(command, true, null);
                            }
                            else if (propertyInfo.PropertyType == typeof(string[]))
                            {
                                List<string> values = new List<string>();

                                object currentValue = propertyInfo.GetValue(command, null);
                                if (currentValue != null)
                                {
                                    values.AddRange(((string[])currentValue));
                                }

                                values.Add(arg.Substring(endOfName + 1));

                                propertyInfo.SetValue(command, values.ToArray(), null);
                            }
                            else
                            {
                                throw new NotSupportedException("unsupported argument type " +
                                    propertyInfo.PropertyType.FullName);
                            }

                            //if (propertyInfo.CanWrite/* || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)*/)
                        }
                    }
                }
            }

            command.Execute();

            return 0;
        }
    }
}