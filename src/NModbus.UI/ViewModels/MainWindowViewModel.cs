using NModbus.UI.Common.Core;
using NModbus.UI.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace NModbus.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            AppCommands = applicationCommands;

            _regionManager.RegisterViewWithRegion("ConnectionRegion", typeof(ConnectionView));
            _regionManager.RegisterViewWithRegion("ModbusInteractionRegion", typeof(ModbusInteractionView));
            _regionManager.RegisterViewWithRegion("ErrorRegion", typeof(ErrorView));
        }

        public IApplicationCommands AppCommands { get; private set; }
    }
}
