namespace NConsole.Tests.CommandClasses
{
    public class LsAppCommand : NoOpCommand
    {
        [Argument(ShortName = "l")]
        public bool LongListingFormat { get; set; }

        [Argument(ShortName = "h", LongName = "human-readable")]
        public bool HumanReadableSizes { get; set; }

        [Argument(ShortName = "S")]
        public bool SortByFileSize { get; set; }

        [Argument(ShortName = "I", LongName = "ignore")]
        public string[] IgnorePatterns { get; set; }
    }
}