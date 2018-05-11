using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Utilities.Tests.HashExtensionsTests
{
    public class HashingTests : TestFixtureBase
    {
        [Test]
        public void HashesByteArrayUsingSha256Correctly()
        {
            byte[] source = Encoding.ASCII.GetBytes("Gaga");
            byte[] hash = source.GetHashCode<SHA256Managed>();
            Assert.AreEqual("6F9D8993CF8925C317CD5404C7BEB4302605173CA6C5EF27A4CDC65288059958", hash.ToStringAggregate(x => x.ToString("X2")));
        }

        [Test]
        public void HashesByteArrayUsingSha256Correctly_ObjectOverload()
        {
            byte[] source = Encoding.ASCII.GetBytes("Gaga");
            byte[] hash = HashExtensions.GetHashCode<SHA256Managed>(@object: source);
            Assert.AreEqual("6F9D8993CF8925C317CD5404C7BEB4302605173CA6C5EF27A4CDC65288059958", hash.ToStringAggregate(x => x.ToString("X2")));
        }

        [Test]
        public void HashesUsingSha256AndSaltCorrectly()
        {
            byte[] source = Encoding.ASCII.GetBytes("Gaga");
            byte[] hash = source.GetHashCode<SHA256Managed>(Encoding.ASCII.GetBytes("salt"));
            Assert.AreEqual("CACC55615CBF9658194C9FBE2928401B853D4118986059E38C0AF27540578562", hash.ToStringAggregate(x => x.ToString("X2")));
        }

        [Test]
        public void SameObjectsShouldHaveSameHashes_1()
        {
            string str1 = "Gaga";
            string str2 = "Gaga";
            byte[] hash1 = str1.GetHashCode<SHA256Managed>();
            byte[] hash2 = str2.GetHashCode<SHA256Managed>();
            Assert.IsTrue(hash1.SequenceEqual(hash2));
        }

        [Test]
        public void SameObjectsShouldHaveSameHashes_2()
        {
            Tuple<string, int> t1 = new Tuple<string, int>("Gaga", 2);
            Tuple<string, int> t2 = new Tuple<string, int>("Gaga", 2);
            byte[] hash1 = t1.GetHashCode<SHA256Managed>();
            byte[] hash2 = t2.GetHashCode<SHA256Managed>();
            Assert.IsTrue(hash1.SequenceEqual(hash2));
        }

        [Test]
        public void DifferentObjectShouldHaveDifferentHashes()
        {
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime(2016, 11, 6);
            Assert.IsFalse(d1.GetHashCode<SHA256Managed>().SequenceEqual(d2.GetHashCode<SHA256Managed>()));
        }

        [Test]
        public void CombineHashCode()
        {
            int a1 = 2, a2 = 3;
            int hash = HashExtensions.CombineHashCodes(a1, a2);

            Assert.AreNotEqual(a1, hash);
            Assert.AreNotEqual(a2, hash);
        }

        [Test]
        public void CombineHashCodeThrowExceptionIfNullValue()
        {
            int a1 = 2, a2 = 3;
            Assert.Throws<ArgumentNullException>(() => HashExtensions.CombineHashCodes(a1, a2, null));
        }

        [Test]
        public void CombineHashCodeThrowExceptionIfNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => HashExtensions.CombineHashCodes(null));
        }
    }
}
