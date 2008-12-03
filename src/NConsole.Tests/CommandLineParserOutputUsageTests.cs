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
            Assert.AreEqual("  /help[+|-]                  Example description", lines[2]);
        }

        [Test]
        public void DescriptionLinesUpWithLongArgumentNames()
        {
            string[] lines = GetUsageLinesFor<Options_OneShortNameOneLonGName>();

            Assert.AreEqual(5, lines.Length);
            Assert.AreEqual("  /short:<text>               Short description", lines[2]);
            Assert.AreEqual("  /thisisalongargumentname:<text>  Long description", lines[3]);
        }

        [Test]
        public void DescriptionLinesUpWithShortArgumentNames()
        {
            string[] lines = GetUsageLinesFor<Options_OneShortNameWithDescription>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /db:<text>                  The database to connect to", lines[2]);
        }

        [Test]
        public void OutputsCorrectArgumentName()
        {
            string[] lines = GetUsageLinesFor<Options_SingleRenamedArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /db:<text>", lines[2]);
        }

        [Test]
        public void OutputsUsageForStringArray()
        {
            string[] lines = GetUsageLinesFor<Options_SeveralArrayArgs>();

            Assert.AreEqual(6, lines.Length);
            Assert.AreEqual("  /strings:<text>[,...]", lines[2]);
            Assert.AreEqual("  /integers:<number>[,...]", lines[3]);
            Assert.AreEqual("  /modes:<all|mode1|mode2>[,...]", lines[4]);
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
            string[] lines = GetUsageLinesFor<Options_SingleNullableNumericArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /timeout:<number>", lines[2]);
        }

        [Test]
        public void OutputsUsageForEnumeration()
        {
            string[] lines = GetUsageLinesFor<Options_SingleEnumArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /mode:<all|mode1|mode2>", lines[2]);
        }

        [Test]
        public void OutputsRequiredArgumentsInUsageHeader()
        {
            string[] lines = GetUsageLinesFor<Options_SingleEnumArg>();

            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("Usage: NConsole.Tests /mode:<all|mode1|mode2> [options]", lines[0]);
            Assert.AreEqual("Options:", lines[1]);
            Assert.AreEqual("  /mode:<all|mode1|mode2>", lines[2]);
        }

        [Test]
        public void OutputsTwoRequiredArgumentsInUsageHeader()
        {
            string[] lines = GetUsageLinesFor<Options_TwoRequiredArgs>();

            Assert.AreEqual(5, lines.Length);
            Assert.AreEqual("Usage: NConsole.Tests /arg1:<text> /arg2:<text> [options]", lines[0]);
        }

        private static string[] GetUsageLinesFor<TOptions>() where TOptions : class, new()
        {
            var parser = new CommandLineParser<TOptions>();
            return parser.Usage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        #region Option Classes

// ReSharper disable UnusedMemberInPrivateClass
        private enum Mode { All, Mode1, Mode2 }
// ReSharper restore UnusedMemberInPrivateClass

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

        private class Options_OneShortNameOneLonGName
        {
            [CommandLineArgument(Description = "Short description")]
            public string Short { get; set; }

            [CommandLineArgument(Description = "Long description")]
            public string ThisIsALongArgumentName { get; set; }
        }

        private class Options_OneShortNameWithDescription
        {
            [CommandLineArgument("db", Description = "The database to connect to")]
            public string Database { get; set; }
        }

        private class Options_SingleRenamedArg
        {
            [CommandLineArgument("db")]
            public string Database { get; set; }
        }

        private class Options_SeveralArrayArgs
        {
            [CommandLineArgument]
            public string[] Strings { get; set; }

            [CommandLineArgument]
            public int[] Integers { get; set; }

            [CommandLineArgument]
            public Mode[] Modes { get; set; }
        }

        private class Options_SingleNumericArg
        {
            [CommandLineArgument]
            public int Timeout { get; set; }
        }

        private class Options_SingleNullableNumericArg
        {
            [CommandLineArgument]
            public int? Timeout { get; set; }
        }

        private class Options_SingleEnumArg
        {
            [CommandLineArgument("mode", Required = true)]
            public Mode AppMode { get; set; }
        }

        private class Options_TwoRequiredArgs
        {
            [CommandLineArgument(Required = true)]
            public string Arg1 { get; set; }

            [CommandLineArgument(Required = true)]
            public string Arg2 { get; set; }
        }

        #endregion
    }
}