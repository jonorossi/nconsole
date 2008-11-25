using System;
using System.CodeDom.Compiler;
using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineParserTests
    {
        [Test]
        public void CanParseAnEmptyCommandLine()
        {
            CommandLineParser<Options_SingleBoolArg> parser = new CommandLineParser<Options_SingleBoolArg>();
            Options_SingleBoolArg options = parser.ParseArguments(new string[0]);

            Assert.IsNotNull(options);
            Assert.IsFalse(options.Help);
        }

        [Test]
        public void CanParseNullArguments()
        {
            CommandLineParser<Options_SingleBoolArg> parser = new CommandLineParser<Options_SingleBoolArg>();
            ArgumentNullException anex = AssertEx.Throws<ArgumentNullException>(() =>
                parser.ParseArguments(null)
            );

            Assert.AreEqual("args", anex.ParamName);
            Assert.AreEqual("Command line arguments are null.\r\nParameter name: args", anex.Message);
        }

        [Test]
        public void CanParseOneEmptyArgument()
        {
            CommandLineParser<Options_SingleBoolArg> parser = new CommandLineParser<Options_SingleBoolArg>();
            Options_SingleBoolArg options = parser.ParseArguments(new[] { "" });

            Assert.IsFalse(options.Help);
        }

        [Test]
        public void CanParseSingleBoolArgument()
        {
            var parser = new CommandLineParser<Options_SingleBoolArg>();
            var options = parser.ParseArguments(new[] { "/help" });

            Assert.IsTrue(options.Help);
        }

        [Test]
        public void CanParseRenamedArgument()
        {
            var parser = new CommandLineParser<Options_RenamedArg>();
            var options = parser.ParseArguments(new[] { "/help" });

            Assert.IsTrue(options.ShowHelp);
        }

        [Test]
        public void CanParseArgumentThatWasRenamedWithAttribute()
        {
            var parser = new CommandLineParser<Options_RenamedArg>();

            AssertEx.Throws<CommandLineArgumentException>("Unknown argument '/showhelp'.", () =>
                parser.ParseArguments(new[] { "/showhelp" })
            );
        }

        [Test]
        public void ThrowsOnUnknownArgument()
        {
            var parser = new CommandLineParser<Options_SingleBoolArg>();

            AssertEx.Throws<CommandLineArgumentException>("Unknown argument '/nonExistantArg'.", () =>
                parser.ParseArguments(new[] { "/nonExistantArg" })
            );
        }

        [Test]
        public void ParseTwoDifferentArguments()
        {
            var parser = new CommandLineParser<Options_TwoBoolArgs>();
            var options = parser.ParseArguments(new[] { "/argument1", "/argument2" });

            Assert.IsTrue(options.Argument1);
            Assert.IsTrue(options.Argument2);
        }

        [Test]
        public void HandlesPropertiesThatAreAttributedWithNonArgumentAttributes()
        {
            var parser = new CommandLineParser<Options_HasPropertyWithGeneratedCodeAttribute>();
            var options = parser.ParseArguments(new string[0]);

            Assert.IsNotNull(options);
        }

        [Test]
        public void ThrowsWhenExclusiveArgumentUsedWithAnotherArgument()
        {
            var parser = new CommandLineParser<Options_HasExclusiveHelp>();
            AssertEx.Throws<CommandLineArgumentException>(
                "The '/help' argument is exclusive and cannot be used with any other argument.",
                () => parser.ParseArguments(new[] { "/help", "/run" })
            );
        }

        [Test]
        public void ThrowsWhenExclusiveArgumentUsedWithAnotherArgumentWhenSpecifiedLast()
        {
            var parser = new CommandLineParser<Options_HasExclusiveHelp>();
            AssertEx.Throws<CommandLineArgumentException>(
                "The '/help' argument is exclusive and cannot be used with any other argument.",
                () => parser.ParseArguments(new[] { "/run", "/help" })
            );
        }

        [Test]
        public void ParserDoesNotSetValueOnNullableArgumentIfNotSpecified()
        {
            var parser = new CommandLineParser<Options_WithNullableInt>();
            var options = parser.ParseArguments(new string[0]);

            Assert.IsFalse(options.Timeout.HasValue);
        }

        [Test]
        public void ParserSetsValueOfNullerableInt()
        {
            var parser = new CommandLineParser<Options_WithNullableInt>();
            var options = parser.ParseArguments(new[] { "/timeout:100" });

            Assert.IsTrue(options.Timeout.HasValue);
            Assert.AreEqual(100, options.Timeout.Value);
        }

        #region Option Classes

        private class Options_SingleBoolArg
        {
            [CommandLineArgument]
            public bool Help { get; set; }
        }

        private class Options_RenamedArg
        {
            [CommandLineArgument("help")]
            public bool ShowHelp { get; set; }
        }

        private class Options_TwoBoolArgs
        {
            [CommandLineArgument]
            public bool Argument1 { get; set; }

            [CommandLineArgument]
            public bool Argument2 { get; set; }
        }

        private class Options_HasPropertyWithGeneratedCodeAttribute
        {
            [GeneratedCode("", "")]
            public string Test { get; set; }
        }

        private class Options_HasExclusiveHelp
        {
            [CommandLineArgument("help", Exclusive = true)]
            public bool Help { get; set; }

            [CommandLineArgument("run")]
            public bool Run { get; set; }
        }

        public class Options_WithNullableInt
        {
            [CommandLineArgument]
            public int? Timeout { get; set; }
        }

        #endregion
    }
}