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
        ModbusType _modbusType;
        IEventAggregator _eventAggregator;

        public ConnectionViewModel(
            RegionManager regionManager, 
            IUnityContainer container, 
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<NewModbusMasterEvent>().Subscribe(OnConnectionConfirmed);
            _eventAggregator.GetEvent<DisconnectEvent>().Subscribe(OnDisconnected);
            RegisterNavigationTypes(container);
            ConnectionCommand = new DelegateCommand(ConnectionStateChange);
            SelectedModbusType = ModbusType.Tcp;
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

        private void RegisterNavigationTypes(IUnityContainer container)
        {
            container.RegisterTypeForNavigation<IpSettingsView>();
            container.RegisterTypeForNavigation<SerialSettingsView>();
#if DEBUG
            container.RegisterTypeForNavigation<RandomSettingsView>();
#endif
        }

        private void NavigateToRegion()
        {
            string viewName = GetSettingsViewName(SelectedModbusType);
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
                case ModbusType.RtuOverTcp:
                case ModbusType.RtuOverUdp:
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
            if (!IsConnected)
                _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Publish(SelectedModbusType);
            else // disconnecting
                _eventAggregator.GetEvent<DisconnectRequestEvent>().Publish();
        }

        private void OnConnectionConfirmed(string masterId)
        {
            IsConnected = true;
            IsEnabled = false;
        }

        private void OnDisconnected(string masterId)
        {
            IsConnected = false;
            IsEnabled = true;
        }
    }
    
}
