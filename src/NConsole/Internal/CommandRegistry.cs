using System;
using System.Collections.Generic;

namespace NConsole.Internal
{
    internal interface ICommandRegistry
    {
        void Register(Type commandType);
        void SetDefaultCommand(Type commandType);
        CommandDescriptor GetDescriptor(string commandName);
    }

    internal class CommandRegistry : ICommandRegistry
    {
        private readonly string defaultCommandName = string.Empty;
        private readonly List<CommandDescriptor> descriptors = new List<CommandDescriptor>();
        private readonly ICommandDescriptorProvider provider = new CommandDescriptorProvider();

        public void Register(Type commandType)
        {
            CommandDescriptor descriptor = provider.BuildDescriptor(commandType);
            descriptors.Add(descriptor);
        }

        public void SetDefaultCommand(Type commandType)
        {
            foreach (CommandDescriptor descriptor in descriptors)
            {
                if (descriptor.CommandType == commandType)
                {
                    // Add an alias for an empty command name
                    descriptor.Aliases.Add(defaultCommandName);

                    return;
                }
            }
            throw new Exception(string.Format("Command {0} is not registered.", commandType.FullName));
        }

        public CommandDescriptor GetDescriptor(string commandName)
        {
            foreach (CommandDescriptor descriptor in descriptors)
            {
                if (descriptor.Name == commandName ||
                    descriptor.Aliases.Contains(commandName))
                {
                    return descriptor;
                }
            }
            return null;
        }
    }
}