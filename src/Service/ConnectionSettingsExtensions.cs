using NModbus.UI.Common.Core;

namespace NModbus.UI.Service
{
    public static class ConnectionSettingsExtensions
    {
        public static string Name(this ConnectionSettings connectionSettings)
        {
            if (connectionSettings is IpSettings ipSettings)
                return ipSettings.Hostname;
            else if (connectionSettings is SerialSettings serialSettings)
                return serialSettings.PortName;
            else
                return "";
        }
    }
}
