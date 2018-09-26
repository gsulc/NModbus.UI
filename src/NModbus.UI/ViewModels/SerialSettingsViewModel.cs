using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.ViewModels
{
    public class SerialSettingsViewModel : BindableBase
    {
        // listen for connection request, then send an serial connection request with setting information
        public IEnumerable<Parity> ParityOptions => Enums.GetValues<Parity>();
        public IEnumerable<StopBits> StopBitsOptions => Enums.GetValues<StopBits>();
        public IEnumerable<Handshake> HandshakeOptions => Enums.GetValues<Handshake>();

        // TODO: load initial values from saved settings
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBits { get; set; } = StopBits.None;
        public Handshake Handshake { get; set; } = Handshake.None;
    }
}
