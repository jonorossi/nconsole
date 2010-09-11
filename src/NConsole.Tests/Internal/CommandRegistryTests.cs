using System;
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

        [Test]
        public void SetDefaultCommand_ThrowsIfCommandNotRegistered()
        {
            CommandRegistry commandRegistry = new CommandRegistry();

            var ex = Assert.Throws<Exception>(() => commandRegistry.SetDefaultCommand(typeof(TestCommand)));

            Assert.That(ex.Message, Is.EqualTo("Command NConsole.Tests.Internal.CommandRegistryTests+TestCommand is not registered."));
        }

        [Test]
        public void SetDefaultCommand_UpdatesRegisteredCommandsDescriptor()
        {
            CommandRegistry commandRegistry = new CommandRegistry();
            commandRegistry.Register(typeof(TestCommand));
            commandRegistry.SetDefaultCommand(typeof(TestCommand));

            CommandDescriptor descriptor = commandRegistry.GetDescriptor("");

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