using NConsole.Tests.CommandClasses;
using NUnit.Framework;
using Rhino.Mocks;

namespace NConsole.Tests
{
    [TestFixture]
    public class ConsoleControllerSingleCommandAppTests
    {
        [Test]
        public void Execute_DefaultCommand()
        {
            // Arrange
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);

            var controller = new ConsoleController(commandFactory) { DefaultCommand = typeof(LsAppCommand) };

            // Act
            int exitCode = controller.Execute(new string[] { });

            // Assert
            command.AssertWasCalled(c => c.Execute());
            Assert.AreEqual(0, exitCode);
            Assert.IsFalse(command.LongListingFormat);
            Assert.IsFalse(command.HumanReadableSizes);
            Assert.IsFalse(command.SortByFileSize);
        }

        [Test]
        public void Execute_DefaultCommandWithArguments()
        {
            // Arrange
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);

            var controller = new ConsoleController(commandFactory) { DefaultCommand = typeof(LsAppCommand) };

            // Act
            int exitCode = controller.Execute(new[] { "-l", "-h", "-S" });

            // Assert
            command.AssertWasCalled(c => c.Execute());
            Assert.AreEqual(0, exitCode);
            Assert.IsTrue(command.LongListingFormat);
            Assert.IsTrue(command.HumanReadableSizes);
            Assert.IsTrue(command.SortByFileSize);
        }

        [Test]
        public void Execute_DefaultCommandWithArgumentsUsingLongName()
        {
            // Arrange
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);

            var controller = new ConsoleController(commandFactory) { DefaultCommand = typeof(LsAppCommand) };

            // Act
            int exitCode = controller.Execute(new[] { "--human-readable" });

            // Assert
            command.AssertWasCalled(c => c.Execute());
            Assert.AreEqual(0, exitCode);
            Assert.IsFalse(command.LongListingFormat);
            Assert.IsTrue(command.HumanReadableSizes);
            Assert.IsFalse(command.SortByFileSize);
        }
    }
}