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
            // Arrange
            var controller = new ConsoleController();

            // Act
            int exitCode = controller.Execute(new string[] { });

            // Assert
            Assert.AreEqual(1, exitCode);
        }

        [Test]
        public void Execute_()
        {
            Assert.Fail("TODO");
            // ls --ignore=ntuser* -ignore=NTUSER*

//            // Arrange
//            var command = MockRepository.GenerateStub<NoOpCommand>();
//            var commandFactory = MockRepository.GenerateStub<ICommandFactory>();
//            commandFactory.Stub(f => f.Create(typeof(NoOpCommand))).Return(command);
//
//            var controller = new ConsoleController();
//
//            // Act
//            int exitCode = controller.Execute(new string[] { });
//
//            // Assert
//            command.AssertWasCalled(c => c.Execute());
//            Assert.AreEqual(1, exitCode);
        }
    }
}