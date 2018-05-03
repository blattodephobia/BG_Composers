using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Core.Tests.Models.Settings.CultureInfoSettingTests
{
    public class CultureInfoSettingTests : TestFixtureBase
    {
        [Test]
        public void AcceptsNullValues()
        {
            CultureInfoSetting setting = new CultureInfoSetting("setting", new CultureInfo("en-US"));

            setting.StringValue = null;

            Assert.IsNull(setting.Locale);
        }
    }
}
