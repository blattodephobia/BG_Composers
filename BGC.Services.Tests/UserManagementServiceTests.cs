using BGC.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests
{
    [TestFixture]
    public class UserManagementServiceTests
    {
        [Test]
        public void ChecksPermissionFromCtor()
        {
            Mock<IRepository<BgcUser>> userRepo = new Mock<IRepository<BgcUser>>();
            Mock<IRepository<Invitation>> invitationsRepo = new Mock<IRepository<Invitation>>();
            BgcUser mockUser = new BgcUser();

            Assert.Throws<InvalidOperationException>(() =>
            {
                UserManagementService svc = new UserManagementService(userRepo.Object, invitationsRepo.Object, mockUser);
            });
        }

        [Test]
        public void ChecksPermissionFromProperty()
        {
            Mock<IRepository<BgcUser>> userRepo = new Mock<IRepository<BgcUser>>();
            Mock<IRepository<Invitation>> invitationsRepo = new Mock<IRepository<Invitation>>();

            BgcUser mockCorrectUser = new BgcUser();
            mockCorrectUser.Roles.Add(new BgcUserRole() { Role = new AdministratorRole(), User = mockCorrectUser });

            BgcUser mockWrongUser = new BgcUser();

            UserManagementService svc = new UserManagementService(userRepo.Object, invitationsRepo.Object, mockCorrectUser);
            Assert.Throws<InvalidOperationException>(() =>
            {
                svc.Administrator = mockWrongUser;
            });
        }
    }
}
