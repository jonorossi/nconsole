using System;
using System.Collections.Generic;
using System.Linq;

namespace NConsole.Internal
{
    internal interface ICommandRegistry
    {
        void Register(Type commandType);
    }

    internal class CommandRegistry : ICommandRegistry
    {
        private readonly ICommandDescriptorProvider provider = new CommandDescriptorProvider();

        private readonly List<CommandDescriptor> descriptors = new List<CommandDescriptor>();

        public void Register(Type commandType)
        {
            CommandDescriptor descriptor = provider.BuildDescriptor(commandType);
            descriptors.Add(descriptor);
        }

        public CommandDescriptor GetDescriptor(string commandName)
        {
            return descriptors.FirstOrDefault(d => d.Name == commandName);
        }
    }
}