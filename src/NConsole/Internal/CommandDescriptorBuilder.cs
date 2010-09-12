using System;
using System.Reflection;

namespace NConsole.Internal
{
    /// <summary>
    /// Defines the contract for implementations that should collect from one or more sources the meta
    /// information that dictates the <see cref="ICommand"/> behavior and the arguments it exposes.
    /// </summary>
    internal interface ICommandDescriptorBuilder
    {
        /// <summary>
        /// Builds a <see cref="CommandDescriptor"/> and collects details from the runtime type information.
        /// </summary>
        /// <param name="commandType">The <see cref="Type"/> that implements this command.</param>
        /// <returns>A <see cref="CommandDescriptor"/> populated with the command's details.</returns>
        CommandDescriptor BuildDescriptor(Type commandType);
    }

    internal class CommandDescriptorBuilder : ICommandDescriptorBuilder
    {
        private const string CommandTypeSuffix = "Command";

        public CommandDescriptor BuildDescriptor(Type commandType)
        {
            // Validate the type before we begin processing it
            ValidateType(commandType);

            // Build a descriptor
            CommandDescriptor descriptor = new CommandDescriptor();
            CollectDetails(commandType, descriptor);
            CollectArguments(commandType, descriptor);

            // Validate command descriptor after it is built
            ValidateDescriptor(descriptor);

            return descriptor;
        }

        /// <summary>
        /// Validates the <see cref="Type"/> before we attempt to collect details from it to ensure it's valid.
        /// </summary>
        /// <param name="commandType">The <see cref="Type"/> to inspect.</param>
        private static void ValidateType(Type commandType)
        {
            // Ensure we have an ICommand
            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new Exception(string.Format("Command type '{0}' does not implement ICommand.", commandType.FullName));
            }
        }

        private void CollectDetails(Type commandType, CommandDescriptor descriptor)
        {
            // Store the CLR type
            descriptor.CommandType = commandType;

            // Set the default command name (remove the word command from the end if it exists)
            string className = commandType.Name;
            if (className.EndsWith(CommandTypeSuffix))
            {
                descriptor.Name = className.Substring(0, className.Length - CommandTypeSuffix.Length).ToLower();
            }
            else
            {
                descriptor.Name = className.ToLower();
            }
        }

        private void CollectArguments(Type commandType, CommandDescriptor descriptor)
        {
            // Build up a list of arguments that will later be used for parsing
            PropertyInfo[] properties = commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.IsDefined(typeof(ArgumentAttribute), true))
                {
                    ArgumentAttribute attribute = (ArgumentAttribute)Attribute.GetCustomAttribute(
                        propertyInfo, typeof(ArgumentAttribute), true);

                    ArgumentDescriptor argumentDescriptor = new ArgumentDescriptor();
                    argumentDescriptor.ArgumentType = propertyInfo.PropertyType;
                    argumentDescriptor.PropertyInfo = propertyInfo;

                    // Add the specified short name
                    if (!string.IsNullOrEmpty(attribute.ShortName))
                    {
                        argumentDescriptor.ShortNames.Add(attribute.ShortName);
                    }

                    // Add the specified long name
                    if (!string.IsNullOrEmpty(attribute.LongName))
                    {
                        argumentDescriptor.LongNames.Add(attribute.LongName);
                    }

                    // If this isn't a positional argument, and a short or long name has not been set in the attribute,
                    // then we'll automatically create a long name using the property name
                    if (attribute.Position == -1 &&
                        string.IsNullOrEmpty(attribute.ShortName) &&
                        string.IsNullOrEmpty(attribute.LongName))
                    {
                        argumentDescriptor.LongNames.Add(propertyInfo.Name.ToLower());
                    }

                    descriptor.Arguments.Add(argumentDescriptor);
                }
            }
        }

        private static void ValidateDescriptor(CommandDescriptor descriptor)
        {
            // Ensure the command has a name
            if (string.IsNullOrEmpty(descriptor.Name))
            {
                throw new Exception(string.Format("Command type '{0}' does not provide a command name.", descriptor.CommandType.FullName));
            }
        }
    }
}