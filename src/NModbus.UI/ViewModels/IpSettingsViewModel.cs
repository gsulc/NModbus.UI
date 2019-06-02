using NModbus.UI.Common.Core;
using NModbus.UI.Properties;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NModbus.UI.ViewModels
{
    public class IpSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        IApplicationCommands _applicationCommands;
        IEventAggregator _eventAggregator;

        public IpSettingsViewModel(IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _applicationCommands = applicationCommands;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(SaveSettings);
            _applicationCommands.SaveCommand.RegisterCommand(SaveCommand);
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        public string Hostname { get; set; } = Settings.Default.Hostname;
        public int Port { get; set; } = Settings.Default.Port;

        public DelegateCommand SaveCommand { get; private set; }
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

        private void SaveSettings()
        {
            Settings.Default.Hostname = Hostname;
            Settings.Default.Port = Port;
            Settings.Default.Save();
        }
    }
}
