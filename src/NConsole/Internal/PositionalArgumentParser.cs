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
                    // We are out of positional descriptors
                    return;
                }

                // Get current value of this argument
                object currentValue = descriptor.PropertyInfo.GetValue(command, null);

                // Determine the value of the argument
                object argValue = ValueConverter.ParseValue(descriptor.ArgumentType, arg, currentValue);

                // Set the value of the argument
                descriptor.PropertyInfo.SetValue(command, argValue, null);

                // Remove the argument because we have processed it
                args.RemoveAt(0);

                // Remove the first descriptor if it isn't an array because we have used it.
                // Keep array descriptors so we can keep using it
                if (!descriptor.ArgumentType.IsArray)
                {
                    descriptors.RemoveAt(0);
                }
            }
        }
    }
}