using System;
using System.Collections.Generic;
using System.Linq;

namespace NConsole.Internal
{
    internal class NamedArgumentParser : IArgumentParser
    {
        private readonly char[] KeyValueSeparators = new[] { '=', ':' };
        private readonly char[] ValueSuffixes = new[] { '+', '-' };

        private enum Prefix { Dash, DoubleDash, ForwardSlash }

        public void Apply(IList<string> args, ICommand command, CommandDescriptor commandDescriptor)
        {
            for (int i = 0; i < args.Count; i++)
            {
                if (ApplyArgument(args[i], command, commandDescriptor))
                {
                    // Remove the argument because we have processed it
                    args.RemoveAt(i);
                    i--;
                }
            }
        }

        private bool ApplyArgument(string arg, ICommand command, CommandDescriptor commandDescriptor)
        {
            // Determine the name of the argument
            Prefix prefix;
            int endIndex;
            string argName;
            if (arg.StartsWith("--"))
            {
                prefix = Prefix.DoubleDash;
                endIndex = arg.IndexOfAny(KeyValueSeparators, 2);
                argName = arg.Substring(2, endIndex == -1 ? arg.Length - 2 : endIndex - 2);
            }
            else if (arg.StartsWith("-"))
            {
                prefix = Prefix.Dash;
                endIndex = arg.IndexOfAny(KeyValueSeparators, 1);
                argName = arg.Substring(1, endIndex == -1 ? arg.Length - 1 : endIndex - 1);
            }
            else if (arg.StartsWith("/"))
            {
                prefix = Prefix.ForwardSlash;
                endIndex = arg.IndexOfAny(KeyValueSeparators, 1);
                argName = arg.Substring(1, endIndex == -1 ? arg.Length - 1 : endIndex - 1);
            }
            else
            {
                // If the argument does not start a with the start of a named argument, we don't have any changes to make
                return false;
            }

            // Remove the value suffixes from the argument name (these cannot be separators because args can have a dash in their name)
            argName = argName.TrimEnd(ValueSuffixes);

            // Attempt to find a descriptor for this argument
            List<ArgumentDescriptor> descriptors = new List<ArgumentDescriptor>();
            if (prefix == Prefix.DoubleDash || prefix == Prefix.ForwardSlash)
            {
                descriptors.AddRange(commandDescriptor.Arguments.Where(a => a.LongNames.Contains(argName)));
            }
            if (prefix == Prefix.Dash || prefix == Prefix.ForwardSlash)
            {
                descriptors.AddRange(commandDescriptor.Arguments.Where(a => a.ShortNames.Contains(argName)));
            }

            if (descriptors.Count == 0) throw new Exception(string.Format("Unknown argument '{0}'.", arg));
            if (descriptors.Count > 1) throw new Exception(string.Format("Ambiguous argument '{0}', found {1} descriptors.", arg, descriptors.Count));

            var descriptor = descriptors[0];

            // Get current value of this argument
            object currentValue = descriptor.PropertyInfo.GetValue(command, null);

            // Determine the value of the argument
            object argValue;
            if (ValueSuffixes.Contains(arg[arg.Length - 1]))
            {
                // Parse value with the '+' or '-'
                argValue = ValueConverter.ParseValue(descriptor.ArgumentType, arg[arg.Length - 1].ToString(), currentValue);
            }
            else if (endIndex == -1)
            {
                // No value was specified, so it is most likely a flag
                argValue = ValueConverter.ParseValue(descriptor.ArgumentType, null, currentValue);
            }
            else
            {
                // Parse value without the ':'
                argValue = ValueConverter.ParseValue(descriptor.ArgumentType, arg.Substring(endIndex + 1), currentValue);
            }

            // Set the value of the argument
            descriptor.PropertyInfo.SetValue(command, argValue, null);

            // We have processed this argument, so indicate this
            return true;
        }
    }
}