using System;
using NConsole.Tests.CommandClasses;
using NConsole.Tests.Utils;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class DefaultCommandFactoryTests
    {
        [Test]
        public void Create_CanCreateInstance()
        {
            // Arrange
            var commandFactory = new DefaultCommandFactory();

            // Act
            var command = commandFactory.Create(typeof(NoOpCommand));

            // Assert
            Assert.IsInstanceOf(typeof(NoOpCommand), command);
        }

        [Test]
        public void Create_ThrowsIfCannotCreateInstance()
        {
            // Arrange
            var commandFactory = new DefaultCommandFactory();

            // Act
            var ex = Record.Exception(() => commandFactory.Create(typeof(int)));

            // Assert
            Assert.IsInstanceOf(typeof(InvalidCastException), ex);
        }

        [Test]
        public void Release_InvokesDispose()
        {
            // Arrange
            var commandFactory = new DefaultCommandFactory();
            var command = (DisposableCommand)commandFactory.Create(typeof(DisposableCommand));

            // Act
            commandFactory.Release(command);

            // Assert
            Assert.IsFalse(command.IsAlive);
        }
    }
}