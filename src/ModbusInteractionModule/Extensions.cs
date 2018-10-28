using NModbus.UI.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NModbus.UI.InteractionModule
{
    public static class Extensions
    {
        public static object ReadSingleObject(
            this IModbusMaster master, ObjectType objectType, byte slaveId, ushort address)
        {
            switch (objectType)
            {
                case ObjectType.Coil:
                    return master.ReadCoils(slaveId, address, 1)[0];
                case ObjectType.DiscreteInput:
                    return master.ReadInputs(slaveId, address, 1)[0];
                case ObjectType.HoldingRegister:
                    return master.ReadHoldingRegisters(slaveId, address, 1)[0];
                case ObjectType.InputRegister:
                    return master.ReadInputRegisters(slaveId, address, 1)[0];
                default:
                    throw new ArgumentException("objectType");
            }
        }

        /// <summary>
        /// IEnumerable<[address,value]>
        /// </summary>
        public static IEnumerable<KeyValuePair<ushort, ushort>> ReadHoldingRegisters(
            this IModbusMaster master, byte slaveAddress, IEnumerable<ushort> addresses)
        {
            foreach (var bucket in FormHoldingRegisterBuckets(addresses))
            {
                ushort startAddress = bucket.Item1;
                ushort numberOfPoints = bucket.Item2;
                ushort[] values = master.ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints);
                foreach (var value in values)
                    yield return new KeyValuePair<ushort, ushort>(startAddress, value);
            }
        }

        const ushort MaxHoldingRegistersInOneRead = 125;
        const ushort MaxHoldingRegistersInOneWrite = 63;
        const ushort MaxCoilsInOneRead = 2000;
        private static IEnumerable<Tuple<ushort, ushort>> FormHoldingRegisterBuckets(
            IEnumerable<ushort> addresses)
        {
            return FormBuckets(addresses, MaxHoldingRegistersInOneRead);
        }

        // TODO: unit test this
        // [address,length]
        public static IEnumerable<Tuple<ushort, ushort>> FormBuckets(
            IEnumerable<ushort> addresses, ushort maxSingleRead)
        {
            IOrderedEnumerable<ushort> sortedAddresses = addresses.OrderBy(a => a);
            var buckets = new List<Tuple<ushort, ushort>>();
            IEnumerator<ushort> enumerator = sortedAddresses.GetEnumerator();

            enumerator.MoveNext();
            ushort startAddress = enumerator.Current;
            ushort length = 1;
            while (enumerator.MoveNext())
            {
                ushort address = enumerator.Current;
                bool inSequence = address == startAddress + length;
                bool atLengthLimit = length >= maxSingleRead;
                if (inSequence && !atLengthLimit)
                {
                    ++length;
                }
                else
                {
                    buckets.Add(new Tuple<ushort, ushort>(startAddress, length));
                    startAddress = address;
                    length = 1;
                }
            }
            buckets.Add(new Tuple<ushort, ushort>(startAddress, length));

            return buckets;
        }

    }
}
