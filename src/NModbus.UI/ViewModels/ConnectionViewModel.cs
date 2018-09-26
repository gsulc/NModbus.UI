using Microsoft.Practices.Unity;
using NModbus.UI.Common.Core;
using NModbus.UI.Views;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System.Collections.Generic;

namespace NModbus.UI.ViewModels
{
    public class ConnectionViewModel : BindableBase
    {
        IRegionManager _regionManager;
        IUnityContainer _container;
        ModbusType _modbusType;

        public ConnectionViewModel(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
            _regionManager.RegisterViewWithRegion("ConnectionStateRegion", typeof(ConnectionStateView));
            _container.RegisterTypeForNavigation<IpSettingsView>();
            _container.RegisterTypeForNavigation<SerialSettingsView>();
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
                    return "SerialSettingsView";
                case ModbusType.Tcp:
                case ModbusType.Udp:
                default:
                    return "IpSettingsView";
            }
        }
    }
    
}
