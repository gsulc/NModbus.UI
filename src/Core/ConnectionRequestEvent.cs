using Prism.Events;
using System;
using System.Collections.Generic;

namespace NModbus.UI.Common.Core
{
    public class ConnectionRequestEvent : PubSubEvent { }

    public class ConnectionTypeRequestEvent : PubSubEvent<ModbusType> { }

    public class IpConnectionRequestEvent : PubSubEvent<IpSettings> { }

    public class SerialConnectionRequestEvent : PubSubEvent<SerialSettings> { }

   // public class NewModbusMasterEvent : PubSubEvent<string> { }

    public class ReadCoilRequestEvent : PubSubEvent<ModbusMultipleAddress> { }
    public class ReadDiscreteInputEvent : PubSubEvent<ModbusMultipleAddress> { }
    public class ReadInputRegisterEvent : PubSubEvent<ModbusMultipleAddress> { }
    public class ReadHoldingRegisterEvent : PubSubEvent<ModbusMultipleAddress> { }

    public class WriteCoilRequestEvent : PubSubEvent<CoilData> { }
    public class WriteHoldingRegisterRequest : PubSubEvent<HoldingRegisterData> { }

    public class CoilReadEvent : PubSubEvent<CoilData> { }
    public class DiscreteInputEvent : PubSubEvent<DiscreteInputData> { }
    public class InputRegisterEvent : PubSubEvent<InputRegisterData> { }
    public class HoldingRegisterEvent : PubSubEvent<HoldingRegisterData> { }
}
