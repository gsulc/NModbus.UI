using Prism.Events;

namespace NModbus.UI.Common.Core
{
    public class DisconnectRequestEvent : PubSubEvent { }

    public class DisconnectEvent : PubSubEvent<string> { }

    public class ConnectionTypeRequestEvent : PubSubEvent<ModbusType> { }

    public class ConnectionRequestEvent : PubSubEvent<ConnectionSettings> { }

#if DEBUG
    public class RandomConnectionRequestEvent : PubSubEvent { }
#endif

    public class NewModbusMasterEvent : PubSubEvent<string> { }
}
