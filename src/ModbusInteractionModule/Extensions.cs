using NModbus.UI.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public static IEnumerable<KeyValuePair<ushort, bool>> ReadCoils(
            this IModbusMaster master, byte slaveAddress, IEnumerable<ushort> addresses)
        {
            foreach (var bucket in GetCoilOrInputBucketsToRead(addresses))
            {
                ushort startAddress = bucket.Item1;
                ushort numberOfPoints = bucket.Item2;
                bool[] values = master.ReadCoils(slaveAddress, startAddress, numberOfPoints);
                ushort i = 0;
                foreach (var value in values)
                {
                    ushort address = (ushort)(startAddress + i);
                    ++i;
                    yield return new KeyValuePair<ushort, bool>(address, value);
                }
            }
        }

        /// <summary>
        /// IEnumerable<[address,value]>
        /// </summary>
        public static IEnumerable<KeyValuePair<ushort, bool>> ReadInputs(
            this IModbusMaster master, byte slaveAddress, IEnumerable<ushort> addresses)
        {
            foreach (var bucket in GetCoilOrInputBucketsToRead(addresses))
            {
                ushort startAddress = bucket.Item1;
                ushort numberOfPoints = bucket.Item2;
                bool[] values = master.ReadInputs(slaveAddress, startAddress, numberOfPoints);
                ushort i = 0;
                foreach (var value in values)
                {
                    ushort address = (ushort)(startAddress + i);
                    ++i;
                    yield return new KeyValuePair<ushort, bool>(address, value);
                }
            }
        }

        /// <summary>
        /// IEnumerable<[address,value]>
        /// </summary>
        public static IEnumerable<KeyValuePair<ushort, ushort>> ReadHoldingRegisters(
            this IModbusMaster master, byte slaveAddress, IEnumerable<ushort> addresses)
        {
            foreach (var bucket in GetRegisterBucketsToRead(addresses))
            {
                ushort startAddress = bucket.Item1;
                ushort numberOfPoints = bucket.Item2;
                ushort[] values = master.ReadHoldingRegisters(slaveAddress, startAddress, numberOfPoints);
                ushort i = 0;
                foreach (var value in values)
                {
                    ushort address = (ushort)(startAddress + i);
                    ++i;
                    yield return new KeyValuePair<ushort, ushort>(address, value);
                }
            }
        }

        /// <summary>
        /// IEnumerable<[address,value]>
        /// </summary>
        public static IEnumerable<KeyValuePair<ushort, ushort>> ReadInputRegisters(
            this IModbusMaster master, byte slaveAddress, IEnumerable<ushort> addresses)
        {
            foreach (var bucket in GetRegisterBucketsToRead(addresses))
            {
                ushort startAddress = bucket.Item1;
                ushort numberOfPoints = bucket.Item2;
                ushort[] values = master.ReadInputRegisters(slaveAddress, startAddress, numberOfPoints);
                ushort i = 0;
                foreach (var value in values)
                {
                    ushort address = (ushort)(startAddress + i);
                    ++i;
                    yield return new KeyValuePair<ushort, ushort>(address, value);
                }
            }
        }

        const ushort MaxRegistersPerRead = 125;
        const ushort MaxRegistersPerWrite = 63;
        private static IEnumerable<Tuple<ushort, ushort>> GetRegisterBucketsToRead(
            IEnumerable<ushort> addresses)
        {
            return FormBuckets(addresses, MaxRegistersPerRead);
        }

        const ushort MaxDiscretesPerRead = 2000;
        private static IEnumerable<Tuple<ushort, ushort>> GetCoilOrInputBucketsToRead(
            IEnumerable<ushort> addresses)
        {
            return FormBuckets(addresses, MaxDiscretesPerRead);
        }

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
