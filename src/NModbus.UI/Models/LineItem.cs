using NModbus.UI.Common.Core;
using Prism.Mvvm;

namespace NModbus.UI.Models
{
    public class LineItem : BindableBase
    {
        private string _valueAsString;

        public ObjectType ObjectType { get; set; }
        public ushort Address { get; set; }
        public string ValueAsString
        {
            get { return _valueAsString; }
            set { SetProperty(ref _valueAsString, value); }
        }
    }
}
