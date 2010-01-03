using System;

namespace NConsole
{
    public interface ICommandFactory
    {
        /// <summary>
        /// Registers the command type with the factory so that it can be resolved.
        /// </summary>
        /// <param name="commandType"></param>
        void Register(Type commandType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        ICommand Resolve(string commandName);

        //void Release(ICommand command);
    }
}