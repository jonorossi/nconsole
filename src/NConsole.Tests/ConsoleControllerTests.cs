using System;
using NUnit.Framework;

namespace NConsole.Tests
{
    [TestFixture]
    public class ConsoleControllerTests
    {
        [Test]
        public void ExecuteWithNoCommandsAndNoArgs()
        {
            // Arrange
            var controller = new ConsoleController();

            // Act
            int exitCode = controller.Execute(new string[] { });

            // Assert
            Assert.AreEqual(0, exitCode);
        }

        [Test]
        public void ExecutedCommandByName()
        {
            // Arrange
            RelayCommandFactory commandFactory = new RelayCommandFactory();
            int count = 0;
            commandFactory.Add(new RelayCommand(delegate { count++; }));

            var controller = new ConsoleController(commandFactory);

            //controller.Register(typeof(RelayCommand));

            // Act
            int exitCode = controller.Execute(new[] { "update" });

            // Assert
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, exitCode);
        }



        private class RelayCommandFactory : ICommandFactory
        {
            private RelayCommand command;

            public void Register(Type commandType)
            {
            }

            public ICommand Resolve(string commandName)
            {
                return command;
            }

            public void Add(RelayCommand command)
            {
                this.command = command;
            }
        }

        private class RelayCommand : ICommand
        {
            private readonly Action code;

            public RelayCommand(Action code)
            {
                this.code = code;
            }

            public void Execute()
            {
                code();
            }
        }

//        private class UpdateCommand
//        {
//            public DirectoryInfo Path { get; set; }
//        }
    }
}