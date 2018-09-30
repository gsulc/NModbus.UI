using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.Service
{
    public class NewModbusMasterEvent : PubSubEvent<IModbusMaster>
    {
    }
}
