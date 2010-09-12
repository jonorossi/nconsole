using NConsole.Tests.CommandClasses;
using NUnit.Framework;
using Rhino.Mocks;

namespace NConsole.Tests
{
    [TestFixture]
    public class ConsoleControllerTests
    {
        [Test]
        public void Execute_ReturnsNonZeroWhenNoCommandsAreAvailable()
        {
            var controller = new ConsoleController();

            int exitCode = controller.Execute(new string[] { });

            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void Execute_DefaultCommand()
        {
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(LsAppCommand));
            controller.SetDefaultCommand(typeof(LsAppCommand));

            int exitCode = controller.Execute(new string[] { });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.IsFalse(command.LongListingFormat);
            Assert.IsFalse(command.HumanReadableSizes);
            Assert.IsFalse(command.SortByFileSize);
        }

        [Test]
        public void Execute_DefaultCommandWithArguments()
        {
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(LsAppCommand));
            controller.SetDefaultCommand(typeof(LsAppCommand));

            int exitCode = controller.Execute(new[] { "-l", "-h", "-S" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.IsTrue(command.LongListingFormat);
            Assert.IsTrue(command.HumanReadableSizes);
            Assert.IsTrue(command.SortByFileSize);
        }

        [Test]
        public void Execute_DefaultCommandWithArgumentsUsingLongName()
        {
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);

            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(LsAppCommand));
            controller.SetDefaultCommand(typeof(LsAppCommand));

            int exitCode = controller.Execute(new[] { "--human-readable" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.IsFalse(command.LongListingFormat);
            Assert.IsTrue(command.HumanReadableSizes);
            Assert.IsFalse(command.SortByFileSize);
        }

        [Test]
        public void Execute_DefaultCommandWithMultipleArguments()
        {
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(LsAppCommand));
            controller.SetDefaultCommand(typeof(LsAppCommand));

            int exitCode = controller.Execute(new[] { "--ignore=ntuser*", "--ignore=NTUSER*" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(exitCode, Is.EqualTo(0));
            CollectionAssert.AreEqual(new[] { "ntuser*", "NTUSER*" }, command.IgnorePatterns);
        }

        [Test]
        public void Execute_RunNonExistantCommand()
        {
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            var controller = new ConsoleController(commandFactory);

            int exitCode = controller.Execute(new[] { "donothing" });

            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void Execute_RunSpecificCommand()
        {
            var command = MockRepository.GenerateStub<CloneCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(CloneCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(CloneCommand));

            int exitCode = controller.Execute(new[] { "clone" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(exitCode, Is.EqualTo(0));
        }
    }
}