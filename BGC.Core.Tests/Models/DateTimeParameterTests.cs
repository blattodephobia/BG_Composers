using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestClass]
    public class DateTimeParameterTests
    {
        [TestClass]
        public class StringValueTests
        {
            [TestMethod]
            public void SyncsFromDateCorrectly1()
            {
                DateTimeParameter param = new DateTimeParameter();
                Assert.AreEqual(param.Date, DateTime.Parse(param.StringValue, DateTimeParameter.FormatProvider));
            }

            [TestMethod]
            public void SyncsFromDateCorrectly2()
            {
                DateTimeParameter param = new DateTimeParameter();
                param.Date = new DateTime(2013, 5, 17, 0, 0, 0);
                Assert.AreEqual("17 May 2013, 00:00:00.000", param.StringValue);
            }

            [TestMethod]
            public void SyncsToDateCorrectly()
            {
                DateTimeParameter param = new DateTimeParameter();
                param.StringValue = "26 Jun 1989, 00:12:21.999";
                Assert.AreEqual(26, param.Date.Day);
                Assert.AreEqual(6, param.Date.Month);
                Assert.AreEqual(1989, param.Date.Year);
                Assert.AreEqual(0, param.Date.Hour);
                Assert.AreEqual(12, param.Date.Minute);
                Assert.AreEqual(21, param.Date.Second);
                Assert.AreEqual(999, param.Date.Millisecond);
            }
        }
    }
}
