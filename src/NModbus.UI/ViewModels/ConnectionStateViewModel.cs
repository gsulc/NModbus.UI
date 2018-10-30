using NModbus.UI.Common.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NModbus.UI.ViewModels
{
    public class ConnectionStateViewModel : BindableBase
    {
        readonly IEventAggregator _eventAggregator;

        public ConnectionStateViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            ConnectCommand = new DelegateCommand<string>(ConnectionStateChangeRequest);
        }

        public DelegateCommand<string> ConnectCommand { get; private set; }

        bool _connected = false;
        public bool Connected
        {
            get { return _connected; }
            set
            {
                _connected = value;
                ConnectButtonText = Connected ? "Disconnect" : "Connect";
            }
        }

        private string _connectedButtonText = "Connect";
        public string ConnectButtonText
        {
            get { return _connectedButtonText; }
            set { SetProperty(ref _connectedButtonText, value); }
        }

        private void ConnectionStateChangeRequest(string request)
        {
            bool connecting = !Connected;
            if (connecting)
                _eventAggregator.GetEvent<ConnectionRequestEvent>().Publish();
            else
                _eventAggregator.GetEvent<DisconnectRequestEvent>().Publish();
            Connected = connecting;
        }
    }
}
