using System;
using System.Collections;

namespace NConsole.Internal
{
    internal static class ValueConverter
    {
        public static object ParseValue(Type type, string value, object currentValue)
        {
            if (type == typeof(bool))
            {
                if (string.IsNullOrEmpty(value) || value == "+")
                {
                    return true;
                }
                if (value == "-")
                {
                    return false;
                }
                throw new Exception("Boolean arguments can only be followed by '', '+' or '-'.");
            }
            if (type == typeof(string))
            {
                // Remove starting and trailing double quotes if they are there
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    return value.Substring(1, value.Length - 2);
                }
                return value;
            }
            if (type == typeof(int))
            {
                return int.Parse(value);
            }
            if (type == typeof(double))
            {
                return double.Parse(value);
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }
            if (type.IsArray)
            {
                // Determine the type of the array's elements
                Type elementType;
                if (type.GetElementType().IsEnum)
                {
                    elementType = type.GetElementType();
                }
                else
                {
                    elementType = typeof(string);
                }

                // Parse each item and return them
                ArrayList values = new ArrayList();
                if (currentValue != null)
                {
                    // If we already have a collection add all the items in it,
                    // otherwise add the single item if it is the item type
                    if (currentValue.GetType() == type)
                    {
                        values.AddRange((ICollection)currentValue);
                    }
                    else if (currentValue.GetType() == elementType)
                    {
                        values.Add(currentValue);
                    }
                }
                foreach (string item in value.Split(','))
                {
                    values.Add(ParseValue(elementType, item, null));
                }

                return values.ToArray(type.GetElementType());
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type underlyingType = type.GetGenericArguments()[0];
                return ParseValue(underlyingType, value, null);
            }
            throw new Exception(string.Format("Unsupported argument type '{0}' or value '{1}'.", type.FullName, value));
        }
    }
}