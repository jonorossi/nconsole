using System;

namespace NConsole.Tests.Utils
{
    [Serializable]
    public class ThrowsException : Exception
    {
        /// <summary>
        /// Used if no exception was thrown.
        /// </summary>
        /// <param name="expectedType">The expected exception type.</param>
        public ThrowsException(Type expectedType)
            : base(string.Format("Expected an exception of type '{0}' to be thrown, however no exception was thrown.",
                expectedType.FullName))
        {
        }

        /// <summary>
        /// Used if the actual exception type is not exactly the expected exception type.
        /// </summary>
        /// <param name="expectedType">The expected exception type.</param>
        /// <param name="actual">The actual exception that was thrown by the user code.</param>
        public ThrowsException(Type expectedType, Exception actual)
            : base(string.Format("Expected an exception of type '{0}' to be thrown, however a '{1}' was thrown.",
                expectedType.FullName, actual.GetType().FullName))
        {
        }

        /// <summary>
        /// Used if the actual exception message does not match the expected exception message.
        /// </summary>
        /// <param name="expectedType">The expected exception type.</param>
        /// <param name="actual">The actual exception that was thrown by the user code.</param>
        /// <param name="expectedMessage">The expected message.</param>
        public ThrowsException(Type expectedType, Exception actual, string expectedMessage)
            : base(string.Format("Expected the message of the '{1}' exception to be{0}'{2}', but it was{0}'{3}'.",
            Environment.NewLine, expectedType.FullName, expectedMessage, actual.Message))
        {
        }
    }
}