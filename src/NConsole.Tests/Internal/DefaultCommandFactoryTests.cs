using System;
using System.Linq;
using NConsole.Internal;
using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests.Internal
{
    [TestFixture]
    public class DefaultCommandFactoryTests
    {
        [Test]
        public void ThrowsIfCommandDoesNotImplementICommand()
        {
            // Arrange
            DefaultCommandFactory commandFactory = new DefaultCommandFactory();

            // Act
            Exception ex = Record.Exception(() => commandFactory.Register(typeof(int)));

            // Assert
            Assert.AreEqual("Command type 'System.Int32' does not implement ICommand.", ex.Message);
        }

        [Test]
        public void CanRegisterCommandAndRetrieveCommandDescriptor()
        {
            // Arrange
            DefaultCommandFactory commandFactory = new DefaultCommandFactory();
            commandFactory.Register(typeof(TestCommand));

            // Act
            var descriptors = commandFactory.CommandDescriptors;

            // Assert
            Assert.AreEqual(1, descriptors.Count());
            Assert.AreEqual(typeof(TestCommand), descriptors.First().CommandType);
        }

        [Test]
        public void CanResolveCommand()
        {
            // Arrange
            DefaultCommandFactory commandFactory = new DefaultCommandFactory();
            commandFactory.Register(typeof(TestCommand));

            // Act
            ICommand command = commandFactory.Resolve("test");

            // Assert
            Assert.IsNotNull(command);
            Assert.IsInstanceOf<TestCommand>(command);
        }

        [Test]
        public void CanResolveSecondCommandRegistered()
        {
            // Arrange
            DefaultCommandFactory commandFactory = new DefaultCommandFactory();
            commandFactory.Register(typeof(TestCommand));
            commandFactory.Register(typeof(Test2Command));

            // Act
            ICommand command = commandFactory.Resolve("test2");

            // Assert
            Assert.IsNotNull(command);
            Assert.IsInstanceOf<Test2Command>(command);
        }

        private class TestCommand : ICommand
        {
            public void Execute()
            {
            }
        }

        private class Test2Command : ICommand
        {
            public void Execute()
            {
            }
        }
    }
}