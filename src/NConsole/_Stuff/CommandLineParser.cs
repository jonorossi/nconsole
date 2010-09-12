//using System;
//using System.Collections;
//using System.IO;
//using System.Reflection;
//using System.Text;

//namespace NConsole
//{
//    /// <summary>
//    /// Provides parsing of command line arguments.
//    /// </summary>
//    /// <typeparam name="TOptions">
//    ///   The options class that provides metadata about the allowed arguments. The
//    ///   <see cref="CommandLineParser{TOptions}" /> will return an instance of this type when parsing.
//    /// </typeparam>
//    public class CommandLineParser<TOptions> where TOptions : class, new()
//    {
//        private readonly CommandLineArgumentCollection _arguments = new CommandLineArgumentCollection();

//        /// <summary>
//        /// Creates a new <see cref="CommandLineParser{TOptions}"/>.
//        /// </summary>
//        public CommandLineParser()
//        {
//            // Build up a list of command line arguments that will later be used for parsing
//            foreach (PropertyInfo propertyInfo in typeof(TOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance))
//            {
//                if (propertyInfo.CanWrite/* || typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType)*/)
//                {
//                    // Attempt to build an argument from a ArgumentAttribute
//                    object[] attributes = propertyInfo.GetCustomAttributes(typeof(ArgumentAttribute), false);
//                    if (attributes.Length == 1)
//                    {
//                        _arguments.Add(new Argument((ArgumentAttribute)attributes[0], propertyInfo));
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Parses the comment line arguments and builds an options object the represents the arguments.
//        /// </summary>
//        /// <param name="args">The command line arguments.</param>
//        /// <returns>The options object that represents the command line arguments.</returns>
//        public TOptions ParseArguments(string[] args)
//        {
//            // Check arguments
//            if (args == null)
//            {
//                throw new ArgumentNullException("args", "Command line arguments are null.");
//            }

//            // If an exclusive argument is found it is stored so that only this argument needs to be bound
//            Argument exclusiveArgument = null;

//            // Parse each command line argument
//            foreach (string arg in args)
//            {
//                // Ensure the argument isn't empty
//                if (string.IsNullOrEmpty(arg))
//                {
//                    continue;
//                }





//                // If this argument is exclusive and multiple arguments have been passed on the command line then throw an exception
////                if (argument.IsExclusive && args.Length > 1)
////                {
////                    throw new CommandLineArgumentException(string.Format("The '/{0}' argument is exclusive and cannot " +
////                        "be used with any other argument.", argument.Name));
////                }

//                // Store the exclusive argument for later use
////                if (argument.IsExclusive)
////                {
////                    exclusiveArgument = argument;
////                }


//            }

//            // Create an options object that will be populated with the command line argument details
//            TOptions options = new TOptions();

//            // Populate the options object with the values from the arguments
//            if (exclusiveArgument != null)
//            {
//                exclusiveArgument.Bind(options);
//            }
//            else
//            {
//                foreach (Argument argument in _arguments)
//                {
//                    argument.Bind(options);
//                }
//            }

//            // Return the populated options class
//            return options;
//        }

//        /// <summary>
//        /// Provides detailed usage information of each command line argument to display to the user.
//        /// </summary>
//        public string Usage
//        {
//            get
//            {
//                StringBuilder helpText = new StringBuilder();

//                // Use either the entry executable assembly or the calling library
//                Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

//                // Usage header
//                helpText.AppendFormat("Usage: {0}", Path.GetFileNameWithoutExtension(assembly.CodeBase));

//                // Required arguments in the header
//                foreach (Argument argument in _arguments)
//                {
////                    if (argument.IsRequired)
////                    {
////                        helpText.AppendFormat(" /{0}{1}", argument.Name, GetOptionUsage(argument.ValueType));
////                    }
//                }

//                // End of usage header
//                helpText.AppendLine(" [options]");

//                // Output each option
//                helpText.AppendLine("Options:");
//                foreach (Argument argument in _arguments)
//                {
//                    string optionUsage = "  /" + argument.Name;

//                    // If the value type is nullable then use the underlying type instead
//                    Type valueType = argument.ValueType;
//                    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
//                    {
//                        valueType = valueType.GetGenericArguments()[0];
//                    }

//                    // Append specific usage by the value type
//                    optionUsage += GetOptionUsage(valueType);

//                    // Append description
////                    if (!string.IsNullOrEmpty(argument.Description))
////                    {
////                        // Pad the argument definition so that the descriptions line up
////                        optionUsage = string.Format("{0,-28}  {1}", optionUsage, argument.Description);
////                    }

//                    helpText.AppendLine(optionUsage);
//                }

//                return helpText.ToString();
//            }
//        }

//        private static string GetOptionUsage(Type valueType)
//        {
//            if (valueType == typeof(bool))
//            {
//                return "[+|-]";
//            }
//            if (valueType == typeof(string))
//            {
//                return ":<text>";
//            }
//            if (valueType == typeof(int))
//            {
//                return ":<number>";
//            }
//            if (valueType.IsEnum)
//            {
//                return ":<" + string.Join("|", Enum.GetNames(valueType)).ToLower() + ">";
//            }
//            if (valueType.IsArray)
//            {
//                return GetOptionUsage(valueType.GetElementType()) + "[,...]";
//            }
//            return string.Empty;
//        }
//    }
//}