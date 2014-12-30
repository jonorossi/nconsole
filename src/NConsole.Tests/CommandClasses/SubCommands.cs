using System;

namespace NConsole.Tests.CommandClasses
{
    [Command("remote")]
    [SubCommand(typeof(RemoteAddCommand))]
    public class RemoteCommand : ICommand
    {
        public void Execute()
        {
        }
    }

    [Command("add")]
    public class RemoteAddCommand : ICommand
    {
        [Argument(Position = 0)]
        public string Name { get; set; }

        public void Execute()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new Exception("Name cannot be null.");
            }
        }
    }
}