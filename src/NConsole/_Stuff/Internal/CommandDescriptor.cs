using System;

namespace NConsole.Internal
{
    /// <summary>
    /// Provides the details about a command.
    /// </summary>
    public class CommandDescriptor
    {
        /// <summary>
        /// Builds a <see cref="CommandDescriptor"/> and collects details from the runtime type information.
        /// </summary>
        /// <param name="commandType">The <see cref="Type"/> that implements this command.</param>
        /// <returns>A <see cref="CommandDescriptor"/> populated with the command's details.</returns>
        public static CommandDescriptor FromType(Type commandType)
        {
            // Validate the type before we begin processing it
            ValidateType(commandType);

            // Build the descriptor
            CommandDescriptor commandDescriptor = new CommandDescriptor(commandType);

            // Validate command descriptor after it is built
            ValidateDescriptor(commandDescriptor);

            return commandDescriptor;
        }

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

        private CommandDescriptor(Type commandType)
        {
            // Store the CLR type
            CommandType = commandType;

            // Set the default command name (remove the word command from the end if it exists)
            string className = commandType.Name;
            if (className.EndsWith("Command"))
            {
                Name = className.Substring(0, className.Length - 7).ToLower();
            }
            else
            {
                Name = className.ToLower();
            }
        }

        /// <summary>
        /// Gets the CLR type that implements this command.
        /// </summary>
        public Type CommandType { get; private set; }

        /// <summary>
        /// Gets the name of the command that the user can use to invoke this command.
        /// </summary>
        public string Name { get; private set; }
    }
}