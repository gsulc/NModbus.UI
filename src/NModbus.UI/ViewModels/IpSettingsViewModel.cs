using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.ViewModels
{
    public class IpSettingsViewModel : BindableBase
    {
        // listen for connection request, then send an ip connection request with setting information
        public string Hostname { get; set; }
        public int Port { get; set; }
    }
}
