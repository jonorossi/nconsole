using System;
using System.Reflection;

//TODO: Probably need to create an ArgumentDescriptor which has information collected from various bits

namespace NConsole
{
    /// <summary>
    /// Represents one of the possible arguments that can be passed to the application. It contains metadata collected
    /// by reflecting the property on an options class.
    /// </summary>
    public class Argument
    {
//        private Type _elementType;
        private readonly ArgumentAttribute _attribute;
        private readonly PropertyInfo _propertyInfo;
        private readonly string _name;
        private object _argumentValue;
        private bool _seenValue;
//        private ArrayList _collectionValues;
//        private NameValueCollection _valuePairs;

        /// <summary>
        /// Creates an object that provides metadata for processing a command line argument.
        /// </summary>
        /// <param name="attribute">The <see cref="ArgumentAttribute"/> that is declared on the property in
        ///     the options class.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> of the argument represented in the options class.</param>
        public Argument(ArgumentAttribute attribute, PropertyInfo propertyInfo)
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

        public Type ValueType
        {
            get { return _propertyInfo.PropertyType; }
        }

        public bool IsRequired
        {
            get { return _attribute.Mandatory; }
        }

        public bool IsExclusive
        {
            get { return _attribute.Exclusive; }
        }

        public string Description
        {
            get { return _attribute.Description; }
        }

        public bool AllowMultiple
        {
            get
            {
//                if (_attribute.ValueType == CommandLineArgumentTypes.Required)
//                {
//                    return true;
//                }

                return false;

                //return _attribute.ValueType & CommandLineArgumentTypes.Multiple;
            }
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
            // If this argument hasn't been specified and it is required
            if (!_seenValue && IsRequired)
            {
                throw new CommandLineArgumentException(string.Format("Argument '/{0}' must appear at least once.", Name));
            }

            _propertyInfo.SetValue(options, _argumentValue, null);
        }
    }
}