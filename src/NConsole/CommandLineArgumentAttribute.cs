using System;

namespace NConsole
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CommandLineArgumentAttribute : Attribute
    {
        private string _name;
        private readonly CommandLineArgumentTypes _type;

//        private string _shortName;
//        private string _description;

        public CommandLineArgumentAttribute()
        {
            _type = CommandLineArgumentTypes.AtMostOnce;
        }

        public CommandLineArgumentAttribute(string name)
            : this(name, CommandLineArgumentTypes.AtMostOnce)
        {
        }

        public CommandLineArgumentAttribute(CommandLineArgumentTypes type)
        {
            _type = type;
        }

        public CommandLineArgumentAttribute(string name, CommandLineArgumentTypes type)
        {
            _name = name;
            _type = type;

            if (string.IsNullOrEmpty(_name))
            {
                throw new CommandLineArgumentException("Command line arguments must have a name. Use the default constructor if the property name should be used.");
            }
        }

        public string Name
        {
            get { return _name; }
            //internal set { _name = value; }
        }

        //        public string ShortName
        //        {
        //            get { return _shortName; }
        //set { _shortName = value; }
        //        }

        public CommandLineArgumentTypes Type
        {
            get { return _type; }
        }

        //        public string Description
        //        {
        //            get { return _description; }
        //            set { _description = value; }
        //        }
    }
}