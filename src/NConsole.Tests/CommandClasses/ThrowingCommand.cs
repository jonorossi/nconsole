using System;

namespace NConsole.Tests.CommandClasses
{
    /// <summary>
    /// Throws an <see cref="Exception"/>.
    /// </summary>
    public class ThrowingCommand : ICommand
    {
        public void Execute()
        {
            throw new Exception();
        }
    }
}