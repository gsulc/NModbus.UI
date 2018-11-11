using NModbus.UI.Common.Core;
using System;

namespace NModbus.UI.Service
{
    internal static class ModbusMasterExtensions
    {
        public static bool[] ReadInputs(this IModbusMaster master, ModbusMultipleAddress address)
        {
            return master.ReadInputs(address.SlaveId, address.StartAddress, address.Count);
        }

        public static bool[] ReadCoils(this IModbusMaster master, ModbusMultipleAddress address)
        {
            return master.ReadCoils(address.SlaveId, address.StartAddress, address.Count);
        }

        public static UInt16[] ReadInputRegisters(
            this IModbusMaster master,
            ModbusMultipleAddress address)
        {
            return master.ReadInputRegisters(address.SlaveId, address.StartAddress, address.Count);
        }

        public static UInt16[] ReadHoldingRegisters(
            this IModbusMaster master, 
            ModbusMultipleAddress address)
        {
            return master.ReadHoldingRegisters(address.SlaveId, address.StartAddress, address.Count);
        }
    }
}
