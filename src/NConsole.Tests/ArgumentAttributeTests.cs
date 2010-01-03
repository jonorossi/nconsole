using System;
using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class ArgumentAttributeTests
    {
        [Test]
        public void ArgumentWithEmptyStringNameThrowsException()
        {
            // Arrange

            // Act
            Exception ex = Record.Exception(() => new ArgumentAttribute(""));

            // Assert
            Assert.IsInstanceOf(typeof(CommandLineArgumentException), ex);
            Assert.AreEqual("Command line arguments must have a name. Use the default constructor " +
                "if the name should match the property name.", ex.Message);
        }

        [Test]
        public void VerifyOptionDefaults()
        {
            // Arrange

            // Act
            ArgumentAttribute attribute = new ArgumentAttribute();

            // Assert
            Assert.IsNull(attribute.Name);
            Assert.IsFalse(attribute.Mandatory);
            Assert.IsFalse(attribute.Exclusive);
            Assert.IsNull(attribute.Description);
        }

        [Test]
        public void VerifyOptionInitialisation()
        {
            // Arrange

            // Act
            ArgumentAttribute attribute = new ArgumentAttribute("arg") {
                Mandatory = true,
                Exclusive = true,
                Description = "Example description"
            };

            // Assert
            Assert.AreEqual("arg", attribute.Name);
            Assert.IsTrue(attribute.Mandatory);
            Assert.IsTrue(attribute.Exclusive);
            Assert.AreEqual("Example description", attribute.Description);
        }
    }
}