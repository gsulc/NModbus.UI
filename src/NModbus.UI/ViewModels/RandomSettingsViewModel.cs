using NModbus.UI.Common.Core;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace NModbus.UI.ViewModels
{
    public class RandomSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        IEventAggregator _eventAggregator;

        public RandomSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        public bool KeepAlive => false;

        private void HandleConnectionRequest(ModbusType type)
        {
            if (type == ModbusType.Random)
                _eventAggregator.GetEvent<RandomConnectionRequestEvent>().Publish();
        }
    }
}
