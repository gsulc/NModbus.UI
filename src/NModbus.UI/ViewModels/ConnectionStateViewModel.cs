using Prism.Commands;
using Prism.Mvvm;

namespace NModbus.UI.ViewModels
{
    public class ConnectionStateViewModel : BindableBase
    {
        public ConnectionStateViewModel()
        {
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
        }
    }
}
