using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class GetPermissionTests
    {
        private class DerivedPermissionA : Permission
        {

        }

        private class DerivedPermissionB : DerivedPermissionA
        {

        }

        [Test]
        public void ReturnsCorrectDerivedInstance() // here we have to make sure that no base classes are returned
        {
            BgcRole role = new BgcRole()
            {
                Permissions = new HashSet<Permission>()
                {
                    new DerivedPermissionB(),
                    new DerivedPermissionA()
                }
            };

            Assert.IsTrue(role.GetPermission<DerivedPermissionA>() == role.Permissions.Last());
        }
    }
}
