using System;
using System.Collections.Generic;
using System.Linq;

namespace NConsole.Internal
{
    internal class NamedArgumentParser : IArgumentParser
    {
        private readonly char[] KeyValueSeparators = new[] { '=' }; // ':', '+', '-'

        public void Apply(IList<string> args, ICommand command, CommandDescriptor commandDescriptor)
        {
            string arg = args[0];

            // If the argument does not start a with the start of a named argument, we don't have any changes to make
            if (arg[0] != '-') return;

            // Determine the name of the argument
            bool? useLongName;
            int endIndex;
            string argName;
            if (arg.StartsWith("--"))
            {
                useLongName = true;
                endIndex = arg.IndexOfAny(KeyValueSeparators, 2);
                argName = arg.Substring(2, endIndex == -1 ? arg.Length - 2 : endIndex - 2);
            }
            else if (arg.StartsWith("-"))
            {
                useLongName = false;
                endIndex = -1;
                argName = arg.Substring(1, endIndex == -1 ? arg.Length - 1 : endIndex - 1);
            }
            else
            {
                throw new Exception("Unsupported named argument type.");
            }

            // Attempt to find a descriptor for this argument
            ArgumentDescriptor[] descriptors;
            if (useLongName.Value)
            {
                descriptors = commandDescriptor.Arguments.Where(a => a.LongNames.Contains(argName)).ToArray();
            }
            else if (!useLongName.Value)
            {
                descriptors = commandDescriptor.Arguments.Where(a => a.ShortNames.Contains(argName)).ToArray();
            }
            else
            {
                throw new Exception("Unsupported named argument type.");
            }

            if (descriptors.Length == 0) throw new Exception(string.Format("Unknown argument '{0}'.", arg));
            if (descriptors.Length > 1) throw new Exception(string.Format("Ambiguous argument '{0}', found {1} descriptors.", arg, descriptors.Length));

            var descriptor = descriptors[0];

            // Get current value of this argument 
            object currentValue = descriptor.PropertyInfo.GetValue(command, null);

            // Determine the value of the argument
            object argValue;
            if (endIndex == -1)
            {
                // No value was specified, so it is most likely a flag
                argValue = ValueConverter.ParseValue(descriptor.ArgumentType, null, currentValue);
            }
            //else if (arg[endIndex] == '+' || arg[endIndex] == '-')
            //{
            //    // Parse value with the '+' or '-'
            //    argValue = ValueConverter.ParseValue(argument.ValueType, arg.Substring(endIndex));
            //}
            else
            {
                // Parse value without the ':'
                argValue = ValueConverter.ParseValue(descriptor.ArgumentType, arg.Substring(endIndex + 1), currentValue);
            }

            // Set the value of the option
            descriptor.PropertyInfo.SetValue(command, argValue, null);

            // Remove the first argument because we have processed it
            args.RemoveAt(0);
        }
    }
}