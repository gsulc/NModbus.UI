using Prism.Commands;

namespace NModbus.UI.Common.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand SaveCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand SaveCommand { get; } = new CompositeCommand();
    }
}
