using System;

namespace NConsole
{
    public static class ConsoleApplication
    {
        public static int Run(string[] args)
        {
            return Run(args, ArgumentMode.Undefined);
        }

        public static int Run(string[] args, ArgumentMode mode)
        {
            throw new NotImplementedException();
        }

        public static int Run(string[] args, ArgumentMode mode, Type onlyCommand)
        {
            throw new NotImplementedException();
        }
    }
}