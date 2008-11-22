using System;
using System.Diagnostics;
using NUnit.Framework;

namespace NConsole.Tests.Utils
{
    [DebuggerNonUserCode]
    internal class AssertEx
    {
        public static void IsExactInstanceOf<T>(object actual)
        {
            if (actual.GetType() != typeof(T))
            {
                throw new AssertionException(string.Format("Expected type '{0}', but was '{1}'.",
                    typeof(T).FullName, actual.GetType().FullName));
            }
        }

        public static Exception Throws(Type expectedType, Action code)
        {
            Exception actual = Record.Exception(code);

            // If no exception was thrown then throw an exception
            if (actual == null)
            {
                throw new ThrowsException(expectedType);
            }

            // If the exception is not the exact type then thrown an exception
            if (!(actual.GetType() == expectedType))
            {
                throw new ThrowsException(expectedType, actual);
            }

            return actual;
        }

        public static Exception Throws(Type expectedType, string expectedMessage, Action code)
        {
            Exception actual = Throws(expectedType, code);

            // If the actual exception message does not match the expected exception message
            if (actual.Message != expectedMessage)
            {
                throw new ThrowsException(expectedType, actual, expectedMessage);
            }

            return actual;
        }

        public static T Throws<T>(Action code) where T : Exception
        {
            return (T)Throws(typeof(T), code);
        }

        public static T Throws<T>(string expectedMessage, Action code) where T : Exception
        {
            return (T)Throws(typeof(T), expectedMessage, code);
        }
    }
}