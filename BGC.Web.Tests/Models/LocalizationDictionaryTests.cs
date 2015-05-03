using BGC.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.WebAPI.Tests.Models
{
	[TestClass]
	public class LocalizationDictionaryTests
	{
		[TestClass]
		public class SetStaticFieldsRecursivelyTests
		{
			internal sealed class MockLocalizationClass
			{
				public class FirstNestedLocalization
				{
					private static string testKey;
					private static object testEmpty1;
					private object testEmpty2;

					public string TestKey { get { return testKey; } }
					public object TestEmpty1 { get { return testEmpty1; } }
					public object TestEmpty2 { get { return this.testEmpty2; } }
				}
			}

			[TestMethod]
			public void SetsCorrectKeys()
			{
				PrivateObject localizationDictionary = new PrivateObject(typeof(LocalizationDictionary), null);
				localizationDictionary.Invoke("SetStaticFieldsRecursively", BindingFlags.NonPublic | BindingFlags.Static, typeof(MockLocalizationClass));
				var testResultsClass = new SetStaticFieldsRecursivelyTests.MockLocalizationClass.FirstNestedLocalization();
				Assert.AreEqual("MockLocalizationClass.FirstNestedLocalization.TestKey", testResultsClass.TestKey);
				Assert.IsNull(testResultsClass.TestEmpty1);
				Assert.IsNull(testResultsClass.TestEmpty2);
			}
		}
	}
}
