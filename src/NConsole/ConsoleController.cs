using System;
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
            // If we don't have a default command then exit
            if (DefaultCommand == null)
            {
                return 1;
            }

            ICommand command = commandFactory.Create(DefaultCommand/*args[0]*/);

            Type commandType = DefaultCommand;

            // Build up a list of arguments that will later be used for parsing
            foreach (PropertyInfo propertyInfo in commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.IsDefined(typeof(ArgumentAttribute), true))
                {
                    var attribute = (ArgumentAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ArgumentAttribute), true);

                    //TODO: Build an ArgumentDescriptor collecting metadata from the property and ArgumentAttribute

                    foreach (string arg in args)
                    {
                        if (arg == "-" + attribute.ShortName ||
                            arg == "--" + attribute.LongName)
                        {
                            propertyInfo.SetValue(command, true, null);
                        }
                    }

//                    if (propertyInfo.CanWrite/* || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)*/)
//                    {
//                    }
                }
            }

            command.Execute();

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