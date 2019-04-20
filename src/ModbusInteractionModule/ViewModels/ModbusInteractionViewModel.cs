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
        private IEventAggregator _ea;
        IModbusMaster _master;
        byte _slaveId;
        ObservableCollection<LineItem> _lineItems = new ObservableCollection<LineItem>();
        bool _isEnabled = false;

        public ModbusInteractionViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<NewModbusMasterEvent>().Subscribe(NewModbusMaster);
            _ea.GetEvent<DisconnectRequestEvent>().Subscribe(Disconnect);
            //_ea.GetEvent<CoilsReadEvent>().Subscribe();
            RemoveSelectedCommand = new DelegateCommand<IList>(RemoveSelectedItems);
            ReadSingleCommand = new DelegateCommand<LineItem>(ReadSingle);
            ReadCommand = new DelegateCommand(Read);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            private set => SetProperty(ref _isEnabled, value);
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
            ReadCoils();
            ReadInputs();
            ReadHoldingRegisters();
            ReadInputRegisters();
        }

        private void ReadCoils() => ReadObjects(ObjectType.Coil, _master.ReadCoils);

        private void ReadInputs() => ReadObjects(ObjectType.DiscreteInput, _master.ReadInputs);

        private void ReadHoldingRegisters() => 
            ReadObjects(ObjectType.HoldingRegister, _master.ReadHoldingRegisters);

        private void ReadInputRegisters() =>
            ReadObjects(ObjectType.InputRegister, _master.ReadInputRegisters);

        private delegate IEnumerable<KeyValuePair<ushort, T>> ReadObjectsDelegate<T>(
            byte slaveId, IEnumerable<ushort> addresses);

        private void ReadObjects<T>(ObjectType type, ReadObjectsDelegate<T> readObjects)
        {
            var items = LineItems.Where(i => i.ObjectType == type);
            if (!items.Any())
                return;
            var addresses = items.Select(i => i.Address);
            var values = readObjects(SlaveId, addresses);
            foreach (var value in values)
            {
                var item = items.Where(i => i.Address == value.Key).First();
                var address = new ModbusMultipleAddress()
                {
                    MasterName = "",
                    SlaveId = SlaveId,
                    StartAddress = item.Address,
                    Count = 1
                };
                _ea.GetEvent<ReadCoilsRequestEvent>().Publish(address);
                item.ValueAsString = value.Value.ToString();
            }
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
            IsEnabled = true;
        }

        private void Disconnect()
        {
            _master.Dispose();
            _master = null;
            IsEnabled = false;
        }
    }
}
