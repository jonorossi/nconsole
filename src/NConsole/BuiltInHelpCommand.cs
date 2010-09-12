namespace NConsole
{
    public class BuiltInHelpCommand : ICommand
    {
        public void Execute()
        {
            //TODO
        }
    }
}

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