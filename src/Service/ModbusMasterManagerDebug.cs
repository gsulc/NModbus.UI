#if DEBUG
using NModbus.UI.Common.Core;
using Prism.Events;

namespace NModbus.UI.Service
{
    public class ModbusMasterManagerDebug
    {
        private readonly IEventAggregator _eventAggregator;
        private IModbusMaster _master;

        public ModbusMasterManagerDebug(IEventAggregator ea)
        {
            _eventAggregator = ea;
            _eventAggregator.GetEvent<RandomConnectionRequestEvent>().Subscribe(NewRandomConnection);
        }

        private void NewRandomConnection()
        {
            _master = new RandomModbusMaster();
            _eventAggregator.GetEvent<NewModbusMasterEvent>().Publish(_master);
        }
    }
}
#endif
