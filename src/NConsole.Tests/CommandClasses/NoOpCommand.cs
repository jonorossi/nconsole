namespace NConsole.Tests.CommandClasses
{
    /// <summary>
    /// Implements <see cref="ICommand"/> and performs no operation when Execute is called.
    /// </summary>
    public class NoOpCommand : ICommand
    {
        public virtual void Execute()
        {
        }
    }
}