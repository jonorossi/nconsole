using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class CommandDescriptorTests
    {
        [Test]
        public void TestInitialValues()
        {
            CommandDescriptor descriptor = new CommandDescriptor();

            Assert.That(descriptor.Name, Is.Null);
            Assert.That(descriptor.CommandType, Is.Null);
            Assert.That(descriptor.Aliases, Is.Not.Null);
            Assert.That(descriptor.Arguments, Is.Not.Null);
            Assert.That(descriptor.SubCommands, Is.Not.Null);
        }
    }
}