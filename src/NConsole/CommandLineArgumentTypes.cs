using System;

namespace NConsole
{
    [Flags]
    public enum CommandLineArgumentTypes
    {
        AtMostOnce = 0,
//        Required = 1
//        Unique = 0x02,
//        Multiple = 0x04,
        Exclusive = 0x08,
//        MultipleUnique = Multiple | Unique,
    }
}