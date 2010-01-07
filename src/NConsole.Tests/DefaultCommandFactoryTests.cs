using System;
using NConsole.Tests.CommandClasses;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class DefaultCommandFactoryTests
    {
        [Test]
        public void Create_CanCreateInstance()
        {
            var commandFactory = new DefaultCommandFactory();

            var command = commandFactory.Create(typeof(NoOpCommand));

            Assert.IsInstanceOf(typeof(NoOpCommand), command);
        }

        [Test]
        public void Create_ThrowsIfCannotCreateInstance()
        {
            var commandFactory = new DefaultCommandFactory();

            Assert.Throws<InvalidCastException>(() => commandFactory.Create(typeof(int)));
        }

        [Test]
        public void Release_InvokesDispose()
        {
            var commandFactory = new DefaultCommandFactory();
            var command = (DisposableCommand)commandFactory.Create(typeof(DisposableCommand));

            commandFactory.Release(command);

            Assert.IsFalse(command.IsAlive);
        }
    }
}