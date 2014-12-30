using System;

namespace NConsole.Tests.CommandClasses
{
    /// <summary>
    /// Implements <see cref="ICommand"/> as well as <see cref="IDisposable"/>, and exposes a flag indicating
    /// whether Dispose has been called.
    /// </summary>
    public class DisposableCommand : NoOpCommand, IDisposable
    {
        public DisposableCommand()
        {
            IsAlive = true;
        }

        /// <summary>
        /// Gets whether this <see cref="ICommand"/> has been disposed.
        /// </summary>
        public bool IsAlive { get; private set; }

        public void Dispose()
        {
            if (!IsAlive) throw new Exception("Command already disposed.");

            IsAlive = false;
        }
    }
}