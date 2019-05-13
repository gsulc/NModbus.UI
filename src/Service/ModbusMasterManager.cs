using NModbus.UI.Common.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NModbus.Serial;
using System.IO.Ports;
using Prism.Events;
using System.Linq;

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
            _ea.GetEvent<ModbusReadRequestEvent>().Subscribe(ReadObjects);

#if DEBUG
            _ea.GetEvent<RandomConnectionRequestEvent>().Subscribe(NewRandomConnection);
#endif
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

#if DEBUG
        private void NewRandomConnection()
        {
            string masterId = Guid.NewGuid().ToString();
            _masters.Add(masterId, new RandomModbusMaster());
            _ea.GetEvent<NewModbusMasterEvent>().Publish(masterId);
        }
#endif

        private void Disconnect()
        {
            foreach (var port in _serialPorts.Values)
                port.Close();
            foreach (var client in _tcpClients.Values)
                client.Close();
        }

        private void ReadObjects(ModbusReadRequest request)
        {
            switch (request.ObjectType)
            {
                case ObjectType.Coil:
                    ReadCoils(request);
                    break;
                case ObjectType.DiscreteInput:
                    ReadDiscreteInputs(request);
                    break;
                case ObjectType.InputRegister:
                    ReadInputRegisters(request);
                    break;
                case ObjectType.HoldingRegister:
                    ReadHoldingRegisters(request);
                    break;
                default:
                    throw new ArgumentException("request");
            }
        }

        private void ReadDiscreteInputs(ModbusReadRequest request)
        {
            bool[] result = _masters[request.MasterId].ReadInputs(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            _ea.GetEvent<ModbusReadResponseEvent>().Publish(CreateResponse(request, result));
        }

        private void ReadCoils(ModbusReadRequest request)
        {
            bool[] result = _masters[request.MasterId].ReadCoils(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            _ea.GetEvent<ModbusReadResponseEvent>().Publish(CreateResponse(request, result));
        }

        private void ReadInputRegisters(ModbusReadRequest request)
        {
            UInt16[] result = _masters[request.MasterId].ReadInputRegisters(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            _ea.GetEvent<ModbusReadResponseEvent>().Publish(CreateResponse(request, result));
        }

        private void ReadHoldingRegisters(ModbusReadRequest request)
        {
            UInt16[] result = _masters[request.MasterId].ReadHoldingRegisters(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            _ea.GetEvent<ModbusReadResponseEvent>().Publish(CreateResponse(request, result));
        }

        private ModbusReadResponse CreateResponse(ModbusReadRequest request, dynamic data)
        {
            return new ModbusReadResponse()
            {
                ObjectType = request.ObjectType,
                MasterId = request.MasterId,
                SlaveId = request.SlaveId,
                StartAddress = request.StartAddress,
                Data = data
            };
        }
    }
}
