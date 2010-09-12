using System.Collections.Generic;
using System.Linq;

namespace NConsole.Internal
{
    internal class PositionalArgumentParser : IArgumentParser
    {
        public void Apply(IList<string> args, ICommand command, CommandDescriptor commandDescriptor)
        {
            var descriptors = commandDescriptor.Arguments.Where(a => a.Position >= 0).OrderBy(a => a.Position).ToList();

            while (args.Count > 0)
            {
                string arg = args[0];

                ArgumentDescriptor descriptor = descriptors.FirstOrDefault();
                if (descriptor == null)
                {
                    return;
                }

                descriptor.PropertyInfo.SetValue(command, arg, null);

                // Remove the first descriptor and the first argument because we have used them
                descriptors.RemoveAt(0);
                args.RemoveAt(0);
            }
        }
    }
}