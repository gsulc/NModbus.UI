namespace NModbus.UI.Common.Core
{
    public enum ModbusType
    {
        Tcp,
        Udp,
        Rtu,
        Ascii,
        RtuOverTcp,
        RtuOverUdp,
#if DEBUG
        Random,
#endif
    }
}
