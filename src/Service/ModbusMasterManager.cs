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
        private readonly IDictionary<string, SerialPort>
            _serialPorts = new Dictionary<string, SerialPort>();
        private readonly IEventAggregator _ea;
        private IModbusFactory _modbusFactory = new ModbusFactory();

        public ModbusMasterManager(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<IpConnectionRequestEvent>().Subscribe(NewIpConnection);
            _ea.GetEvent<SerialConnectionRequestEvent>().Subscribe(NewSerialConnection);
            _ea.GetEvent<DisconnectRequestEvent>().Subscribe(Disconnect);

            _ea.GetEvent<ReadDiscreteInputEvent>().Subscribe(ReadDiscreteInputs);
            _ea.GetEvent<ReadCoilRequestEvent>().Subscribe(ReadCoils);
            _ea.GetEvent<ReadInputRegisterEvent>().Subscribe(ReadInputRegisters);
            _ea.GetEvent<ReadHoldingRegisterEvent>().Subscribe(ReadHoldingRegisters);
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
                    CreateTcpMaster(ipSettings);
                    break;
                case ModbusType.Udp:
                    CreateTcpMaster(ipSettings);
                    break;
                default:
                    throw new ArgumentException("Ip settings must be either of type Tcp or Udp.");
            }
        }

        private void CreateTcpMaster(IpSettings ipSettings)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(ipSettings.Hostname, ipSettings.Port);
            var tcpMaster = _modbusFactory.CreateMaster(tcpClient);
            _masters.Add(ipSettings.Hostname, tcpMaster);
            _tcpClients.Add(ipSettings.Hostname, tcpClient);
        }

        private void CreateUdpMaster(IpSettings ipSettings)
        {
            var udpClient = new UdpClient();
            udpClient.Connect(ipSettings.Hostname, ipSettings.Port);
            var udpMaster = _modbusFactory.CreateMaster(udpClient);
            _masters.Add(ipSettings.Hostname, udpMaster);
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
            serialPort.Open();

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

        private void Disconnect()
        {
            foreach (var port in _serialPorts.Values)
                port.Close();
            foreach (var client in _tcpClients.Values)
                client.Close();
        }

        private void ReadDiscreteInputs(ModbusMultipleAddress address)
        {
            bool[] result = _masters[address.MasterName].ReadInputs(address);
            _ea.GetEvent<DiscreteInputEvent>().Publish(
                new DiscreteInputData() { Address = address, Values = result });
        }

        private void ReadCoils(ModbusMultipleAddress address)
        {
            bool[] result = _masters[address.MasterName].ReadCoils(address);
            _ea.GetEvent<CoilReadEvent>().Publish(
                new CoilData() { Address = address, Values = result });
        }

        private void ReadInputRegisters(ModbusMultipleAddress address)
        {
            UInt16[] result = _masters[address.MasterName].ReadInputRegisters(address);
            _ea.GetEvent<InputRegisterEvent>().Publish(
                new InputRegisterData() { Address = address, Values = result });
        }

        private void ReadHoldingRegisters(ModbusMultipleAddress address)
        {
            UInt16[] result = _masters[address.MasterName].ReadHoldingRegisters(address);
            _ea.GetEvent<HoldingRegisterEvent>().Publish(
                new HoldingRegisterData() { Address = address, Values = result });
        }
    }
}
