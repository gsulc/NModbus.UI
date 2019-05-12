using Prism.Events;

namespace NModbus.UI.Service
{
    public class NewModbusMasterEvent : PubSubEvent<IModbusMaster>
    {
    }

    public class NewModbusMasterIdEvent : PubSubEvent<string>
    {

    }
}
