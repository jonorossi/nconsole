namespace NConsole
{
//    [Flags]
//    public enum CommandLineArgumentTypes
//    {
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

        // Requried for collections should mean at least one item
        // [CommandLineArgument("files", Required = true)]
        // [CommandLineArgument("files", Required = true, Exclusive = true)]
        // [CommandLineArgument("files", AllowDuplicates = false)]
        // [CommandLineArgument("files", UniqueValues = true)]

        //  /collarg:val1,val2,val3
        //  /collarg:val1 /colarg:val2 /colarg:val3



        // 0..1
//        AtMostOnce/* = 0*/,

        // Must be in the range 0..1
//        Required/* = 1*/,

//        Unique = 0x02,

        // 0..n
//        Multiple = 0x04,
//        Exclusive/* = 0x08*/,
//        MultipleUnique = Multiple | Unique,
//    }
}