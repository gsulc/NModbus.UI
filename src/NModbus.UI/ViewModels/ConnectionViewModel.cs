using Microsoft.Practices.Unity;
using NModbus.UI.Common.Core;
using NModbus.UI.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;

namespace NModbus.UI.ViewModels
{
    public class ConnectionViewModel : BindableBase
    {
        IRegionManager _regionManager;
        IUnityContainer _container;
        ModbusType _modbusType;
        IEventAggregator _eventAggregator;

        public ConnectionViewModel(
            RegionManager regionManager, 
            IUnityContainer container, 
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _container = container;
            _eventAggregator = eventAggregator;
            _container.RegisterTypeForNavigation<IpSettingsView>();
            _container.RegisterTypeForNavigation<SerialSettingsView>();
#if DEBUG
            _container.RegisterTypeForNavigation<RandomSettingsView>();
#endif
            _eventAggregator.GetEvent<ConnectionRequestEvent>().Subscribe(HandleConnectionRequest);
            _eventAggregator.GetEvent<DisconnectRequestEvent>().Subscribe(HandleDisconnect);
            ConnectionCommand = new DelegateCommand(ConnectionStateChange);
        }

        public DelegateCommand ConnectionCommand { get; private set; }

        public IEnumerable<ModbusType> ModbusTypes => Enums.GetValues<ModbusType>();

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set { SetProperty(ref _isConnected, value); }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public ModbusType SelectedModbusType
        {
            get
            {
                return _modbusType;
            }
            set
            {
                _modbusType = value;
                NavigateToRegion();
            }
        }

        private void NavigateToRegion()
        {
            string viewName = GetSettingsViewName(_modbusType);
            _regionManager.RequestNavigate("ConnectionSettingsRegion", viewName);
        }

        private string GetSettingsViewName(ModbusType modbusType)
        {
            switch (modbusType)
            {
                case ModbusType.Rtu:
                case ModbusType.Ascii:
                    return nameof(SerialSettingsView);
                case ModbusType.Tcp:
                case ModbusType.Udp:
                    return nameof(IpSettingsView);
#if DEBUG
                case ModbusType.Random:
                    return nameof(RandomSettingsView);
#endif
                default:
                    throw new ArgumentException("modbusType");
            }
        }

        private void ConnectionStateChange()
        {
            bool connecting = !IsConnected;
            if (connecting)
            {
                _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Publish(SelectedModbusType);
            }
            else // disconnecting
            {

            }
            IsConnected = connecting;
            IsEnabled = !IsEnabled;
        }

        private void HandleConnectionRequest()
        {
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Publish(SelectedModbusType);
            IsEnabled = false;
        }

        private void HandleDisconnect()
        {
            IsEnabled = true;
        }
    }
    
}
