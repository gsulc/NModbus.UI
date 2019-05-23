using NModbus.IO;
using NModbus.Serial;
using NModbus.UI.Common.Core;
using System;
using System.IO.Ports;
using System.Net.Sockets;

namespace NModbus.UI.Service
{
    public static class FactoryExtensions
    {
        public static IModbusMaster CreateIpMaster(this IModbusFactory factory, IpSettings ipSettings)
        {
            switch (ipSettings.ModbusType)
            {
                case ModbusType.Tcp:
                    return factory.CreateMaster(
                        new TcpClient(ipSettings.Hostname, ipSettings.Port));
                case ModbusType.Udp:
                    return factory.CreateMaster(
                        new UdpClient(ipSettings.Hostname, ipSettings.Port));
                case ModbusType.RtuOverTcp:
                    return factory.CreateRtuMaster(
                        new TcpClientAdapter(
                            new TcpClient(ipSettings.Hostname, ipSettings.Port)));
                case ModbusType.RtuOverUdp:
                    return factory.CreateRtuMaster(
                        new UdpClientAdapter(
                            new UdpClient(ipSettings.Hostname, ipSettings.Port)));
                default:
                    throw new ArgumentException(
                        "Ip settings must be of type Tcp, Udp, RtuOverTcp, or RtuOverUdp.");
            }
        }

        public static IModbusMaster CreateSerialMaster(this IModbusFactory factory, SerialSettings settings)
        {
            SerialPort serialPort = new SerialPort()
            {
                PortName = settings.PortName,
                BaudRate = settings.BaudRate,
                DataBits = settings.DataBits,
                Parity = settings.Parity,
                StopBits = settings.StopBits,
                Handshake = settings.Handshake
            };

            var adapter = new SerialPortAdapter(serialPort);

            serialPort.Open();

            switch (settings.ModbusType)
            {
                case ModbusType.Rtu:
                    return factory.CreateRtuMaster(adapter);
                case ModbusType.Ascii:
                    return factory.CreateAsciiMaster(adapter);
                default:
                    throw new ArgumentException("Serial Settings must be either of type Rtu or Ascii.");
            }
        }
    }
}
