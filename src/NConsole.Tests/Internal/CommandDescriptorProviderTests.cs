using System;
using NConsole.Internal;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class CommandDescriptorProviderTests
    {
        [Test]
        public void ThrowsIfCommandDoesNotImplementICommand()
        {
            CommandDescriptorProvider provider = new CommandDescriptorProvider();

            Exception ex = Assert.Throws<Exception>(() => provider.BuildDescriptor(typeof(int)));

            Assert.That(ex.Message, Is.EqualTo("Command type 'System.Int32' does not implement ICommand."));
        }

        [Test]
        public void CommandTypeIsSet()
        {
            CommandDescriptorProvider provider = new CommandDescriptorProvider();

            CommandDescriptor descriptor = provider.BuildDescriptor(typeof(TestClass));

            Assert.That(descriptor.CommandType, Is.EqualTo(typeof(TestClass)));
        }

        [Test]
        public void DefaultCommandNameIsTheClassName()
        {
            CommandDescriptorProvider provider = new CommandDescriptorProvider();

            CommandDescriptor descriptor = provider.BuildDescriptor(typeof(TestClass));

            Assert.That(descriptor.Name, Is.EqualTo("testclass"));
        }

        [Test]
        public void DefaultCommandNameIsTheClassNameWithoutCommand()
        {
            CommandDescriptorProvider provider = new CommandDescriptorProvider();

            CommandDescriptor descriptor = provider.BuildDescriptor(typeof(TestCommand));

            Assert.That(descriptor.Name, Is.EqualTo("test"));
        }

        [Test]
        public void CommandsMustHaveAName()
        {
            CommandDescriptorProvider provider = new CommandDescriptorProvider();

            Exception ex = Assert.Throws<Exception>(() => provider.BuildDescriptor(typeof(Command)));

            Assert.That(ex.Message, Is.EqualTo("Command type 'NConsole.Tests.Internal.CommandDescriptorProviderTests+Command' does not provide a command name."));
        }

        #region Test Commands

        private class TestClass : ICommand
        {
            public void Execute()
            {
            }
        }

        private class TestCommand : ICommand
        {
            public void Execute()
            {
            }
        }

        private class Command : ICommand
        {
            public void Execute()
            {
            }
        }

        #endregion
    }
}