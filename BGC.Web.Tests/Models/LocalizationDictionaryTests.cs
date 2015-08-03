using BGC.Utilities;
using BGC.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.WebAPI.Tests.Models
{
	[TestClass]
	public class LocalizationDictionaryTests
	{
		[TestClass]
		public class PropertiesTests
		{
			[TestMethod]
			public void SetsPropertyKeysCorrectly1()
			{
				LocalizationDictionary dict = new LocalizationDictionary(new Dictionary<string, string>());
				Assert.AreEqual("LocalizationDictionary.GenericText.Ok", dict.GenericText.Ok.Key);
			}

			[TestMethod]
			public void LocalizesCorrectly1()
			{
				LocalizationDictionary dict = new LocalizationDictionary(new Dictionary<string, string>() { { "LocalizationDictionary.GenericText.Ok", "OK" } });
				Assert.AreEqual("OK", dict.GenericText.Ok.Value);
			}
		}
	}
}
