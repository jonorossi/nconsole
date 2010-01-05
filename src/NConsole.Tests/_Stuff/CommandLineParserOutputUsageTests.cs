using System;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture, Ignore]
    public class CommandLineParserOutputUsageTests
    {
        [Test]
        public void OutputsProgramUsageLine()
        {
            // Arrange

            // Act
            string[] lines = GetUsageLinesFor<Options_NoArgs>();

            // Assert
            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual("Usage: NConsole.Tests [options]", lines[0]);
            Assert.AreEqual("Options:", lines[1]);
            Assert.AreEqual("", lines[2]);
        }

        [Test]
        public void OutputsSingleArgumentName()
        {
            // Arrange

            // Act
            string[] lines = GetUsageLinesFor<Options_SingleArg>();

            // Assert
            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /help[+|-]", lines[2]);
        }

//        [Test]
//        public void OutputsDescription()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_SingleArgWithDescription>();
//
//            // Assert
//            Assert.AreEqual(4, lines.Length);
//            Assert.AreEqual("  /help[+|-]                  Example description", lines[2]);
//        }
//
//        [Test]
//        public void DescriptionLinesUpWithLongArgumentNames()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_OneShortNameOneLonGName>();
//
//            // Assert
//            Assert.AreEqual(5, lines.Length);
//            Assert.AreEqual("  /short:<text>               Short description", lines[2]);
//            Assert.AreEqual("  /thisisalongargumentname:<text>  Long description", lines[3]);
//        }
//
//        [Test]
//        public void DescriptionLinesUpWithShortArgumentNames()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_OneShortNameWithDescription>();
//
//            // Assert
//            Assert.AreEqual(4, lines.Length);
//            Assert.AreEqual("  /db:<text>                  The database to connect to", lines[2]);
//        }
//
//        [Test]
//        public void OutputsCorrectArgumentName()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_SingleRenamedArg>();
//
//            // Assert
//            Assert.AreEqual(4, lines.Length);
//            Assert.AreEqual("  /db:<text>", lines[2]);
//        }

        [Test]
        public void OutputsUsageForStringArray()
        {
            // Arrange

            // Act
            string[] lines = GetUsageLinesFor<Options_SeveralArrayArgs>();

            // Assert
            Assert.AreEqual(6, lines.Length);
            Assert.AreEqual("  /strings:<text>[,...]", lines[2]);
            Assert.AreEqual("  /integers:<number>[,...]", lines[3]);
            Assert.AreEqual("  /modes:<all|mode1|mode2>[,...]", lines[4]);
        }

        [Test]
        public void OutputsUsageForNumberArg()
        {
            // Arrange

            // Act
            string[] lines = GetUsageLinesFor<Options_SingleNumericArg>();

            // Assert
            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /timeout:<number>", lines[2]);
        }

        [Test]
        public void OutputsUsageForNullableNumberArg()
        {
            // Arrange

            // Act
            string[] lines = GetUsageLinesFor<Options_SingleNullableNumericArg>();

            // Assert
            Assert.AreEqual(4, lines.Length);
            Assert.AreEqual("  /timeout:<number>", lines[2]);
        }

//        [Test]
//        public void OutputsUsageForEnumeration()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_SingleEnumArg>();
//
//            // Assert
//            Assert.AreEqual(4, lines.Length);
//            Assert.AreEqual("  /mode:<all|mode1|mode2>", lines[2]);
//        }
//
//        [Test]
//        public void OutputsRequiredArgumentsInUsageHeader()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_SingleEnumArg>();
//
//            // Assert
//            Assert.AreEqual(4, lines.Length);
//            Assert.AreEqual("Usage: NConsole.Tests /mode:<all|mode1|mode2> [options]", lines[0]);
//            Assert.AreEqual("Options:", lines[1]);
//            Assert.AreEqual("  /mode:<all|mode1|mode2>", lines[2]);
//        }
//
//        [Test]
//        public void OutputsTwoRequiredArgumentsInUsageHeader()
//        {
//            // Arrange
//
//            // Act
//            string[] lines = GetUsageLinesFor<Options_TwoRequiredArgs>();
//
//            // Assert
//            Assert.AreEqual(5, lines.Length);
//            Assert.AreEqual("Usage: NConsole.Tests /arg1:<text> /arg2:<text> [options]", lines[0]);
//        }

        private static string[] GetUsageLinesFor<TOptions>() where TOptions : class, new()
        {
            var parser = new CommandLineParser<TOptions>();
            return parser.Usage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        #region Option Classes

        private enum Mode { All, Mode1, Mode2 }

        private class Options_NoArgs
        {
        }

        private class Options_SingleArg
        {
            [Argument]
            public bool Help { get; set; }
        }

//        private class Options_SingleArgWithDescription
//        {
//            [Argument(Description = "Example description")]
//            public bool Help { get; set; }
//        }
//
//        private class Options_OneShortNameOneLonGName
//        {
//            [Argument(Description = "Short description")]
//            public string Short { get; set; }
//
//            [Argument(Description = "Long description")]
//            public string ThisIsALongArgumentName { get; set; }
//        }
//
//        private class Options_OneShortNameWithDescription
//        {
//            [Argument("db", Description = "The database to connect to")]
//            public string Database { get; set; }
//        }
//
//        private class Options_SingleRenamedArg
//        {
//            [Argument("db")]
//            public string Database { get; set; }
//        }

        private class Options_SeveralArrayArgs
        {
            [Argument]
            public string[] Strings { get; set; }

            [Argument]
            public int[] Integers { get; set; }

            [Argument]
            public Mode[] Modes { get; set; }
        }

        private class Options_SingleNumericArg
        {
            [Argument]
            public int Timeout { get; set; }
        }

        private class Options_SingleNullableNumericArg
        {
            [Argument]
            public int? Timeout { get; set; }
        }

//        private class Options_SingleEnumArg
//        {
//            [Argument("mode", Mandatory = true)]
//            public Mode AppMode { get; set; }
//        }
//
//        private class Options_TwoRequiredArgs
//        {
//            [Argument(Mandatory = true)]
//            public string Arg1 { get; set; }
//
//            [Argument(Mandatory = true)]
//            public string Arg2 { get; set; }
//        }

        #endregion
    }
}