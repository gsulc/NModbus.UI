using NModbus.UI.Common.Core;
using NModbus.UI.Service;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.InteractionModule.ViewModels
{
    public class ModbusInteractionViewModel : BindableBase //, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        IModbusMaster _master;
        ObservableCollection<LineItem> _lineItems = new ObservableCollection<LineItem>();

        public ModbusInteractionViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            ea.GetEvent<NewModbusMasterEvent>().Subscribe(NewModbusMaster);
            //_lineItems.Add(new LineItem());
            //LineItems = _lineItems;
            LineItems.Add(new LineItem());
        }

        public ObservableCollection<LineItem> LineItems
        {
            get => _lineItems;
            set => SetProperty(ref _lineItems, value);
        }

        private void NewModbusMaster(IModbusMaster master)
        {
            _master = master;
        }

        
    }
}
