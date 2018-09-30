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
            //_eventAggregator.GetEvent<ConnectionRequestEvent>().Publish()
            ConnectCommand = new DelegateCommand<string>(ConnectionStateChangeRequest);
            UpdateConnectButtonText();
        }

        public DelegateCommand<string> ConnectCommand { get; private set; }

        public bool Connected { get; set; } = false;

        private string _connectedButtonText;
        public string ConnectButtonText
        {
            get { return _connectedButtonText; }
            set
            {
                _connectedButtonText = value;
                RaisePropertyChanged();
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
            _eventAggregator.GetEvent<ConnectionRequestEvent>().Publish();
        }
    }
}
