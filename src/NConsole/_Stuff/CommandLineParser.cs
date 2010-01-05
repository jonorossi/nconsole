using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

namespace NConsole
{
    /// <summary>
    /// Provides parsing of command line arguments.
    /// </summary>
    /// <typeparam name="TOptions">
    ///   The options class that provides metadata about the allowed arguments. The
    ///   <see cref="CommandLineParser{TOptions}" /> will return an instance of this type when parsing.
    /// </typeparam>
    public class CommandLineParser<TOptions> where TOptions : class, new()
    {
        private readonly CommandLineArgumentCollection _arguments = new CommandLineArgumentCollection();

        /// <summary>
        /// Creates a new <see cref="CommandLineParser{TOptions}"/>.
        /// </summary>
        public CommandLineParser()
        {
            // Build up a list of command line arguments that will later be used for parsing
            foreach (PropertyInfo propertyInfo in typeof(TOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.CanWrite/* || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)*/)
                {
                    // Attempt to build an argument from a ArgumentAttribute
                    object[] attributes = propertyInfo.GetCustomAttributes(typeof(ArgumentAttribute), false);
                    if (attributes.Length == 1)
                    {
                        _arguments.Add(new Argument((ArgumentAttribute)attributes[0], propertyInfo));
                    }
                }
            }
        }

        /// <summary>
        /// Parses the comment line arguments and builds an options object the represents the arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The options object that represents the command line arguments.</returns>
        public TOptions ParseArguments(string[] args)
        {
            // Check arguments
            if (args == null)
            {
                throw new ArgumentNullException("args", "Command line arguments are null.");
            }

            // If an exclusive argument is found it is stored so that only this argument needs to be bound
            Argument exclusiveArgument = null;

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
                Argument argument = _arguments[optionName];
                if (argument == null)
                {
                    throw new CommandLineArgumentException(string.Format("Unknown argument '{0}'.", arg));
                }

                // If this argument is exclusive and multiple arguments have been passed on the command line then throw an exception
//                if (argument.IsExclusive && args.Length > 1)
//                {
//                    throw new CommandLineArgumentException(string.Format("The '/{0}' argument is exclusive and cannot " +
//                        "be used with any other argument.", argument.Name));
//                }

                // Store the exclusive argument for later use
//                if (argument.IsExclusive)
//                {
//                    exclusiveArgument = argument;
//                }

                // Determine the value of the argument
                object optionValue;
                if (endIndex == -1)
                {
                    // No value was specified, so it is most likely a flag
                    optionValue = ParseValue(argument.ValueType, null);
                }
                else if (arg[endIndex] == '+' || arg[endIndex] == '-')
                {
                    // Parse value with the '+' or '-'
                    optionValue = ParseValue(argument.ValueType, arg.Substring(endIndex));
                }
                else
                {
                    // Parse value without the ':'
                    optionValue = ParseValue(argument.ValueType, arg.Substring(endIndex + 1));
                }

                // Set the value of the option
                argument.SetValue(optionValue);
            }

            // Create an options object that will be populated with the command line argument details
            TOptions options = new TOptions();

            // Populate the options object with the values from the arguments
            if (exclusiveArgument != null)
            {
                exclusiveArgument.Bind(options);
            }
            else
            {
                foreach (Argument argument in _arguments)
                {
                    argument.Bind(options);
                }
            }

            // Return the populated options class
            return options;
        }

        /// <summary>
        /// Provides detailed usage information of each command line argument to display to the user.
        /// </summary>
        public string Usage
        {
            get
            {
                StringBuilder helpText = new StringBuilder();

                // Use either the entry executable assembly or the calling library
                Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

                // Usage header
                helpText.AppendFormat("Usage: {0}", Path.GetFileNameWithoutExtension(assembly.CodeBase));

                // Required arguments in the header
                foreach (Argument argument in _arguments)
                {
//                    if (argument.IsRequired)
//                    {
//                        helpText.AppendFormat(" /{0}{1}", argument.Name, GetOptionUsage(argument.ValueType));
//                    }
                }

                // End of usage header
                helpText.AppendLine(" [options]");

                // Output each option
                helpText.AppendLine("Options:");
                foreach (Argument argument in _arguments)
                {
                    string optionUsage = "  /" + argument.Name;

                    // If the value type is nullable then use the underlying type instead
                    Type valueType = argument.ValueType;
                    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        valueType = valueType.GetGenericArguments()[0];
                    }

                    // Append specific usage by the value type
                    optionUsage += GetOptionUsage(valueType);

                    // Append description
//                    if (!string.IsNullOrEmpty(argument.Description))
//                    {
//                        // Pad the argument definition so that the descriptions line up
//                        optionUsage = string.Format("{0,-28}  {1}", optionUsage, argument.Description);
//                    }

                    helpText.AppendLine(optionUsage);
                }

                return helpText.ToString();
            }
        }

        private static string GetOptionUsage(Type valueType)
        {
            if (valueType == typeof(bool))
            {
                return "[+|-]";
            }
            if (valueType == typeof(string))
            {
                return ":<text>";
            }
            if (valueType == typeof(int))
            {
                return ":<number>";
            }
            if (valueType.IsEnum)
            {
                return ":<" + string.Join("|", Enum.GetNames(valueType)).ToLower() + ">";
            }
            if (valueType.IsArray)
            {
                return GetOptionUsage(valueType.GetElementType()) + "[,...]";
            }
            return string.Empty;
        }

        internal object ParseValue(Type type, string value)
        {
            if (type == typeof(bool))
            {
                if (string.IsNullOrEmpty(value) || value == "+")
                {
                    return true;
                }
                if (value == "-")
                {
                    return false;
                }
                throw new CommandLineArgumentException("Boolean arguments can only be followed by '', '+' or '-'.");
            }
            if (type == typeof(string))
            {
                // Remove starting and trailing double quotes if they are there
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    //TODO: This needs to support escaped quotes within the string
                    return value.Substring(1, value.Length - 2);
                }
                return value;
            }
            if (type == typeof(int))
            {
                return int.Parse(value);
            }
            if (type == typeof(double))
            {
                return double.Parse(value);
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }
            if (type.IsArray)
            {
                // Determine the type of the array's elements
                Type elementType;
                if (type.GetElementType().IsEnum)
                {
                    elementType = type.GetElementType();
                }
                else
                {
                    elementType = typeof(string);
                }

                // Parse each item and return them
                ArrayList values = new ArrayList();
                foreach (string item in value.Split(','))
                {
                    values.Add(ParseValue(elementType, item));
                }

                return values.ToArray(type.GetElementType());
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type underlyingType = type.GetGenericArguments()[0];
                return ParseValue(underlyingType, value);
            }
            throw new CommandLineArgumentException(string.Format("Unsupported argument type '{0}' or value '{1}'.", type.FullName, value));
        }
    }
}