using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class EqualityTests
    {
        public class TestPermissionA : Permission
        {
        }

        public class TestPermissionB : Permission
        {
        }

        [Test]
        public void HashSetEquality()
        {
            TestPermissionA tp = new TestPermissionA();
            HashSet<Permission> hs = new HashSet<Permission>(new Permission[] { new TestPermissionA() });
            Assert.IsTrue(hs.Contains(tp));
        }

        [Test]
        public void EqualsTest1()
        {
            TestPermissionA tp1 = new TestPermissionA();
            TestPermissionA tp2 = new TestPermissionA();
            Assert.AreEqual(tp1, tp2);
        }

        [Test]
        public void EqualsTest2()
        {
            TestPermissionA tp1 = new TestPermissionA();
            TestPermissionB tp2 = new TestPermissionB();
            Assert.AreNotEqual(tp1, tp2);
        }

        [Test]
        public void TransitivityTest1()
        {
            TestPermissionA tp1 = new TestPermissionA();
            TestPermissionA tp2 = new TestPermissionA();
            TestPermissionA tp3 = new TestPermissionA();

            Assert.AreEqual(tp1, tp2);
            Assert.AreEqual(tp2, tp3);
            Assert.AreEqual(tp1, tp3);
        }

        [Test]
        public void TransitivityTest2()
        {
            TestPermissionA tp1 = new TestPermissionA();
            TestPermissionB tp2 = new TestPermissionB();
            TestPermissionB tp3 = new TestPermissionB();

            Assert.AreNotEqual(tp1, tp2);
            Assert.AreEqual(tp2, tp3);
            Assert.AreNotEqual(tp1, tp3);
        }
    }
}
