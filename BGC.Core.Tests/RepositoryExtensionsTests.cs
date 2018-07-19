using BGC.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Core.Tests.RepositoryExtensionsTests
{
    public class DeleteTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullRepo()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                RepositoryExtensions.Delete<Guid, MediaTypeInfo>(null, new[] { new MediaTypeInfo("image/*") });
            });
        }

        [Test]
        public void ThrowsExceptionIfNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                RepositoryExtensions.Delete<Guid, MediaTypeInfo>(null, new[] { new MediaTypeInfo("image/*") });
            });
        }

        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                RepositoryExtensions.Delete(new Mock<INonQueryableRepository<Guid, MediaTypeInfo>>().Object, entity: null);
            });
        }

        [Test]
        public void CallsMethodCorrectly1()
        {
            var repo = new Mock<INonQueryableRepository<Guid, MediaTypeInfo>>();
            Guid[] callback = null;
            repo.Setup(r => r.Delete(It.IsAny<Guid[]>())).Callback((Guid[] @params) => callback = @params.ToArray());

            Guid[] keys = new[] { new Guid(1, 0, 0, new byte[8]), new Guid(2, 0, 0, new byte[8]), new Guid(3, 0, 0, new byte[8]) };
            repo.Object.Delete(keys);

            Assert.IsTrue(callback.SequenceEqual(keys));
        }

        [Test]
        public void CallsMethodCorrectly2()
        {
            var repo = new Mock<INonQueryableRepository<Guid, MediaTypeInfo>>();
            MediaTypeInfo callbackObj = null;
            repo.Setup(r => r.Delete(It.Is<Guid[]>(x => x.Length == 1))).Callback((Guid[] key) => callbackObj = new MediaTypeInfo(@"image/*") { Id = key[0] });

            MediaTypeInfo deleteEntity = new MediaTypeInfo(@"image/*") { Id = new Guid(5, 0, 0, new byte[8]) };
            repo.Object.Delete(deleteEntity);

            Assert.AreEqual(deleteEntity.Id, callbackObj.Id);
        }
    }
}
