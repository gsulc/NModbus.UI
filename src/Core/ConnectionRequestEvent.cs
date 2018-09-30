using Prism.Events;

namespace NModbus.UI.Common.Core
{
    public class ConnectionRequestEvent : PubSubEvent { }

    public class ConnectionTypeRequestEvent : PubSubEvent<ModbusType> { }

    public class IpConnectionRequestEvent : PubSubEvent<IpSettings> { }

    public class SerialConnectionRequestEvent : PubSubEvent<SerialSettings> { }
}
