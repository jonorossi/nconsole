using System.Diagnostics;
using NUnit.Framework;

namespace NConsole.Tests.Utils
{
    [DebuggerNonUserCode]
    internal class AssertEx
    {
        //TODO: Work out if I still want this
        public static void IsExactInstanceOf<T>(object actual)
        {
            if (actual.GetType() != typeof(T))
            {
                throw new AssertionException(string.Format("Expected type '{0}', but was '{1}'.",
                    typeof(T).FullName, actual.GetType().FullName));
            }
        }
    }
}