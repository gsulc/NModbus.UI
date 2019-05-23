using NModbus.UI.Common.Core;
using System;
using System.Collections.Generic;
using Prism.Events;
using System.Linq;

namespace NModbus.UI.Service
{
    public class ModbusMasterManager : IDisposable
    {
        private readonly IDictionary<string, IModbusMaster> 
            _masters = new Dictionary<string, IModbusMaster>();
        private readonly IEventAggregator _ea;
        private readonly IModbusFactory _modbusFactory = new ModbusFactory();

        public ModbusMasterManager(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<ConnectionRequestEvent>().Subscribe(NewConnection);
            _ea.GetEvent<DisconnectRequestEvent>().Subscribe(Disconnect);
            _ea.GetEvent<ModbusReadRequestEvent>().Subscribe(ReadObjects);

#if DEBUG
            _ea.GetEvent<RandomConnectionRequestEvent>().Subscribe(NewRandomConnection);
#endif
        }

        public IEnumerable<IModbusMaster> ModbusMasters => _masters.Values;

        private void NewConnection(ConnectionSettings connectionSettings)
        {
            try
            {
                if (connectionSettings is IpSettings)
                    CreateIpModbusMaster(connectionSettings as IpSettings);
                else if (connectionSettings is SerialSettings)
                    CreateSerialMaster(connectionSettings as SerialSettings);
                else
                    throw new ArgumentException("connectionSettings");
            }
            catch (Exception e)
            {
                PublishException(e);
            }
        }

        private void CreateIpModbusMaster(IpSettings ipSettings)
        {
            var master = _modbusFactory.CreateIpMaster(ipSettings);
            string masterId = ipSettings.Hostname;
            AddMaster(masterId, master);
            _ea.GetEvent<NewModbusMasterEvent>().Publish(masterId);
        }

        private void CreateSerialMaster(SerialSettings settings)
        {
            var master = _modbusFactory.CreateSerialMaster(settings);
            string masterId = settings.PortName;
            AddMaster(masterId, master);
            _ea.GetEvent<NewModbusMasterEvent>().Publish(masterId);
        }

#if DEBUG
        private void NewRandomConnection()
        {
            string masterId = Guid.NewGuid().ToString();
            AddMaster(masterId, new RandomModbusMaster());
            _ea.GetEvent<NewModbusMasterEvent>().Publish(masterId);
        }
#endif

        private void Disconnect()
        {
            foreach (var masterId in _masters.Keys.ToArray())
                RemoveMaster(masterId);
        }

        private void AddMaster(string masterId, IModbusMaster master)
        {
            if (!_masters.ContainsKey(masterId))
                _masters.Add(masterId, master);
        }

        private void RemoveMaster(string masterId)
        {
            var master = _masters[masterId];
            _masters.Remove(masterId);
            master.Dispose();
            master = null;
            _ea.GetEvent<DisconnectEvent>().Publish(masterId);
        }

        private void ReadObjects(ModbusReadRequest request)
        {
            try
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
            catch (Exception e)
            {
                PublishException(e);
            }
        }

        private void ReadDiscreteInputs(ModbusReadRequest request)
        {
            bool[] result = _masters[request.MasterId].ReadInputs(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            PublishResponse(CreateResponse(request, result));
        }

        private void ReadCoils(ModbusReadRequest request)
        {
            bool[] result = _masters[request.MasterId].ReadCoils(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            PublishResponse(CreateResponse(request, result));
        }

        private void ReadInputRegisters(ModbusReadRequest request)
        {
            UInt16[] result = _masters[request.MasterId].ReadInputRegisters(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            PublishResponse(CreateResponse(request, result));
        }

        private void ReadHoldingRegisters(ModbusReadRequest request)
        {
            UInt16[] result = _masters[request.MasterId].ReadHoldingRegisters(
                request.SlaveId, request.StartAddress, request.NumberOfPoints);
            PublishResponse(CreateResponse(request, result));
        }

        private void PublishResponse(ModbusReadResponse response)
        {
            _ea.GetEvent<ModbusReadResponseEvent>().Publish(response);
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

        private void PublishException(Exception e)
        {
            _ea.GetEvent<ExceptionEvent>().Publish(e);
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
