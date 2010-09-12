namespace NConsole.Tests.CommandClasses
{
    public class StringArgCommand : NoOpCommand
    {
        [Argument(LongName = "arg")]
        public string Argument { get; set; }

        [Argument(LongName = "array")]
        public string[] ArrayArgument { get; set; }
    }
}