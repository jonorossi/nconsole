using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineParserArgumentTypeTests
    {
        [Test]
        public void OccursAtMostOnce()
        {
            var parser = new CommandLineParser<Options_ArgTypes>();

            // Allowed once
            Assert.IsNotNull(parser.ParseArguments(new[] { "/atmostonce" }));

            // Throws if used multiple times
            AssertEx.Throws<CommandLineArgumentException>("Argument '/atmostonce' can only appear once.", () =>
                parser.ParseArguments(new[] { "/atmostonce" })
            );
        }

        [Test]
        public void OccursRequired()
        {
            var parser = new CommandLineParser<Options_ArgTypes>();

            // Throws if not provided
            AssertEx.Throws<CommandLineArgumentException>("Argument '/required' must appear at least once.", () =>
                parser.ParseArguments(new[] { "/required" })
            );

            // Allowed to appear once
            Assert.IsNotNull(parser.ParseArguments(new[] { "/required" }));

            // Allowed to appear twice
            Assert.IsNotNull(parser.ParseArguments(new[] { "/required", "/required" }));
        }

        private class Options_ArgTypes
        {
            [CommandLineArgument(CommandLineArgumentTypes.AtMostOnce)]
            public bool AtMostOnce { get; set; }
            [CommandLineArgument(CommandLineArgumentTypes.Required)]
            public bool Required { get; set; }
        }

        //TODO: test rest

//        AtMostOnce = 0,
//        Required = 1
//        Unique = 0x02,
//        Multiple = 0x04,
//        Exclusive = 0x08,
//        MultipleUnique = Multiple | Unique,
    }
}