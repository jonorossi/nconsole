using System;

namespace NConsole
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SubCommandAttribute : Attribute
    {
        public SubCommandAttribute(Type commandType)
        {
            CommandType = commandType;
        }

        public Type CommandType { get; private set; }
    }
}