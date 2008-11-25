using System;

namespace NConsole
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CommandLineArgumentAttribute : Attribute
    {
        private readonly string _name;

        public CommandLineArgumentAttribute()
        {
        }

        public CommandLineArgumentAttribute(string name)
        {
            _name = name;

            if (string.IsNullOrEmpty(_name))
            {
                throw new CommandLineArgumentException("Command line arguments must have a name. Use the default " +
                    "constructor if the name should match the property name.");
            }
        }

        public string Name
        {
            get { return _name; }
        }

        //public bool ShortName { get; set; }

        public bool Required { get; set; }

        public bool Exclusive { get; set; }

        //public bool Description { get; set; }
    }
}