//using NUnit.Framework;

//namespace NConsole.Tests
//{
//    [TestFixture, Ignore]
//    public class CommandLineParserDataTypeTests
//    {
//        [Test]
//        public void StringArguments()
//        {
//            // Arrange
//            var parser = new CommandLineParser<Options_WithStringArguments>();
//
//            // Act
//            var options = parser.ParseArguments(new[] { "/server:box", "/db:dev" });
//
//            // Assert
//            Assert.AreEqual("box", options.Server);
//            Assert.AreEqual("dev", options.Database);
//        }

//        [Test]
//        public void BooleanArguments()
//        {
//            // Arrange
//            var parser = new CommandLineParser<Options_WithBooleanArguments>();

//            // Act
//            var options = parser.ParseArguments(new[] { "/help", "/debug+", "/release-" });

//            // Assert
//            Assert.IsTrue(options.Help);
//            Assert.IsTrue(options.Debug);
//            Assert.IsFalse(options.Release);
//        }

//        [Test]
//        public void NumericArguments()
//        {
//            // Arrange
//            var parser = new CommandLineParser<Options_WithNumericArguments>();

//            // Act
//            var options = parser.ParseArguments(new[] { "/intarg:123456", "/doublearg:123.456" });

//            // Assert
//            Assert.AreEqual(123456, options.IntArg);
//            Assert.AreEqual(123.456, options.DoubleArg);
//        }

//        [Test]
//        public void ParserDoesNotSetValueOnNullableArgumentIfNotSpecified()
//        {
//            // Arrange
//            var parser = new CommandLineParser<Options_WithNullableInt>();

//            // Act
//            var options = parser.ParseArguments(new string[0]);

//            // Assert
//            Assert.IsFalse(options.Timeout.HasValue);
//        }

//        [Test]
//        public void ParserSetsValueOfNullableInt()
//        {
//            // Arrange
//            var parser = new CommandLineParser<Options_WithNullableInt>();

//            // Act
//            var options = parser.ParseArguments(new[] { "/timeout:100" });

//            // Assert
//            Assert.IsTrue(options.Timeout.HasValue);
//            Assert.AreEqual(100, options.Timeout.Value);
//        }

//        #region Option Classes

////        private class Options_WithStringArguments
////        {
////            [Argument]
////            public string Server { get; set; }
////            [Argument("db")]
////            public string Database { get; set; }
////        }

//        private class Options_WithBooleanArguments
//        {
//            [Argument]
//            public bool Help { get; set; }
//            [Argument]
//            public bool Debug { get; set; }
//            [Argument]
//            public bool Release { get; set; }
//        }

//        private class Options_WithNumericArguments
//        {
//            [Argument]
//            public int IntArg { get; set; }
//            [Argument]
//            public double DoubleArg { get; set; }
//        }

//        private class Options_WithNullableInt
//        {
//            [Argument]
//            public int? Timeout { get; set; }
//        }

//        #endregion
//    }
//}