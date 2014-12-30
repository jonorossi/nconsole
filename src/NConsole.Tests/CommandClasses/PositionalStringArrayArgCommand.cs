namespace NConsole.Tests.CommandClasses
{
    public class PositionalStringArrayArgCommand : NoOpCommand
    {
        [Argument(Position = 0)]
        public string[] ArrayArgument { get; set; }

        [Argument(LongName = "arg")]
        public bool Argument { get; set; }
    }

    public class PositionalStringArgCommand : NoOpCommand
    {
        [Argument(Position = 0)]
        public string Argument1 { get; set; }

        [Argument(Position = 1)]
        public string Argument2 { get; set; }

        [Argument(LongName = "arg")]
        public bool FlagArgument { get; set; }
    }
}