using System;

namespace NConsole
{
    [Flags]
    public enum CommandLineArgumentTypes
    {
        // === Standard/Non-Collection Arguments ===
        // 0..1     AtMostOnce                  // Default for standard arguments
        // 1        AtMostOnce | Required       // Only need to specify Required because it it automatically 0..1
        //          Exclusive

        // === Collection Arguments & Values ===
        // 0..n     Multiple                    // This just extends AtMostOnce to any number of times
        // 1..n     Multiple | Required         // 
        //          Exclusive

        //Occurs.AtMostOnce, Occurs.MultipleTimes

        // Defaults: Required=false, Exclusive=false

        // The required properties of the arguments should only be checked if the program isn't being run
        // with an exclusive argument

        // [CommandLineArgument("server", Required = true)]
        // [CommandLineArgument("help", Exclusive = true)]

        // Requried for collections should mean at least one
        // [CommandLineArgument("files", Required = true)]
        // [CommandLineArgument("files", Required = true, Exclusive = true)]

        //  /collarg:val1,val2,val3
        //  /collarg:val1 /colarg:val2 /colarg:val3



        // 0..1
        AtMostOnce/* = 0*/,

        // Must be in the range 0..1
        Required/* = 1*/,

//        Unique = 0x02,

        // 0..n
        Multiple = 0x04,
        Exclusive/* = 0x08*/,
//        MultipleUnique = Multiple | Unique,
    }

    /// <summary>
    /// Used to control parsing of command-line arguments.
    /// </summary>
    [Flags]
    public enum CommandLineArgumentTypes2
    {
        /// <summary>
        /// Indicates that this field is required. An error will be displayed
        /// if it is not present when parsing arguments.
        /// </summary>
        Required = 0x01,

        /// <summary>
        /// Only valid in conjunction with Multiple.
        /// Duplicate values will result in an error.
        /// </summary>
        Unique = 0x02,

        /// <summary>
        /// Inidicates that the argument may be specified more than once.
        /// Only valid if the argument is a collection
        /// </summary>
        Multiple = 0x04,

        /// <summary>
        /// Inidicates that if this argument is specified, no other arguments may be specified.
        /// </summary>
        Exclusive = 0x08,

        /// <summary>
        /// The default type for non-collection arguments.
        /// The argument is not required, but an error will be reported if it is specified more than once.
        /// </summary>
        AtMostOnce = 0x00

        /// <summary>
        /// The default type for collection arguments.
        /// The argument is permitted to occur multiple times, but duplicate 
        /// values will cause an error to be reported.
        /// </summary>
        MultipleUnique = Multiple | Unique,
    }
}