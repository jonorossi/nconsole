using System;
using NConsole.Internal;
using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class CommandDescriptorTests
    {
        [Test]
        public void ThrowsIfCommandDoesNotImplementICommand()
        {
            // Arrange

            // Act
            Exception ex = Record.Exception(() => CommandDescriptor.FromType(typeof(int)));

            // Assert
            Assert.AreEqual("Command type 'System.Int32' does not implement ICommand.", ex.Message);
        }

        [Test]
        public void DefaultCommandNameIsTheClassName()
        {
            // Arrange

            // Act
            CommandDescriptor descriptor = CommandDescriptor.FromType(typeof(TestClass));

            // Assert
            Assert.AreEqual("testclass", descriptor.Name);
        }

        [Test]
        public void DefaultCommandNameIsTheClassNameWithoutCommand()
        {
            // Arrange

            // Act
            CommandDescriptor descriptor = CommandDescriptor.FromType(typeof(TestCommand));

            // Assert
            Assert.AreEqual("test", descriptor.Name);
        }

        [Test]
        public void CommandsMustHaveAName()
        {
            // Arrange

            // Act
            Exception ex = Record.Exception(() => CommandDescriptor.FromType(typeof(Command)));

            // Assert
            Assert.AreEqual("Command type 'NConsole.Tests.Internal.CommandDescriptorTests+Command' does not provide a command name.", ex.Message);
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