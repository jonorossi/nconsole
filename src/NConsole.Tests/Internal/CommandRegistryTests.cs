using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class CommandRegistryTests
    {
        [Test]
        public void GetDescriptor_ReturnsRegisteredCommand()
        {
            CommandRegistry commandRegistry = new CommandRegistry();
            commandRegistry.Register(typeof(TestCommand));

            CommandDescriptor descriptor = commandRegistry.GetDescriptor("test");

            Assert.That(descriptor.Name, Is.EqualTo("test"));
        }

        #region Test Commands

        private class TestCommand : ICommand
        {
            public void Execute()
            {
            }
        }

        #endregion
    }
}