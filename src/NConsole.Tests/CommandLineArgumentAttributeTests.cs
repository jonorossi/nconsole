using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineArgumentAttributeTests
    {
        [Test]
        public void ArgumentWithEmptyStringNameThrowsException()
        {
            AssertEx.Throws<CommandLineArgumentException>("Command line arguments must have a name. Use the default constructor " +
                "if the name should match the property name.", delegate {
                    new CommandLineArgumentAttribute("");
                }
            );
        }

        [Test]
        public void VerifyOptionDefaults()
        {
            CommandLineArgumentAttribute attribute = new CommandLineArgumentAttribute();

            Assert.IsNull(attribute.Name);
            Assert.IsFalse(attribute.Required);
            Assert.IsFalse(attribute.Exclusive);
            Assert.IsNull(attribute.Description);
        }

        [Test]
        public void VerifyOptionInitialisation()
        {
            CommandLineArgumentAttribute attribute = new CommandLineArgumentAttribute("arg");
            attribute.Required = true;
            attribute.Exclusive = true;
            attribute.Description = "Example description";

            Assert.AreEqual("arg", attribute.Name);
            Assert.IsTrue(attribute.Required);
            Assert.IsTrue(attribute.Exclusive);
            Assert.AreEqual("Example description", attribute.Description);
        }
    }
}