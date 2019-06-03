using NModbus.UI.Common.Core;
using Prism.Mvvm;
using System;

namespace NModbus.UI.Models
{
    public class LineItem : BindableBase
    {
        private object _value;
        private object _converted;
        private NumericDisplayType _displayType = NumericDisplayType.Unsigned;

        public ObjectType ObjectType { get; set; }

        public ushort Address { get; set; }

        public object Value
        {
            get => _converted;
            set
            {
                SetProperty(ref _value, value);
                value = ConvertValue(value);
                SetProperty(ref _converted, value);
            }
        }

        public NumericDisplayType DisplayAs
        {
            get => _displayType;
            set
            {
                _displayType = value;
                RefreshValue();
            }
        }

        private object ConvertValue(object value)
        {
            if (value is bool)
                return value;

            switch(DisplayAs)
            {
                case NumericDisplayType.Signed:
                    return ShortConverter.ToShort((ushort)value);
                case NumericDisplayType.Hex:
                    return string.Format("{0:X}", value);
                case NumericDisplayType.Binary:
                    return Convert.ToString((ushort)value, 2).PadLeft(16, '0');
                case NumericDisplayType.Unsigned:
                default:
                    return value;
            }
        }

        private void RefreshValue()
        {
            Value = _value;
        }
    }
}
