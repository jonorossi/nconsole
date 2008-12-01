using System;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineParserOutputUsageTests
    {
        [Test]
        public void OutputsProgramUsageLine()
        {
            string[] lines = GetUsageLinesFor<Options_NoArgs>();

            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual("Usage: NConsole.Tests [options]", lines[0]);
            Assert.AreEqual("Options:", lines[1]);
            Assert.AreEqual("", lines[2]);
        }

        [Test]
        public void OutputsSingleArgumentName()
        {
            string[] lines = GetUsageLinesFor<Options_SingleArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /help[+|-]", lines[2]);
        }

        [Test]
        public void OutputsDescription()
        {
            string[] lines = GetUsageLinesFor<Options_SingleArgWithDescription>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /help[+|-]\t\tExample description", lines[2]);
        }

        [Test]
        public void OutputsCorrectArgumentName()
        {
            string[] lines = GetUsageLinesFor<Options_SingleRenamedArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /db:<text>", lines[2]);
        }

        [Test]
        public void OutputsUsageForNumberArg()
        {
            string[] lines = GetUsageLinesFor<Options_SingleNumericArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /timeout:<number>", lines[2]);
        }

        [Test]
        public void OutputsUsageForNullableNumberArg()
        {
            

            Assert.Fail();
        }

        private static string[] GetUsageLinesFor<TOptions>() where TOptions : class, new()
        {
            var parser = new CommandLineParser<TOptions>();
            return parser.Usage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        #region Option Classes

        private class Options_NoArgs
        {
        }

        private class Options_SingleArg
        {
            [CommandLineArgument]
            public bool Help { get; set; }
        }

        private class Options_SingleArgWithDescription
        {
            [CommandLineArgument(Description = "Example description")]
            public bool Help { get; set; }
        }

        private class Options_SingleRenamedArg
        {
            [CommandLineArgument("db")]
            public string Database { get; set; }
        }

        private class Options_SingleNumericArg
        {
            [CommandLineArgument]
            public int Timeout { get; set; }
        }

        #endregion
    }
}