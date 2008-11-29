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
        public void ThrowsWhenExclusiveArgumentSpecifiedLastAndUsedWithAnotherArgument()
        {
            var parser = new CommandLineParser<Options_HasExclusiveHelp>();

            AssertEx.Throws<CommandLineArgumentException>(
                "The '/help' argument is exclusive and cannot be used with any other argument.",
                () => parser.ParseArguments(new[] { "/run", "/help" })
            );
        }

        [Test]
        public void IgnoresRequiredArgumentsWhenExclusiveArgumentSpecified()
        {
            var parser = new CommandLineParser<Options_HasExclusiveHelpAndRequiredMode>();
            var options = parser.ParseArguments(new[] { "/help" });

            Assert.IsTrue(options.Help);
            Assert.IsNull(options.Run);
        }

        [Test]
        public void ParsesDoubleQuotedStringValues()
        {
            var parser = new CommandLineParser<Options_WithStringArg>();
            var options = parser.ParseArguments(new[] { "/arg:\"First Second Third\"" });

            Assert.AreEqual("First Second Third", options.Argument);
        }

        [Test]
        public void ParsesDoubleQuotedStringValuesInArray()
        {
            var parser = new CommandLineParser<Options_WithStringArg>();
            var options = parser.ParseArguments(new[] { "/array:\"First Second Third\",NextValue" });

            Assert.AreEqual(2, options.ArrayArgument.Length);
            Assert.AreEqual("First Second Third", options.ArrayArgument[0]);
            Assert.AreEqual("NextValue", options.ArrayArgument[1]);
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

        private class Options_HasExclusiveHelpAndRequiredMode
        {
            [CommandLineArgument("help", Exclusive = true)]
            public bool Help { get; set; }

            [CommandLineArgument("mode", Required = true)]
            public string Run { get; set; }
        }

        private class Options_WithStringArg
        {
            [CommandLineArgument("arg")]
            public string Argument { get; set; }

            [CommandLineArgument("array")]
            public string[] ArrayArgument { get; set; }
        }

        #endregion
    }
}