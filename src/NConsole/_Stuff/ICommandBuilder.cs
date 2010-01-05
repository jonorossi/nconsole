using NConsole.Model;

namespace NConsole
{
    // Represents a concern that will be applied to a component instance
    // during commission or decomission phase.
    public interface ICommandBuilder
    {
        // <summary>
        // Implementors should act on the instance in response to 
        // a decomission or commission phase. 
        // </summary>
        // <param name="model">The model.</param>
        // <param name="component">The component.</param>
        void Apply(CommandModel model, object component);
    }
}