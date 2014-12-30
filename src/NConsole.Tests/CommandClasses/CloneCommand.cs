namespace NConsole.Tests.CommandClasses
{
    public class CloneCommand : NoOpCommand
    {
        [Argument(Position = 0)]
        public string Repository { get; set; }
    }
}