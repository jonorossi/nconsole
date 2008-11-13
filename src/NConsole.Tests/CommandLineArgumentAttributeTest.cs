using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class CommandLineArgumentAttributeTest
    {
        [Test]
        public void ArgumentWithEmptyStringNameThrowsException()
        {
            AssertEx.Throws<CommandLineArgumentException>("Command line arguments must have a name. Use the default constructor " +
                "if the property name should be used.", delegate {
                    new CommandLineArgumentAttribute("");
                }
            );
        }
    }
}