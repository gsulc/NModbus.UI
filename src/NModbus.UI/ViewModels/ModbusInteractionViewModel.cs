using NModbus.UI.Common.Core;
using NModbus.UI.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NModbus.UI.ViewModels
{
    public class ModbusInteractionViewModel : BindableBase //, INavigationAware
    {
        private IEventAggregator _ea;
        byte _slaveId;
        ObservableCollection<LineItem> _lineItems = new ObservableCollection<LineItem>();
        bool _isEnabled = false;
        string _masterId;

        public ModbusInteractionViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<NewModbusMasterEvent>().Subscribe(NewModbusMaster);
            _ea.GetEvent<DisconnectRequestEvent>().Subscribe(Disconnect);
            _ea.GetEvent<ModbusReadResponseEvent>().Subscribe(OnReadResponse);
            AddToListCommand = new DelegateCommand(AddToList);
            RemoveSelectedCommand = new DelegateCommand<IList>(RemoveSelectedItems);
            RemoveAllCommand = new DelegateCommand(RemoveAll);
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

        public IEnumerable<ObjectType> ObjectTypes => Enums.GetValues<ObjectType>();

        public ObjectType ObjectType { get; set; }

        public ushort StartAddress { get; set; } = 0;

        public ushort NumberOfItems { get; set; } = 0;

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

        public DelegateCommand AddToListCommand { get; private set; }
        public DelegateCommand<IList> RemoveSelectedCommand { get; private set; }
        public DelegateCommand RemoveAllCommand { get; set; }
        public DelegateCommand<LineItem> ReadSingleCommand { get; private set; }
        public DelegateCommand<LineItem> WriteSingleCommand { get; private set; }
        public DelegateCommand ReadCommand { get; private set; }

        private void Read()
        {
            foreach (var item in LineItems)
            {
                var request = new ModbusReadRequest()
                {
                    MasterId = _masterId,
                    ObjectType = item.ObjectType,
                    SlaveId = SlaveId,
                    StartAddress = item.Address,
                    NumberOfPoints = 1
                };
                _ea.GetEvent<ModbusReadRequestEvent>().Publish(request);
            }
        }

        private void OnReadResponse(ModbusReadResponse response)
        {
            var item = LineItems.First(i => i.Address == response.StartAddress);
            item.ValueAsString = response.Data[0].ToString();
        }

        private void RemoveSelectedItems(IList items)
        {
            object[] arr = new object[items.Count];
            items.CopyTo(arr, 0);
            foreach (var item in arr)
                LineItems.Remove(item as LineItem);
        }

        private void RemoveAll()
        {
            LineItems.Clear();
        }

        private void ReadSingle(LineItem item)
        {
            var request = new ModbusReadRequest()
            {
                MasterId = _masterId,
                ObjectType = item.ObjectType,
                SlaveId = SlaveId,
                StartAddress = item.Address,
                NumberOfPoints = 1
            };

            _ea.GetEvent<ModbusReadRequestEvent>().Publish(request);
        }

        private void NewModbusMaster(string masterId)
        {
            _masterId = masterId;
            IsEnabled = true;
        }

        private void Disconnect()
        {
            IsEnabled = false;
        }

        private void AddToList()
        {
            ushort endAddress = (ushort)(StartAddress + NumberOfItems - 1);
            for (ushort address = StartAddress; address <= endAddress; ++address)
            {
                if (!LineItems.Any(l => l.Address == address && l.ObjectType == ObjectType))
                {
                    LineItems.Add(new LineItem()
                    {
                        ObjectType = ObjectType,
                        Address = address
                    });
                }
            }
        }
    }
}
