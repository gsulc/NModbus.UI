using NModbus.UI.Common.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NModbus.Serial;
using System.IO.Ports;
using Prism.Events;

namespace NModbus.UI.Service
{
    public class ModbusMasterManager : IDisposable
    {
        private readonly IDictionary<string, IModbusMaster> 
            _masters = new Dictionary<string, IModbusMaster>();
        private readonly IDictionary<string, TcpClient> 
            _tcpClients = new Dictionary<string, TcpClient>();
        private readonly IDictionary<string, UdpClient>
            _updClients = new Dictionary<string, UdpClient>();
        private readonly IDictionary<string, SerialPort>
            _serialPorts = new Dictionary<string, SerialPort>();

        public ModbusMasterManager(IEventAggregator ea)
        {
            ea.GetEvent<IpConnectionRequestEvent>().Subscribe(NewIpConnection);
            ea.GetEvent<SerialConnectionRequestEvent>().Subscribe(NewSerialConnection);
        }

        public IEnumerable<IModbusMaster> ModbusMasters => _masters.Values;

        public void Dispose()
        {
            foreach (var port in _serialPorts.Values)
                port.Dispose();
            foreach (var client in _tcpClients.Values)
                client.Close();
        }

        private void NewIpConnection(IpSettings ipSettings)
        {
            var factory = new ModbusFactory();

            switch (ipSettings.ModbusType)
            {
                case ModbusType.Tcp:
                    var tcpClient = new TcpClient();
#if !DEBUG
                    tcpClient.Connect(ipSettings.Hostname, ipSettings.Port);
#endif
                    var tcpMaster = factory.CreateMaster(tcpClient);
                    _masters.Add(ipSettings.Hostname, tcpMaster);
                    _tcpClients.Add(ipSettings.Hostname, tcpClient);
                    break;
                case ModbusType.Udp:
                    var udpClient = new UdpClient();
#if !DEBUG
                    udpClient.Connect(ipSettings.Hostname, ipSettings.Port);
#endif
                    var udpMaster = factory.CreateMaster(udpClient);
                    _masters.Add(ipSettings.Hostname, udpMaster);
                    break;
                default:
                    throw new ArgumentException("Ip settings must be either of type Tcp or Udp.");
            }
        }

        private void NewSerialConnection(SerialSettings settings)
        {
            var factory = new ModbusFactory();
            SerialPort serialPort = new SerialPort()
            {
                PortName = settings.PortName,
                BaudRate = settings.BaudRate,
                DataBits = settings.DataBits,
                Parity = settings.Parity,
                StopBits = settings.StopBits,
                Handshake = settings.Handshake
            };
            
            var adapter = new SerialPortAdapter(serialPort);
#if !DEBUG
            serialPort.Open();
#endif
            switch (settings.ModbusType)
            {
                case ModbusType.Rtu:
                    var rtuMaster = factory.CreateRtuMaster(adapter);
                    _masters.Add(settings.PortName, rtuMaster);
                    break;
                case ModbusType.Ascii:
                    var asciiMaster = factory.CreateAsciiMaster(adapter);
                    _masters.Add(settings.PortName, asciiMaster);
                    break;
                default:
                    throw new ArgumentException("Serial Settings must be either of type Rtu or Ascii.");
            }
        }
    }
}
