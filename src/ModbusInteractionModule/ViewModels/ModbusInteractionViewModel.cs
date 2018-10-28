using NModbus.UI.Common.Core;
using NModbus.UI.Service;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections;
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
        byte _slaveId;
        ObservableCollection<LineItem> _lineItems = new ObservableCollection<LineItem>();

        public ModbusInteractionViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            ea.GetEvent<NewModbusMasterEvent>().Subscribe(NewModbusMaster);
            RemoveSelectedCommand = new DelegateCommand<IList>(RemoveSelectedItems);
            ReadSingleCommand = new DelegateCommand<LineItem>(ReadSingle);
            ReadCommand = new DelegateCommand(Read);
        }

        public byte SlaveId
        {
            get => _slaveId;
            set => SetProperty(ref _slaveId, value);
        }

        public ObservableCollection<LineItem> LineItems
        {
            get => _lineItems;
            set => SetProperty(ref _lineItems, value);
        }

        private LineItem _selectedItem;
        public LineItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public DelegateCommand<IList> RemoveSelectedCommand { get; private set; }
        public DelegateCommand<LineItem> ReadSingleCommand { get; private set; }
        public DelegateCommand<LineItem> WriteSingleCommand { get; private set; }
        public DelegateCommand ReadCommand { get; private set; }

        private void Read()
        {

        }

        private void RemoveSelectedItems(IList items)
        {
            object[] arr = new object[items.Count];
            items.CopyTo(arr, 0);
            foreach (var item in arr)
                LineItems.Remove(item as LineItem);
        }

        private void ReadSingle(LineItem item)
        {
            item.ValueAsString = _master.ReadSingleObject(
                item.ObjectType, SlaveId, item.Address).ToString();
        }

        private void NewModbusMaster(IModbusMaster master)
        {
            _master = master;
        }
    }
}
