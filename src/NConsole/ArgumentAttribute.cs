using System;

namespace NConsole
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ArgumentAttribute : Attribute
    {
        private readonly string _name;

        public ArgumentAttribute()
        {
        }

        public ArgumentAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new CommandLineArgumentException("Command line arguments must have a name. Use the default " +
                    "constructor if the name should match the property name.");
            }

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        //public bool ShortName { get; set; }

        /// <summary>
        /// Gets or sets whether this argument must be specified when using this command.
        /// </summary>
        public bool Mandatory { get; set; }

        public bool Exclusive { get; set; }

        public string Description { get; set; }
    }
}

//        public ArgumentAttribute()
//        {
//            Position = -1;
//        }
//
//        public int Position { get; set; }
//        public string ShortName { get; set; }
//        public bool Mandatory { get; set; }
//        public string HelpMessage { get; set; }