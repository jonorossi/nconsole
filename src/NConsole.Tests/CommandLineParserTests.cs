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
            // Arrange
            var parser = new CommandLineParser<Options_SingleBoolArg>();

            // Act
            Options_SingleBoolArg options = parser.ParseArguments(new string[0]);

            // Assert
            Assert.IsFalse(options.Help);
        }

        [Test]
        public void CanParseNullArguments()
        {
            // Arrange
            var parser = new CommandLineParser<Options_SingleBoolArg>();

            // Act
            var ex = Record.Exception<ArgumentNullException>(() => parser.ParseArguments(null));

            // Assert
            Assert.IsInstanceOf(typeof(ArgumentNullException), ex);
            Assert.AreEqual("args", ex.ParamName);
            Assert.AreEqual("Command line arguments are null." + Environment.NewLine + "Parameter name: args", ex.Message);
        }

        [Test]
        public void CanParseOneEmptyArgument()
        {
            // Arrange
            var parser = new CommandLineParser<Options_SingleBoolArg>();

            // Act
            Options_SingleBoolArg options = parser.ParseArguments(new[] { "" });

            // Assert
            Assert.IsFalse(options.Help);
        }

        [Test]
        public void CanParseSingleBoolArgument()
        {
            // Arrange
            var parser = new CommandLineParser<Options_SingleBoolArg>();

            // Act
            var options = parser.ParseArguments(new[] { "/help" });

            // Assert
            Assert.IsTrue(options.Help);
        }

        [Test]
        public void CanParseRenamedArgument()
        {
            // Arrange
            var parser = new CommandLineParser<Options_RenamedArg>();

            // Act
            var options = parser.ParseArguments(new[] { "/help" });

            // Assert
            Assert.IsTrue(options.ShowHelp);
        }

        [Test]
        public void CanParseArgumentThatWasRenamedWithAttribute()
        {
            // Arrange
            var parser = new CommandLineParser<Options_RenamedArg>();

            // Act
            Exception ex = Record.Exception(() => parser.ParseArguments(new[] { "/showhelp" }));

            // Assert
            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
            Assert.AreEqual("Unknown argument '/showhelp'.", ex.Message);
        }

        [Test]
        public void ThrowsOnUnknownArgument()
        {
            // Arrange
            var parser = new CommandLineParser<Options_SingleBoolArg>();

            // Act
            Exception ex = Record.Exception(() => parser.ParseArguments(new[] { "/nonExistantArg" }));

            // Asseert
            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
            Assert.AreEqual("Unknown argument '/nonExistantArg'.", ex.Message);
        }

        [Test]
        public void ParseTwoDifferentArguments()
        {
            // Arrange
            var parser = new CommandLineParser<Options_TwoBoolArgs>();

            // Act
            var options = parser.ParseArguments(new[] { "/argument1", "/argument2" });

            // Assert
            Assert.IsTrue(options.Argument1);
            Assert.IsTrue(options.Argument2);
        }

        [Test]
        public void HandlesPropertiesThatAreAttributedWithNonArgumentAttributes()
        {
            // Arrange
            var parser = new CommandLineParser<Options_HasPropertyWithGeneratedCodeAttribute>();

            // Act
            var options = parser.ParseArguments(new string[0]);

            // Assert
            Assert.IsNotNull(options);
        }

        [Test]
        public void ThrowsWhenExclusiveArgumentUsedWithAnotherArgument()
        {
            // Arrange
            var parser = new CommandLineParser<Options_HasExclusiveHelp>();

            // Act
            Exception ex = Record.Exception(() => parser.ParseArguments(new[] { "/help", "/run" }));

            // Assert
            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
            Assert.AreEqual("The '/help' argument is exclusive and cannot be used with any other argument.", ex.Message);
        }

        [Test]
        public void ThrowsWhenExclusiveArgumentSpecifiedLastAndUsedWithAnotherArgument()
        {
            // Arrange
            var parser = new CommandLineParser<Options_HasExclusiveHelp>();

            // Act
            Exception ex = Record.Exception(() => parser.ParseArguments(new[] { "/run", "/help" }));

            // Assert
            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
            Assert.AreEqual("The '/help' argument is exclusive and cannot be used with any other argument.", ex.Message);
        }

        [Test]
        public void IgnoresRequiredArgumentsWhenExclusiveArgumentSpecified()
        {
            // Arrange
            var parser = new CommandLineParser<Options_HasExclusiveHelpAndRequiredMode>();

            // Act
            var options = parser.ParseArguments(new[] { "/help" });

            // Assert
            Assert.IsTrue(options.Help);
            Assert.IsNull(options.Run);
        }

        [Test]
        public void ParsesDoubleQuotedStringValues()
        {
            // Arrange
            var parser = new CommandLineParser<Options_WithStringArg>();

            // Act
            var options = parser.ParseArguments(new[] { "/arg:\"First Second Third\"" });

            // Assert
            Assert.AreEqual("First Second Third", options.Argument);
        }

        [Test]
        public void ParsesDoubleQuotedStringValuesInArray()
        {
            // Arrange
            var parser = new CommandLineParser<Options_WithStringArg>();

            // Act
            var options = parser.ParseArguments(new[] { "/array:\"First Second Third\",NextValue" });

            // Assert
            Assert.AreEqual(2, options.ArrayArgument.Length);
            Assert.AreEqual("First Second Third", options.ArrayArgument[0]);
            Assert.AreEqual("NextValue", options.ArrayArgument[1]);
        }

        #region Option Classes

        private class Options_SingleBoolArg
        {
            [Argument]
            public bool Help { get; set; }
        }

        private class Options_RenamedArg
        {
            [Argument("help")]
            public bool ShowHelp { get; set; }
        }

        private class Options_TwoBoolArgs
        {
            [Argument]
            public bool Argument1 { get; set; }

            [Argument]
            public bool Argument2 { get; set; }
        }

        private class Options_HasPropertyWithGeneratedCodeAttribute
        {
            [GeneratedCode("", "")]
            public string Test { get; set; }
        }

        private class Options_HasExclusiveHelp
        {
            [Argument("help", Exclusive = true)]
            public bool Help { get; set; }

            [Argument("run")]
            public bool Run { get; set; }
        }

        private class Options_HasExclusiveHelpAndRequiredMode
        {
            [Argument("help", Exclusive = true)]
            public bool Help { get; set; }

            [Argument("mode", Mandatory = true)]
            public string Run { get; set; }
        }

        private class Options_WithStringArg
        {
            [Argument("arg")]
            public string Argument { get; set; }

            [Argument("array")]
            public string[] ArrayArgument { get; set; }
        }

        #endregion
    }
}