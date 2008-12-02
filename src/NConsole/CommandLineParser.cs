using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;

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

            // If an exclusive argument is found it is stored so that only this argument needs to be bound
            CommandLineArgument exclusiveArgument = null;

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

                // Store the exclusive argument for later use
                if (argument.IsExclusive)
                {
                    exclusiveArgument = argument;
                }

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
                foreach (CommandLineArgument argument in _arguments)
                {
                    argument.Bind(options);
                }
            }

            // Return the populated options class
            return options;
        }

        public string Usage
        {
            get
            {
                StringBuilder helpText = new StringBuilder();

                // Use either the entry executable assembly or the calling library
                Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

                // Usage header
                helpText.AppendFormat("Usage: {0} [options]", Path.GetFileNameWithoutExtension(assembly.CodeBase));
                helpText.AppendLine();

                // Output each option
                helpText.AppendLine("Options:");
                foreach (CommandLineArgument argument in _arguments)
                {
                    string optionUsage = "  /" + argument.Name;

                    // If the value type is nullable then use the underlying type instead
                    Type valueType = argument.ValueType;
                    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        valueType = valueType.GetGenericArguments()[0];
                    }

                    // Append specific usage by the value type
                    if (valueType == typeof(bool))
                    {
                        optionUsage += "[+|-]";
                    }
                    else if (valueType == typeof(string))
                    {
                        optionUsage += ":<text>";
                    }
                    else if (valueType == typeof(int))
                    {
                        optionUsage += ":<number>";
                    }
                    else if (valueType.IsEnum)
                    {
                        optionUsage += ":<";
                        foreach (string enumName in Enum.GetNames(valueType))
                        {
                            optionUsage += enumName.ToLower() + "|";
                        }
                        optionUsage += ">";
                    }

                    // Append description
                    if (!string.IsNullOrEmpty(argument.Description))
                    {
                        // Pad the argument definition so that the descriptions line up
                        optionUsage = string.Format("{0,-28}  {1}", optionUsage, argument.Description);
                    }

                    helpText.AppendLine(optionUsage);
                }

                return helpText.ToString();
            }
        }

//        public void OutputDebug()
//        {
//            Console.WriteLine("Mode: {0}", options.Mode);
//            Console.WriteLine("Server: {0}", options.Server);
//            Console.WriteLine("Database: {0}", options.Database);
//            Console.WriteLine("Timeout: {0} (HasValue: {1})", options.Timeout.GetValueOrDefault(), options.Timeout.HasValue);
//            Console.WriteLine("DbObjects [Length: {0}] {{", options.DbObjects.Length);
//            foreach (string dbObject in options.DbObjects)
//            {
//                Console.WriteLine("    {0}", dbObject);
//            }
//            Console.WriteLine("}");
//        }

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
                //TODO test
                throw new CommandLineArgumentException("Boolean arguments can only be followed by '', '+' or '-'.");
            }
            else if (type == typeof(string))
            {
                // Remove starting and trailing double quotes if they are there
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    return value.Substring(1, value.Length - 2);
                }

                return value;
            }
            else if (type == typeof(int))
            {
                return int.Parse(value);
            }
            else if (type == typeof(double))
            {
                return double.Parse(value);
            }
            else if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }
            else if (type.IsArray)
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
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type underlyingType = type.GetGenericArguments()[0];
                return ParseValue(underlyingType, value);
            }

            throw new CommandLineArgumentException(string.Format("Unsupported argument type '{0}' or value '{1}'.", type.FullName, value));
        }
    }
}