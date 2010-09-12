using System;

namespace NConsole.Internal
{
    /// <summary>
    /// Standard implementation of <see cref="ICommandFactory"/>.
    /// It inspects assemblies looking for concrete classes that extend <see cref="ICommand"/>.
    /// </summary>
    internal class DefaultCommandFactory : ICommandFactory
    {
        public ICommand Create(Type commandType)
        {
            if (commandType == null) throw new ArgumentNullException("commandType");

            return (ICommand)Activator.CreateInstance(commandType);
        }

        public void Release(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            // Determine if the command needs to be disposed, otherwise this is just a noop
            IDisposable disposable = command as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}