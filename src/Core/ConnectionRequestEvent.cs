using Prism.Events;
using System;
using System.Collections.Generic;

namespace NModbus.UI.Common.Core
{
    public class DisconnectRequestEvent : PubSubEvent { }

    public class ConnectionRequestEvent : PubSubEvent { }

    public class ConnectionTypeRequestEvent : PubSubEvent<ModbusType> { }

    public class IpConnectionRequestEvent : PubSubEvent<IpSettings> { }

    public class SerialConnectionRequestEvent : PubSubEvent<SerialSettings> { }

#if DEBUG
    public class RandomConnectionRequestEvent : PubSubEvent { }
#endif

    // public class NewModbusMasterEvent : PubSubEvent<string> { }

    public class ReadCoilsRequestEvent : PubSubEvent<ModbusMultipleAddress> { }
    public class ReadDiscreteInputsRequestEvent : PubSubEvent<ModbusMultipleAddress> { }
    public class ReadInputRegistersRequestEvent : PubSubEvent<ModbusMultipleAddress> { }
    public class ReadHoldingRegisterRequestEvent : PubSubEvent<ModbusMultipleAddress> { }

    public class WriteCoilRequestEvent : PubSubEvent<CoilData> { }
    public class WriteHoldingRegisterRequest : PubSubEvent<HoldingRegisterData> { }

    public class CoilsReadEvent : PubSubEvent<CoilData> { }
    public class DiscreteInputEvent : PubSubEvent<DiscreteInputData> { }
    public class InputRegistersReadEvent : PubSubEvent<InputRegisterData> { }
    public class HoldingRegistersReadEvent : PubSubEvent<HoldingRegisterData> { }
}
