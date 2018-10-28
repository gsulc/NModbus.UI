using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NModbus.UI.InteractionModule;

namespace UnitTests
{
    [TestClass]
    public class BucketTests
    {
        [TestMethod]
        public void TestOneBucketCountNoOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 3, 4, 5, 6, 7 };
            ushort maxContiguous = (ushort)addresses.Length;

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);

            Assert.AreEqual(1, buckets.Count());
        }

        [TestMethod]
        public void TestOneBucketStartNoOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 3, 4, 5, 6, 7 };
            ushort maxContiguous = (ushort)addresses.Length;

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);
            ushort address = buckets.ElementAt(0).Item1;

            Assert.AreEqual(0, address);
        }

        [TestMethod]
        public void TestOneBucketLengthNoOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 3, 4, 5, 6, 7 };
            ushort maxContiguous = (ushort)addresses.Length;

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);
            ushort length = buckets.ElementAt(0).Item2;

            Assert.AreEqual(addresses.Length, length);
        }

        [TestMethod]
        public void TestTwoBucketCountOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 3, 4, 5, 6, 7 };
            ushort maxContiguous = (ushort)(addresses.Length - 1);

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);

            Assert.AreEqual(2, buckets.Count());
        }

        [TestMethod]
        public void TestFourBucketCountMultipleOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 3, 4, 5, 6, 7 };
            ushort maxContiguous = 2;

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);

            Assert.AreEqual(4, buckets.Count());
        }

        [TestMethod]
        public void TestTwoBucketsAddressesNoOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 4, 5, 6, 7 };
            ushort maxContiguous = (ushort)addresses.Length;

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);
            ushort address1 = buckets.ElementAt(0).Item1;
            ushort address2 = buckets.ElementAt(1).Item1;

            Assert.AreEqual(0, address1);
            Assert.AreEqual(4, address2);
        }

        [TestMethod]
        public void TestTwoBucketsLengthsNoOverflow()
        {
            ushort[] addresses = { 0, 1, 2, 4, 5, 6, 7 };
            ushort maxContiguous = (ushort)addresses.Length;

            var buckets = Extensions.FormBuckets(addresses, maxContiguous);
            ushort length1 = buckets.ElementAt(0).Item2;
            ushort length2 = buckets.ElementAt(1).Item2;
            
            Assert.AreEqual(3, length1);
            Assert.AreEqual(4, length2);
        }
    }
}
