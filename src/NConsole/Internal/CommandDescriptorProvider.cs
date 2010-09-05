using System;

namespace NConsole.Internal
{
    /// <summary>
    /// Defines the contract for implementations that should collect from one or more sources the meta
    /// information that dictates the <see cref="ICommand"/> behavior and the arguments it exposes.
    /// </summary>
    internal interface ICommandDescriptorProvider
    {
        /// <summary>
        /// Builds a <see cref="CommandDescriptor"/> and collects details from the runtime type information.
        /// </summary>
        /// <param name="commandType">The <see cref="Type"/> that implements this command.</param>
        /// <returns>A <see cref="CommandDescriptor"/> populated with the command's details.</returns>
        CommandDescriptor BuildDescriptor(Type commandType);
    }

    internal class CommandDescriptorProvider : ICommandDescriptorProvider
    {
        public CommandDescriptor BuildDescriptor(Type commandType)
        {
            // Validate the type before we begin processing it
            ValidateType(commandType);

            // Build a descriptor
            CommandDescriptor descriptor = new CommandDescriptor();
            CollectDetails(commandType, descriptor);

            // Validate command descriptor after it is built
            ValidateDescriptor(descriptor);

            return descriptor;
        }

        private void CollectDetails(Type commandType, CommandDescriptor descriptor)
        {
            // Store the CLR type
            descriptor.CommandType = commandType;

            // Set the default command name (remove the word command from the end if it exists)
            string className = commandType.Name;
            if (className.EndsWith("Command"))
            {
                descriptor.Name = className.Substring(0, className.Length - 7).ToLower();
            }
            else
            {
                descriptor.Name = className.ToLower();
            }
        }

        /// <summary>
        /// Validate the <see cref="Type"/> before we attempt to collect details from it to ensure it's valid.
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