namespace NConsole
{
    /// <summary>
    /// Defines a contract the all commands must implement.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The command should perform its logic.
        /// </summary>
        void Execute();
    }
}