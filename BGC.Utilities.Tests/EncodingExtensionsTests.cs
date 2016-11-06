using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class EncryptionTests
    {
        [Test]
        public void EncryptDecryptCorrectly()
        {
            string plainText = "Electric Kiss";
            byte[] encrypted = Encoding.ASCII.GetBytes(plainText).Encrypt("Gaga");
            string decrypted = Encoding.ASCII.GetString(encrypted.Decrypt("Gaga"));
            Assert.AreEqual(plainText, decrypted);
        }
    }

    [TestFixture]
    public class HashingTests
    {
        [Test]
        public void HashesUsingSha256Correctly()
        {
            byte[] source = Encoding.ASCII.GetBytes("Gaga");
            byte[] hash = source.GetHashCode<SHA256Managed>();
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
        public void HashGenericObject()
        {
            string str1 = "Gaga";
            string str2 = "Gaga";
            byte[] hash1 = str1.GetHashCode<SHA256Managed>();
            byte[] hash2 = str2.GetHashCode<SHA256Managed>();
            Assert.IsTrue(hash1.SequenceEqual(hash2));
        }
    }
}
