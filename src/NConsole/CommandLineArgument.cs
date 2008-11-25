using System;
using System.Reflection;

namespace NConsole
{
    /// <summary>
    /// Represents one of the possible arguments that can be passed to the application. It contains metadata collected
    /// by reflecting the property on an options class.
    /// </summary>
    public class CommandLineArgument
    {
//        private Type _elementType;
//        private CommandLineArgumentTypes _argumentType;
        private readonly CommandLineArgumentAttribute _attribute;
        private readonly PropertyInfo _propertyInfo;
        private readonly string _name;
        private object _argumentValue;
        private bool _seenValue;
//        private ArrayList _collectionValues;
//        private NameValueCollection _valuePairs;

        /// <summary>
        /// Creates an object that provides metadata for processing a command line argument.
        /// </summary>
        /// <param name="attribute">The <see cref="CommandLineArgumentAttribute"/> that is declared on the property in
        ///     the options class.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> of the argument represented in the options class.</param>
        public CommandLineArgument(CommandLineArgumentAttribute attribute, PropertyInfo propertyInfo)
        {
            _attribute = attribute;
            _propertyInfo = propertyInfo;

            // Use the name specifed on the attribute, if null use the property name
            _name = !string.IsNullOrEmpty(_attribute.Name) ? _attribute.Name : propertyInfo.Name.ToLower();
        }

        public string Name
        {
            get { return _name; }
        }

        public Type Type
        {
            get { return _propertyInfo.PropertyType; }
        }

        public bool IsExclusive
        {
            get { return _attribute.Exclusive; }
        }

        public bool AllowMultiple
        {
            get
            {
//                if (_attribute.Type == CommandLineArgumentTypes.Required)
//                {
//                    return true;
//                }

                return false;

                //return _attribute.Type & CommandLineArgumentTypes.Multiple;
            }
        }

        public bool IsRequired
        {
            get { return _attribute.Required; }
        }

        /// <summary>
        /// Sets the value of this argument.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        public void SetValue(object value)
        {
            // If this argument has already been parsed once and it doesn't allow multiple
            if (_seenValue && !AllowMultiple)
            {
                throw new CommandLineArgumentException(string.Format("Argument '/{0}' can only appear once.", Name));
            }

            _seenValue = true;

            _argumentValue = value;
        }

        public void Bind(object options)
        {
            // If this argument is required and no value has been specified
            if (IsRequired && !_seenValue)
            {
                throw new CommandLineArgumentException(string.Format("Argument '/{0}' must appear at least once.", Name));
            }

            _propertyInfo.SetValue(options, _argumentValue, null);
        }
    }
}