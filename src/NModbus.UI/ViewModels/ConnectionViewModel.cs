using Microsoft.Practices.Unity;
using NModbus.UI.Common.Core;
using NModbus.UI.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _container.RegisterTypeForNavigation<IpSettingsView>();
            _container.RegisterTypeForNavigation<SerialSettingsView>();
            ConnectCommand = new DelegateCommand<string>(ConnectionStateChangeRequest);
            UpdateConnectButtonText();
        }

        public DelegateCommand<string> ConnectCommand { get; private set; }

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

        public bool Connected { get; set; } = false;

        private string _connectedButtonText;
        public string ConnectButtonText
        {
            get { return _connectedButtonText; }
            set { _connectedButtonText = value; RaisePropertyChanged(); }
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

        private void UpdateConnectButtonText()
        {
            ConnectButtonText = Connected ? "Disconnect" : "Connect";
        }

        private void ConnectionStateChangeRequest(string request)
        {
            Connected = !Connected;
            UpdateConnectButtonText();
        }
    }
    
}
