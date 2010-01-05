using System;

namespace NConsole
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ArgumentAttribute : Attribute
    {
        public string ShortName { get; set; }

        public string LongName { get; set; }

        /// <summary>
        /// Gets or sets whether this argument must be specified when using this command.
        /// </summary>
        //public bool Mandatory { get; set; }

        //public bool Exclusive { get; set; }

        //public string Description { get; set; }
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