using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class EncodingExtensionsTests
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
}
