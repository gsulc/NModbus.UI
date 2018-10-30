using NModbus.UI.Common.Core;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.ViewModels
{
    public class RandomSettingsViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public RandomSettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        private void HandleConnectionRequest(ModbusType type)
        {
            if (type == ModbusType.Random)
                _eventAggregator.GetEvent<RandomConnectionRequestEvent>().Publish();
        }
    }
}
