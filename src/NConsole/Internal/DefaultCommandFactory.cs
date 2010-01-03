using System;
using System.Collections.Generic;

namespace NConsole.Internal
{
    public class DefaultCommandFactory : ICommandFactory
    {
        private readonly IList<CommandDescriptor> commandDescriptors = new List<CommandDescriptor>();

        public void Register(Type commandType)
        {
            // Ensure we have an ICommand
            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new Exception(string.Format("Command type '{0}' does not implement ICommand.", commandType.FullName));
            }

            // Build a command descriptor
            CommandDescriptor descriptor = CommandDescriptor.FromType(commandType);
            commandDescriptors.Add(descriptor);
        }

        public ICommand Resolve(string commandName)
        {
            foreach (CommandDescriptor descriptor in commandDescriptors)
            {
                if (descriptor.Name == commandName)
                {
                    return Activator.CreateInstance(descriptor.CommandType) as ICommand;
                }
            }
            return null;
        }

        public IEnumerable<CommandDescriptor> CommandDescriptors
        {
            get { return commandDescriptors; }
        }
    }
}