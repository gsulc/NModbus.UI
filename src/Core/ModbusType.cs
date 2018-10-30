namespace NModbus.UI.Common.Core
{
    public enum ModbusType
    {
        Tcp,
        Udp,
        Rtu,
        Ascii,
#if DEBUG
        Random,
#endif
    }
}
