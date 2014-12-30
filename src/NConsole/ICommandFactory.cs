using System;

namespace NConsole
{
    /// <summary>
    /// Defines the contract that the <see cref="ConsoleController"/> uses to perform the creation and disposal of
    /// <see cref="ICommand"/> instances.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="commandType">The type of the command to create.</param>
        /// <returns>An instance of the command type.</returns>
        ICommand Create(Type commandType);

        /// <summary>
        /// Implementors should perform their logic to release the <see cref="ICommand"/> instance and its resources.
        /// </summary>
        /// <param name="command">Command instance to release.</param>
        void Release(ICommand command);
    }
}