using System;
using NConsole.Internal;
using NConsole.Tests.CommandClasses;
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

            CommandDescriptor descriptor = commandRegistry.GetDescriptor("test", null);

            Assert.That(descriptor.Name, Is.EqualTo("test"));
        }

        [Test]
        public void GetDescriptor_ReturnsRegisteredCommandUsingCommandAttribute()
        {
            CommandRegistry commandRegistry = new CommandRegistry();
            commandRegistry.Register(typeof(TestAttributedCommand));

            CommandDescriptor descriptor = commandRegistry.GetDescriptor("overridden", null);

            Assert.That(descriptor.Name, Is.EqualTo("overridden"));
        }

        [Test]
        public void GetDescriptor_ReturnsRegisteredSubCommand()
        {
            CommandRegistry commandRegistry = new CommandRegistry();
            commandRegistry.Register(typeof(RemoteCommand));

            CommandDescriptor remoteDescriptor = commandRegistry.GetDescriptor("remote", null);
            CommandDescriptor addDescriptor = commandRegistry.GetDescriptor("add", remoteDescriptor);

            Assert.That(remoteDescriptor.Name, Is.EqualTo("remote"));
            Assert.That(addDescriptor.Name, Is.EqualTo("add"));
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

            CommandDescriptor descriptor = commandRegistry.GetDescriptor("", null);

            Assert.That(descriptor.Name, Is.EqualTo("test"));
        }

        #region Test Commands

        private class TestCommand : ICommand
        {
            public void Execute()
            {
            }
        }

        [Command("overridden")]
        private class TestAttributedCommand : ICommand
        {
            public void Execute()
            {
            }
        }

        #endregion
    }
}