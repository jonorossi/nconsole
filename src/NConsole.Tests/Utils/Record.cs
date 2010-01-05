using System;
using NUnit.Framework;

namespace NConsole.Tests.Utils
{
    /// <summary>
    /// Records thrown exceptions thrown from invoked delegates to support AAA-style unit testing.
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCode]
    public static class Record
    {
        /// <summary>
        /// Invokes a delegate and captures an exception if one is thrown.
        /// </summary>
        /// <typeparam name="T">The exception type to catch and return.</typeparam>
        /// <param name="code">The delegate to invoke.</param>
        /// <returns>The thrown exception, or null.</returns>
        public static T Exception<T>(Action code) where T : Exception
        {
            try
            {
                code();
            }
            catch (T ex)
            {
                return ex;
            }
            Assert.Fail("Delegate did not throw an " + typeof(T).Name + ".");
            return null;
        }

        /// <summary>
        /// Invokes a delegate and captures an exception if one is thrown.
        /// </summary>
        /// <param name="code">The delegate to invoke.</param>
        /// <returns>The thrown exception, or null.</returns>
        public static Exception Exception(Action code)
        {
            return Exception<Exception>(code);
        }
    }
}