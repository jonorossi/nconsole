//using System;
//using NConsole.Tests.Utils;
//using NUnit.Framework;

//namespace NConsole.Tests
//{
//    [TestFixture, Ignore]
//    public class ArgumentTests
//    {
//        [Test]
//        public void WillBindStringArgument()
//        {
//            // Arrange
//            Argument argument = new Argument(new ArgumentAttribute("stringProperty"),
//                typeof(Options_OneStringProperty).GetProperty("StringProperty"));
//            argument.SetValue("ExampleValue");
//
//            var options = new Options_OneStringProperty();
//
//            // Act
//            argument.Bind(options);
//
//            // Assert
//            Assert.AreEqual("ExampleValue", options.StringProperty);
//        }
//
//        [Test]
//        public void SetValueThrowsIfNotCollectionAndSetValueHasAlreadyBeenCalled()
//        {
//            // Arrange
//            Argument argument = new Argument(new ArgumentAttribute("arg"), null);
//            argument.SetValue("firstValue");
//
//            // Act
//            Exception ex = Record.Exception(() => argument.SetValue("secondValue"));
//
//            // Assert
//            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
//            Assert.AreEqual("Argument '/arg' can only appear once.", ex.Message);
//        }
//
//        [Test]
//        public void ArgumentBindThrowsIfRequiredSetValueNotCalled()
//        {
//            // Arrange
//            ArgumentAttribute attribute = new ArgumentAttribute("arg") { Mandatory = true };
//            Argument argument = new Argument(attribute, null);
//
//            // Act
//            Exception ex = Record.Exception(() => argument.Bind(null));
//
//            // Assert
//            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
//            Assert.AreEqual("Argument '/arg' must appear at least once.", ex.Message);
//        }

//        #region Option Classes

//        private class Options_OneStringProperty
//        {
//            public string StringProperty { get; set; }
//        }

//        #endregion
//    }
//}