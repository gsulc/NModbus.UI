using NModbus.UI.Common.Core;
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
        }

        public string Hostname { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 502;

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

            _eventAggregator.GetEvent<IpConnectionRequestEvent>().Publish(ipSettings);
        }

        private bool IsAnIpType(ModbusType modbusType)
        {
            return modbusType == ModbusType.Tcp
                || modbusType == ModbusType.Udp
                || modbusType == ModbusType.RtuOverTcp
                || modbusType == ModbusType.RtuOverUdp;
        }
    }
}
