using Microsoft.Practices.Unity;
using NModbus.UI.Common.Core;
using NModbus.UI.Views;
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
            _regionManager.RegisterViewWithRegion("ConnectionStateRegion", typeof(ConnectionStateView));
            _container.RegisterTypeForNavigation<IpSettingsView>();
            _container.RegisterTypeForNavigation<SerialSettingsView>();
#if DEBUG
            _container.RegisterTypeForNavigation<RandomSettingsView>();
#endif
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ConnectionRequestEvent>().Subscribe(HandleConnectionRequest);
        }

        public IEnumerable<ModbusType> ModbusTypes => Enums.GetValues<ModbusType>();

        public ModbusType ModbusType
        {
            get
            {
                return _modbusType;
            }
            set
            {
                _modbusType = value;
                string viewName = GetSettingsViewName(_modbusType);
                _regionManager.RequestNavigate("ConnectionSettingsRegion", viewName);
            }
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

        private void HandleConnectionRequest()
        {
            _eventAggregator.GetEvent<ConnectionTypeRequestEvent>().Publish(ModbusType);
        }
    }
    
}
