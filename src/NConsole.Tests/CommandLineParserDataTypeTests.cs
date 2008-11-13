using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineParserDataTypeTests
    {
        [Test]
        public void StringArguments()
        {
            var parser = new CommandLineParser<Options_WithStringArguments>();
            var options = parser.ParseArguments(new[] { "/server:box", "/db:dev" });

            Assert.AreEqual("box", options.Server);
            Assert.AreEqual("dev", options.Database);
        }

        private class Options_WithStringArguments
        {
            [CommandLineArgument]
            public string Server { get; set; }
            [CommandLineArgument("db")]
            public string Database { get; set; }
        }

        [Test]
        public void IntegerArguments()
        {
            var parser = new CommandLineParser<Options_WithIntegerArguments>();
            var options = parser.ParseArguments(new[] { "/shortArg:123", "/intArg:123456", "/longArg:1234567890" });

            Assert.AreEqual(123, options.ShortArg);
            Assert.AreEqual(123456, options.IntArg);
            Assert.AreEqual(1234567890, options.LongArg);
        }

        private class Options_WithIntegerArguments
        {
            public short ShortArg { get; set; }
            public int IntArg { get; set; }
            public long LongArg { get; set; }
        }
    }
}