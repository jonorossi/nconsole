using System;
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

        [Test]
        public void Execute_DoubleQuotedStringValue()
        {
            var command = MockRepository.GenerateStub<StringArgCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(StringArgCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(StringArgCommand));
            controller.SetDefaultCommand(typeof(StringArgCommand));

            controller.Execute(new[] { "/arg:\"First Second Third\"" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(command.Argument, Is.EqualTo("First Second Third"));
        }

        [Test]
        public void Execute_DoubleQuotedStringValuesInArray()
        {
            var command = MockRepository.GenerateStub<StringArgCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(StringArgCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(StringArgCommand));
            controller.SetDefaultCommand(typeof(StringArgCommand));

            controller.Execute(new[] { "/array:\"First Second Third\",NextValue" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(command.ArrayArgument.Length, Is.EqualTo(2));
            Assert.That(command.ArrayArgument[0], Is.EqualTo("First Second Third"));
            Assert.That(command.ArrayArgument[1], Is.EqualTo("NextValue"));
        }

        [Test]
        public void Execute_EnableThenDisableFlagArgument()
        {
            var command = MockRepository.GenerateStub<LsAppCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(LsAppCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(LsAppCommand));
            controller.SetDefaultCommand(typeof(LsAppCommand));

            controller.Execute(new[] { "--human-readable+", "--human-readable-" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(command.HumanReadableSizes, Is.EqualTo(false));
        }

        [Test]
        public void Execute_PositionalArguments()
        {
            var command = MockRepository.GenerateStub<CloneCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(CloneCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(CloneCommand));

            controller.Execute(new[] { "clone", "the_repo" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(command.Repository, Is.EqualTo("the_repo"));
        }

        [Test]
        public void Execute_MixOfPositionalAndNamedArgumentsWithArrays()
        {
            var command = MockRepository.GenerateStub<PositionalStringArrayArgCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(PositionalStringArrayArgCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(PositionalStringArrayArgCommand));

            controller.Execute(new[] { "positionalstringarrayarg", "value_one", "--arg", "value_two" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(command.ArrayArgument, Is.EqualTo(new[] { "value_one", "value_two" }));
            Assert.That(command.Argument);
        }

        [Test]
        public void Execute_MixOfPositionalAndNamedArgumentsWithMultiplePositionalArguments()
        {
            var command = MockRepository.GenerateStub<PositionalStringArgCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(PositionalStringArgCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(PositionalStringArgCommand));

            controller.Execute(new[] { "positionalstringarg", "value_one", "--arg", "value_two" });

            command.AssertWasCalled(c => c.Execute());
            Assert.That(command.Argument1, Is.EqualTo("value_one"));
            Assert.That(command.Argument2, Is.EqualTo("value_two"));
            Assert.That(command.FlagArgument);
        }

        [Test]
        public void Execute_CreatesAndDelegatesToSubCommand()
        {
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            RemoteAddCommand command = new RemoteAddCommand();
            commandFactory.Stub(f => f.Create(typeof(RemoteAddCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(RemoteCommand));
            controller.RethrowExceptions = true;

            controller.Execute(new[] { "remote", "add", "test_name" });

            Assert.That(command.Name, Is.EqualTo("test_name"));
        }

        [Test]
        public void RethrowExceptions_True()
        {
            var command = MockRepository.GenerateStub<ThrowingCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(ThrowingCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(ThrowingCommand));
            controller.RethrowExceptions = true;

            Assert.Throws<Exception>(delegate { controller.Execute(new[] { "throwing" }); });
        }

        [Test]
        public void RethrowExceptions_False()
        {
            var command = MockRepository.GenerateStub<ThrowingCommand>();
            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
            commandFactory.Stub(f => f.Create(typeof(ThrowingCommand))).Return(command);
            var controller = new ConsoleController(commandFactory);
            controller.Register(typeof(ThrowingCommand));
            controller.RethrowExceptions = false;

            Assert.DoesNotThrow(delegate { controller.Execute(new[] { "throwing" }); });
        }
    }
}