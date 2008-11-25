using System;
using System.Collections;
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
                int endIndex = arg.IndexOfAny(new[] { ':', '+', '-' }, 1);
                string optionName = arg.Substring(1, endIndex == -1 ? arg.Length - 1 : endIndex - 1);

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
                    throw new CommandLineArgumentException(string.Format("The '/{0}' argument is exclusive and cannot " +
                        "be used with any other argument.", argument.Name));
                }

                // Determine the value of the argument
                object optionValue;
                if (endIndex == -1)
                {
                    // No value was specified, so it is most likely a flag
                    optionValue = ParseValue(argument.Type, null);
                }
                else if (arg[endIndex] == '+' || arg[endIndex] == '-')
                {
                    // Parse value with the '+' or '-'
                    optionValue = ParseValue(argument.Type, arg.Substring(endIndex));
                }
                else
                {
                    // Parse value without the ':'
                    optionValue = ParseValue(argument.Type, arg.Substring(endIndex + 1));
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

        public object ParseValue(Type type, string stringValue)
        {
            if (type == typeof(bool))
            {
                switch (stringValue)
                {
                    case null:
                    case "":
                    case "+":
                        return true;
                    case "-":
                        return false;
                }
            }
            else if (type == typeof(string))
            {
                return stringValue;
            }
            else if (type == typeof(int))
            {
                return int.Parse(stringValue);
            }
            else if (type == typeof(double))
            {
                return double.Parse(stringValue);
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, stringValue, true);
            }
            else if (type.IsArray)
            {
                ArrayList values = new ArrayList();

                if (type.GetElementType().IsEnum)
                {
                    foreach (string part in stringValue.Split(','))
                    {
                        values.Add(Enum.Parse(type.GetElementType(), part, true));
                    }
                    return values.ToArray(type.GetElementType());
                }

                return stringValue.Split(',');
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type underlyingType = type.GetGenericArguments()[0];
                return ParseValue(underlyingType, stringValue);
            }

            //TODO test
            throw new CommandLineArgumentException(string.Format("Unsupported argument type '{0}' or value '{1}'.", type.FullName, stringValue));
        }
    }
}