using NModbus.UI.Common.Core;
using Prism.Events;
using Prism.Mvvm;

namespace NModbus.UI.ViewModels
{
    public class IpSettingsViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public IpSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        public string Hostname { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 502;

        //public DelegateCommand ConnectionRequestComand { get; private set; }

        private void HandleConnectionRequest(ModbusType connectionType)
        {
            var ipSettings = new IpSettings()
            {
                ModbusType = connectionType,
                Hostname = Hostname,
                Port = Port
            };
            _eventAggregator.GetEvent<IpConnectionRequestEvent>().Publish(ipSettings);
        }
    }
}
