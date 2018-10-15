using System;

namespace NModbus.UI.Common.Core
{
    public class ModbusAddress
    {
        public string MasterName { get; set; }
        public byte SlaveId { get; set; }
        public UInt16 Address { get; set; }
    }

    public class ModbusMultipleAddress
    {
        public string MasterName { get; set; }
        public byte SlaveId { get; set; }
        public UInt16 StartAddress { get; set; } 
        public UInt16 Count { get; set; }
    }

    public abstract class ModbusData<T>
    {
        public ModbusMultipleAddress Address { get; set; }
        public T[] Values { get; set; }

        //public ModbusData(ModbusAddress address, T[] values)
        //{
        //    ModbusAddress = address;
        //    Values = values;
        //}
    }

    public class CoilData : ModbusData<bool>
    {
        //public CoilData(ModbusAddress address, bool[] values) : base(address, values) { }
    }
    public class DiscreteInputData : ModbusData<bool>
    {
        //public DiscreteInputData(ModbusAddress address, bool[] values) : base(address, values) { }
    }
    public class InputRegisterData : ModbusData<UInt16>
    {
        //public InputRegisterData(ModbusAddress address, UInt16[] values) : base(address, values) { }
    }
    public class HoldingRegisterData : ModbusData<UInt16> { }
}
