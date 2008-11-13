using System;

namespace NConsole
{
    [Serializable]
    public class CommandLineArgumentException : Exception
    {
        public CommandLineArgumentException(string message)
            : base(message)
        {
        }
    }
}