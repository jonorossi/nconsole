using System;
using System.Collections.Generic;

namespace NConsole.Internal
{
    internal class ArgumentDescriptor
    {
        public ArgumentDescriptor()
        {
            ShortNames = new List<string>();
            LongNames = new List<string>();
        }

        /// <summary>
        /// Gets or sets the CLR type that of this argument.
        /// </summary>
        public Type ArgumentType { get; set; }

        /// <summary>
        /// Gets or sets the short names of this argument.
        /// </summary>
        public IList<string> ShortNames { get; private set; }

        /// <summary>
        /// Gets or sets the long names of this argument.
        /// </summary>
        public IList<string> LongNames { get; private set; }
    }
}