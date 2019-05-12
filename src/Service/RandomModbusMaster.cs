using System;
using System.Threading.Tasks;

namespace NModbus.UI.Service
{
    public class RandomModbusMaster : IModbusMaster, IDisposable
    {
        private readonly Random _random = new Random();

        public IModbusTransport Transport => throw new NotImplementedException();

        public TResponse ExecuteCustomMessage<TResponse>(IModbusMessage request) where TResponse : IModbusMessage, new()
        {
            throw new NotImplementedException();
        }

        private bool[] GenerateRandomBools(ushort numberOfPoints)
        {
            bool[] values = new bool[numberOfPoints];
            for (int i = 0; i < numberOfPoints; ++i)
                values[i] = _random.Next(0, 2) > 0;
            return values;
        }

        private ushort[] GenerateRandomUInt16s(ushort numberOfPoints)
        {
            ushort[] values = new ushort[numberOfPoints];
            for (int i = 0; i < numberOfPoints; ++i)
                values[i] = (ushort)_random.Next(ushort.MinValue, ushort.MaxValue);
            return values;
        }

        public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return GenerateRandomBools(numberOfPoints);
        }

        public Task<bool[]> ReadCoilsAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return Task.Factory.StartNew(() => ReadCoils(slaveAddress, startAddress, numberOfPoints));
        }

        public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return GenerateRandomUInt16s(numberOfPoints);
        }

        public Task<ushort[]> ReadHoldingRegistersAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return Task.Factory.StartNew(() => ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints));
        }

        public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return GenerateRandomUInt16s(numberOfPoints);
        }

        public Task<ushort[]> ReadInputRegistersAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return Task.Factory.StartNew(() => ReadInputRegisters(slaveAddress, startAddress, numberOfPoints));
        }

        public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return GenerateRandomBools(numberOfPoints);
        }

        public Task<bool[]> ReadInputsAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        {
            return Task.Factory.StartNew(() => ReadInputs(slaveAddress, startAddress, numberOfPoints));
        }

        public ushort[] ReadWriteMultipleRegisters(byte slaveAddress, ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData)
        {
            throw new NotImplementedException();
        }

        public Task<ushort[]> ReadWriteMultipleRegistersAsync(byte slaveAddress, ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData)
        {
            throw new NotImplementedException();
        }

        public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
        {
            throw new NotImplementedException();
        }

        public Task WriteMultipleCoilsAsync(byte slaveAddress, ushort startAddress, bool[] data)
        {
            throw new NotImplementedException();
        }

        public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
        {
            throw new NotImplementedException();
        }

        public Task WriteMultipleRegistersAsync(byte slaveAddress, ushort startAddress, ushort[] data)
        {
            throw new NotImplementedException();
        }

        public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
        {
            throw new NotImplementedException();
        }

        public Task WriteSingleCoilAsync(byte slaveAddress, ushort coilAddress, bool value)
        {
            throw new NotImplementedException();
        }

        public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
        {
            throw new NotImplementedException();
        }

        public Task WriteSingleRegisterAsync(byte slaveAddress, ushort registerAddress, ushort value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }

    public static class RandomModbusConstructor
    {
        public static IModbusMaster CreateRandomMaster(this ModbusFactory factory)
        {
            return new RandomModbusMaster();
        }
    }
}
