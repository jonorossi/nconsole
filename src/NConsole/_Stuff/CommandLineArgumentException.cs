using System;
using System.Runtime.Serialization;

namespace NConsole
{
    [Serializable]
    public class CommandLineArgumentException : Exception
    {
        public CommandLineArgumentException()
        {
        }

        public CommandLineArgumentException(string message)
            : base(message)
        {
        }

        public CommandLineArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CommandLineArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}