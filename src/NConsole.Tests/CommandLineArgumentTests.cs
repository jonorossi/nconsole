using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineArgumentTests
    {
        [Test]
        public void WillBindStringArgument()
        {
            // Set the value on the argument
            CommandLineArgument argument = new CommandLineArgument(new CommandLineArgumentAttribute("stringProperty"),
                typeof(Options_OneStringProperty).GetProperty("StringProperty"));
            argument.SetValue("ExampleValue");

            // Ensure that the argument will bind to a string property
            var options = new Options_OneStringProperty();
            Assert.IsNull(options.StringProperty);
            argument.Bind(options);
            Assert.AreEqual("ExampleValue", options.StringProperty);
        }

        [Test]
        public void SetValueThrowsIfNotCollectionAndSetValueHasAlreadyBeenCalled()
        {
            CommandLineArgument argument = new CommandLineArgument(new CommandLineArgumentAttribute("arg"), null);
            argument.SetValue("firstValue");

            AssertEx.Throws<CommandLineArgumentException>("Argument '/arg' can only appear once.",
                delegate { argument.SetValue("secondValue"); }
            );
        }

        #region Option Classes

        private class Options_OneStringProperty
        {
            public string StringProperty { get; set; }
        }

        #endregion
    }
}