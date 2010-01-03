using System;

namespace NConsole
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name)
        {
        }

        public string HelpMessage { get; set; }
    }
}