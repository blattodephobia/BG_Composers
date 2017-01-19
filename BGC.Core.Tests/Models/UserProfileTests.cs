using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class GetSetLocaleTests
    {
        [Test]
        public void SetsLocaleToUserSettings()
        {
            var user = new BgcUser() { UserSettings = new HashSet<Setting>() };
            UserProfile profile = new UserProfile(user);

            CultureInfo locale = new CultureInfo("de-DE");
            profile.PreferredLocale = locale;

            Assert.IsNotNull(user.UserSettings.OfType<CultureInfoSetting>().Select(c => c.Locale).FirstOrDefault());
        }

        [Test]
        public void GetsCorrectLocale()
        {
            var user = new BgcUser() { UserSettings = new HashSet<Setting>() };
            UserProfile profile = new UserProfile(user);

            CultureInfo locale = new CultureInfo("de-DE");
            profile.PreferredLocale = locale;

            CultureInfoSetting localeSetting = user.UserSettings.OfType<CultureInfoSetting>().Where(c => c?.Locale == locale).First();
            localeSetting.Locale = new CultureInfo("bg-BG");

            Assert.AreEqual("bg-BG", profile.PreferredLocale.Name);
        }
    }
}
