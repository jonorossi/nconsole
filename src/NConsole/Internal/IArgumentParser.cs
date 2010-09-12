using System.Collections.Generic;

namespace NConsole.Internal
{
    internal interface IArgumentParser
    {
        void Apply(IList<string> args, ICommand command, CommandDescriptor commandDescriptor);
    }
}