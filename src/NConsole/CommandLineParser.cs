using System;
using System.Reflection;

namespace NConsole
{
    //TODO: Should the options class become an interface? Does a class give you anything an interface doesn't?

    public class CommandLineParser<TOptions> where TOptions : class, new()
    {
        private readonly CommandLineArgumentCollection _arguments = new CommandLineArgumentCollection();

        public CommandLineParser()
        {
            // Build up a list of command line arguments that will later be used for parsing
            foreach (PropertyInfo propertyInfo in typeof(TOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.CanWrite/* || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)*/)
                {
                    // Attempt to build an argument from a CommandLineArgumentAttribute
                    object[] attributes = propertyInfo.GetCustomAttributes(typeof(CommandLineArgumentAttribute), false);
                    if (attributes.Length == 1)
                    {
                        _arguments.Add(new CommandLineArgument((CommandLineArgumentAttribute)attributes[0], propertyInfo));
                    }
                }
            }
        }

        public TOptions ParseArguments(string[] args)
        {
            // Check arguments
            if (args == null)
            {
                throw new ArgumentNullException("args", "Command line arguments are null.");
            }

            // Parse each command line argument
            foreach (string arg in args)
            {
                // Ensure the argument isn't empty
                if (string.IsNullOrEmpty(arg))
                {
                    continue;
                }

                // If the string does not start a with the start of a command line argument
//                if (arg[0] != '/')
//                {
//                    if (_defaultArgument != null)
//                    {
//                        _defaultArgument.SetValue(argument);
//                    }
//                    else
//                    {
//                        throw new CommandLineArgumentException(string.Format("Unsupported argument '{0}'.", arg));
//                    }
//                }

                // Determine the name of the argument
                //int endIndex = arg.IndexOfAny(new char[] { ':', '+', '-' }, 1);
                //string optionName = arg.Substring(1, endIndex == -1 ? arg.Length - 1 : endIndex - 1);
                string optionName = arg.Substring(1, arg.Length - 1);

                // Determine the value of the argument
                object optionValue;
                optionValue = true;

                // Attempt to get the argument
                CommandLineArgument argument = _arguments[optionName];
                if (argument == null)
                {
                    //TODO make sure this is tested
                    throw new CommandLineArgumentException(string.Format("Unknown argument '{0}'.", arg));
                }

                // If this argument is exclusive and multiple arguments have been passed on the command line then throw an exception
                if (argument.IsExclusive && args.Length > 1)
                {
                    throw new CommandLineArgumentException(string.Format("The '/{0}' argument is exclusive and cannot be used with any other argument.", argument.Name));
                }

                // Set the value of the option
                argument.SetValue(optionValue);
            }

            // Create an options object that will be populated with the command line argument details
            TOptions options = new TOptions();

            // Populate the options object with the values from the arguments
            foreach (CommandLineArgument argument in _arguments)
            {
                argument.Bind(options);
            }

            // Return the populated options class
            return options;
        }
    }
}