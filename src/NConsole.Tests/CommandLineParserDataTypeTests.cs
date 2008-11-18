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
        public void BooleanArguments()
        {
            var parser = new CommandLineParser<Options_WithBooleanArguments>();
            var options = parser.ParseArguments(new[] { "/help", "/debug+", "/release-" });

            Assert.IsTrue(options.Help);
            Assert.IsTrue(options.Debug);
            Assert.IsFalse(options.Release);
        }

        private class Options_WithBooleanArguments
        {
            [CommandLineArgument]
            public bool Help { get; set; }
            [CommandLineArgument]
            public bool Debug { get; set; }
            [CommandLineArgument]
            public bool Release { get; set; }
        }

        [Test]
        public void NumericArguments()
        {
            var parser = new CommandLineParser<Options_WithNumericArguments>();
            var options = parser.ParseArguments(new[] { "/intarg:123456", "/doublearg:123.456" });

            Assert.AreEqual(123456, options.IntArg);
            Assert.AreEqual(123.456, options.DoubleArg);
        }

        private class Options_WithNumericArguments
        {
            [CommandLineArgument]
            public int IntArg { get; set; }
            [CommandLineArgument]
            public double DoubleArg { get; set; }
        }
    }
}