namespace NModbus.UI.Common.Core
{
    public enum ObjectType
    {
        Coil,
        DiscreteInput,
        InputRegister,
        HoldingRegister
    }

    public class ModbusObject
    {
        ObjectType ObjectType { get; set; }
        object Value { get; set; }
    }

    //public class Coil : ModbusObject<bool>
    //{ }

    //public class DiscreteInput : ModbusObject<bool>
    //{ }

    //public class InputRegister : ModbusObject<ushort>
    //{ }

    //public class HoldingRegister : ModbusObject<ushort>
    //{ }
}
