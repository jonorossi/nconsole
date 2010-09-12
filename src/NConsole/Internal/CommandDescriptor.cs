using System;
using System.Collections.Generic;

namespace NConsole.Internal
{
    /// <summary>
    /// Provides the details about a command.
    /// </summary>
    internal class CommandDescriptor
    {
        public CommandDescriptor()
        {
            Aliases = new List<string>();
            Arguments = new List<ArgumentDescriptor>();
        }

        /// <summary>
        /// Gets or sets the CLR type that implements this command.
        /// </summary>
        public Type CommandType { get; set; }

        /// <summary>
        /// Gets or sets the name of the command that the user can use to invoke this command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the aliases of this command.
        /// </summary>
        public IList<string> Aliases { get; private set; }

        /// <summary>
        /// Gets or sets the arguments supported by this command.
        /// </summary>
        public IList<ArgumentDescriptor> Arguments { get; private set; }
    }
}