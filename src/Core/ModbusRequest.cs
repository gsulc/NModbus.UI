using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.Common.Core
{
    public class ModbusReadRequestEvent : PubSubEvent<ModbusReadRequest>
    { }

    public class ModbusReadResponseEvent : PubSubEvent<ModbusReadResponse>
    { }

    public class ModbusReadRequest
    {
        public ObjectType ObjectType { get; set; }
        public string MasterId { get; set; }
        public byte SlaveId { get; set; }
        public ushort StartAddress { get; set; }
        public ushort NumberOfPoints { get; set; }
    }

    public class ModbusReadResponse
    {
        public ObjectType ObjectType { get; set; }
        public string MasterId { get; set; }
        public byte SlaveId { get; set; }
        public ushort StartAddress { get; set; }
        public IEnumerable<object> Data { get; set; }
    }
}
