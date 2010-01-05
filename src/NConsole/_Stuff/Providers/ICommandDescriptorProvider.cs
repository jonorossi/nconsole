using System;
using NConsole.Internal;

namespace NConsole.Providers
{
    /// <summary>
    /// Defines the contract for implementations that should collect from one or more sources the meta
    /// information that dictates the <see cref="ICommand"/> behavior and the arguments it exposes.
    /// </summary>
    public interface ICommandDescriptorProvider
    {
        /// <summary>
        /// Builds the descriptor.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <returns></returns>
        CommandDescriptor BuildDescriptor(Type commandType);
    }
}