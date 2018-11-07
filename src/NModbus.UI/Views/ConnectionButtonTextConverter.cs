using System;
using System.Globalization;
using System.Windows.Data;

namespace NModbus.UI.Views
{
    class ConnectionButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool connected = (bool)value;
            return connected ? "Disconnect" : "Connect";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if (text == "Disconnect")
                return true;
            else
                return false;
        }
    }
}
