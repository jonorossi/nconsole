using System;

namespace NConsole.Tests.Utils
{
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