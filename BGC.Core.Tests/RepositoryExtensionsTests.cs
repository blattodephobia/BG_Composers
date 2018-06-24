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
                RepositoryExtensions.Delete<long, MediaTypeInfo>(null, new[] { new MediaTypeInfo("image/*") });
            });
        }

        [Test]
        public void ThrowsExceptionIfNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                RepositoryExtensions.Delete<long, MediaTypeInfo>(null, new[] { new MediaTypeInfo("image/*") });
            });
        }

        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                RepositoryExtensions.Delete(new Mock<INonQueryableRepository<long, MediaTypeInfo>>().Object, entity: null);
            });
        }

        [Test]
        public void CallsMethodCorrectly1()
        {
            var repo = new Mock<INonQueryableRepository<long, MediaTypeInfo>>();
            long[] callback = null;
            repo.Setup(r => r.Delete(It.IsAny<long[]>())).Callback((long[] @params) => callback = @params.ToArray());

            long[] keys = new[] { 0L, 12L, 7L };
            repo.Object.Delete(keys);

            Assert.IsTrue(callback.SequenceEqual(keys));
        }

        [Test]
        public void CallsMethodCorrectly2()
        {
            var repo = new Mock<INonQueryableRepository<long, MediaTypeInfo>>();
            MediaTypeInfo callbackObj = null;
            repo.Setup(r => r.Delete(It.Is<long[]>(x => x.Length == 1))).Callback((long[] key) => callbackObj = new MediaTypeInfo(@"image/*") { Id = key[0] });

            MediaTypeInfo deleteEntity = new MediaTypeInfo(@"image/*") { Id = 5 };
            repo.Object.Delete(deleteEntity);

            Assert.AreEqual(deleteEntity.Id, callbackObj.Id);
        }
    }
}
