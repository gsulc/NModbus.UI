using NModbus.UI.Common.Core;
using Prism.Mvvm;

namespace NModbus.UI.InteractionModule
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

    public abstract class LineItem<T>
    {
        public T Value { get; set; }
    }

    public class BoolLineItem : LineItem<bool>
    {
    }

    public class NumericLineItem : LineItem<ushort>
    {
        public short SignedValue { get { return (short)Value; } }
        public string HexValue { get { return string.Format("{0:X}", Value); } }
    }
}
