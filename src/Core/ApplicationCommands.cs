using Prism.Commands;

namespace NModbus.UI.Common.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand ConnectCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand ConnectCommand { get; } = new CompositeCommand();
    }
}
