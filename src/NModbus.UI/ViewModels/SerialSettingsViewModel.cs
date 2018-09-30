using NModbus.UI.Common.Core;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.IO.Ports;

namespace NModbus.UI.ViewModels
{
    public class SerialSettingsViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        public SerialSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        public IEnumerable<Parity> ParityOptions => Enums.GetValues<Parity>();
        public IEnumerable<StopBits> StopBitsOptions => Enums.GetValues<StopBits>();
        public IEnumerable<Handshake> HandshakeOptions => Enums.GetValues<Handshake>();

        // TODO: load initial values from saved settings
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Handshake Handshake { get; set; } = Handshake.None;

        private void HandleConnectionRequest(ModbusType modbusType)
        {
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
            _eventAggregator.GetEvent<SerialConnectionRequestEvent>().Publish(serialSettings);
        }
    }
}
