using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BGC.Core.Tests.Models
{
	[TestClass]
	public class ComposerNameTests
	{
		[TestClass]
		public class NameTests
		{
			[TestMethod]
			public void FirstAndLastNameDependencies()
			{
				ComposerName name = new ComposerName();
				name.FirstName = "First";
				Assert.AreEqual("First", name.FullName);

				name.LastName = "Last";
				Assert.AreEqual("First Last", name.FullName);

				name.FullName = "First1 Middle1 Last1";
				Assert.AreEqual("First1", name.FirstName);
				Assert.AreEqual("Last1", name.LastName);
			}
		}
	}
}
