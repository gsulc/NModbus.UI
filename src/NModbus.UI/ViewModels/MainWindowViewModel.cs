using NModbus.UI.Common.Core;
using NModbus.UI.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;

namespace NModbus.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _ea;

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;
            _ea = ea;
            _regionManager.RegisterViewWithRegion("ConnectionRegion", typeof(ConnectionView));
            _regionManager.RegisterViewWithRegion("ModbusInteractionRegion", typeof(ModbusInteractionView));
            _regionManager.RegisterViewWithRegion("ErrorRegion", typeof(ErrorView));
            CloseCommand = new DelegateCommand(OnClose);
        }

        public ICommand CloseCommand { get; private set; }

        private void OnClose()
        {
            _ea.GetEvent<CloseEvent>().Publish();
        }
    }
}
