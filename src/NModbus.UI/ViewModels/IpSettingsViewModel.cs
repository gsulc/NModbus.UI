using NModbus.UI.Common.Core;
using NModbus.UI.Properties;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NModbus.UI.ViewModels
{
    public class IpSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        IEventAggregator _eventAggregator;

        public IpSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
            _eventAggregator.GetEvent<CloseEvent>().Subscribe(OnClose);
        }

        public string Hostname { get; set; } = Settings.Default.Hostname;
        public int Port { get; set; } = Settings.Default.Port;

        public bool KeepAlive => false;

        private void HandleConnectionRequest(ModbusType modbusType)
        {
            if (!IsAnIpType(modbusType))
                return;

            var ipSettings = new IpSettings()
            {
                ModbusType = modbusType,
                Hostname = Hostname,
                Port = Port
            };

            _eventAggregator.GetEvent<ConnectionRequestEvent>().Publish(ipSettings);
        }

        private bool IsAnIpType(ModbusType modbusType)
        {
            return modbusType == ModbusType.Tcp
                || modbusType == ModbusType.Udp
                || modbusType == ModbusType.RtuOverTcp
                || modbusType == ModbusType.RtuOverUdp;
        }

        private void OnClose()
        {
            Settings.Default.Hostname = Hostname;
            Settings.Default.Port = Port;
            Settings.Default.Save();
        }
    }
}
