using System;

namespace NConsole.Internal
{
    /// <summary>
    /// Provides the details about a command.
    /// </summary>
    internal class CommandDescriptor
    {
        /// <summary>
        /// Gets the CLR type that implements this command.
        /// </summary>
        public Type CommandType { get; set; }

        /// <summary>
        /// Gets the name of the command that the user can use to invoke this command.
        /// </summary>
        public string Name { get; set; }
    }
}