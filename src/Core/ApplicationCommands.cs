using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.Common.Core
{
    public interface IApplicationCommands
    {
        CompositeCommand ConnectCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        //private CompositeCommand _connectCommand = new CompositeCommand();
        public CompositeCommand ConnectCommand { get; } = new CompositeCommand();
        //{
        //    get { return _connectCommand; }
        //}
    }
}
