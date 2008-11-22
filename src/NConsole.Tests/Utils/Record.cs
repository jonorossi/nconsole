using System;
using System.Diagnostics;

namespace NConsole.Tests.Utils
{
    [DebuggerNonUserCode]
    internal class Record
    {
        internal static Exception Exception(Action code)
        {
            try
            {
                code();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
    }
}