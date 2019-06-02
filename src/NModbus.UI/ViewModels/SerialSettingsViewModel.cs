using NModbus.UI.Common.Core;
using NModbus.UI.Properties;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.IO.Ports;

namespace NModbus.UI.ViewModels
{
    public class SerialSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        IApplicationCommands _applicationCommands;
        IEventAggregator _eventAggregator;

        public SerialSettingsViewModel(IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _applicationCommands = applicationCommands;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(SaveSettings);
            _applicationCommands.SaveCommand.RegisterCommand(SaveCommand);
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        public IEnumerable<Parity> ParityOptions => Enums.GetValues<Parity>();
        public IEnumerable<StopBits> StopBitsOptions => Enums.GetValues<StopBits>();
        public IEnumerable<Handshake> HandshakeOptions => Enums.GetValues<Handshake>();

        public string PortName { get; set; } = Settings.Default.PortName;
        public int BaudRate { get; set; } = Settings.Default.BaudRate;
        public int DataBits { get; set; } = Settings.Default.DataBits;
        public Parity Parity { get; set; } = Settings.Default.Parity;
        public StopBits StopBits { get; set; } = Settings.Default.StopBits;
        public Handshake Handshake { get; set; } = Settings.Default.Handshake;

        public DelegateCommand SaveCommand { get; private set; }
        public bool KeepAlive => false;

        private void HandleConnectionRequest(ModbusType modbusType)
        {
            if (!(modbusType == ModbusType.Rtu || modbusType == ModbusType.Ascii))
                return;

            var serialSettings = new SerialSettings()
            {
                ModbusType = modbusType,
                PortName = PortName,
                BaudRate = BaudRate,
                DataBits = DataBits,
                Parity = Parity,
                StopBits = StopBits,
                Handshake = Handshake
            };

            _eventAggregator.GetEvent<ConnectionRequestEvent>().Publish(serialSettings);
        }

        private void SaveSettings()
        {
            Settings.Default.PortName = PortName;
            Settings.Default.BaudRate = BaudRate;
            Settings.Default.DataBits = DataBits;
            Settings.Default.Parity = Parity;
            Settings.Default.StopBits = StopBits;
            Settings.Default.Handshake = Handshake;
            Settings.Default.Save();
        }
    }
}
