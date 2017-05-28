using BGC.Web.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BGC.Web.Tests.Models.ActionCollectionTests
{
    public class MockController
    {
        [NonAction]
        public ActionResult NonAction() => null;

        public ViewResult ViewResult() => null;

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult AcceptVerbs() => null;

        [HttpPost]
        public ActionResult Post() => null;

        [HttpGet]
        [HttpDelete]
        public ViewResult Multiple() => null;

        public int WrongReturnType() => 0;

        public ActionResult Action() => null;

        [HttpPost]
        [ActionName(nameof(Action))]
        public Task<ViewResult> Action_Post() => null;
    }

    [TestFixture]
    public class CtorTests
    {

        ActionCollection _col;

        [OneTimeSetUp]
        public void InitActionCollection()
        {
            _col = ActionCollection.FromType(typeof(MockController));
        }

        [Test]
        public void ExcludesNonActionResultMethods()
        {
            Assert.IsNull(_col[nameof(MockController.NonAction)]);
        }

        [Test]
        public void ExcludesMethodsWithWrongType()
        {
            Assert.IsNull(_col[nameof(MockController.NonAction)]);
        }

        [Test]
        public void UsesActionNameAttributeForActionNames()
        {
            Assert.AreEqual(nameof(MockController.Action_Post), _col[nameof(MockController.Action), HttpVerbs.Post].Name);
        }

        [Test]
        public void UsesAcceptVerbsAttributeAppropriately()
        {
            Assert.AreEqual(nameof(MockController.AcceptVerbs), _col[nameof(MockController.AcceptVerbs), HttpVerbs.Get].Name);
            Assert.AreEqual(nameof(MockController.AcceptVerbs), _col[nameof(MockController.AcceptVerbs), HttpVerbs.Post].Name);
        }

        [Test]
        public void UsesMultipleActionSelectorAttributesCorrectly()
        {
            Assert.AreSame(_col[nameof(MockController.Multiple), HttpVerbs.Get], _col[nameof(MockController.Multiple), HttpVerbs.Delete]);
        }

        [Test]
        public void ActionWithoutAttributesAcceptsAnyVerb()
        {
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Get].Name);
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Post].Name);
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Put].Name);
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Patch].Name);
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Options].Name);
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Delete].Name);
            Assert.AreEqual(nameof(MockController.ViewResult), _col[nameof(MockController.ViewResult), HttpVerbs.Head].Name);
        }

        [Test]
        public void SelectsCorrectMethods()
        {
            Type type = typeof(MockController);
            IEnumerable<MethodInfo> actions = new MethodInfo[]
            {
                type.GetMethod(nameof(MockController.ViewResult)),
                type.GetMethod(nameof(MockController.AcceptVerbs)),
                type.GetMethod(nameof(MockController.Post)),
                type.GetMethod(nameof(MockController.Action)),
                type.GetMethod(nameof(MockController.Action_Post)),
                type.GetMethod(nameof(MockController.Multiple)),
            }
            .OrderBy(mi => mi.Name);

            IEnumerable<MethodInfo> selectedActions = _col.Distinct().OrderBy(mi => mi.Name);

            Assert.IsTrue(actions.SequenceEqual(selectedActions));
        }
    }

    [TestFixture]
    public class IndexerTests
    {
        ActionCollection _col;

        [OneTimeSetUp]
        public void InitActionCollection()
        {
            _col = ActionCollection.FromType(typeof(MockController));
        }

        [Test]
        public void MapsStringAndHttpVerbsCorrectly()
        {
            Assert.AreSame(_col[nameof(MockController.Action), HttpVerbs.Post], _col[nameof(MockController.Action), "post"]);
        }
    }
}
