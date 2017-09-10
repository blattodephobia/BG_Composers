using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services.Tests
{
    [TestFixture]
    public class SmtpClientConfigurationElementTests
    {
        private Configuration _config;
        [OneTimeSetUp]
        public void SetConfiguration()
        {
            // Get the directory containing the test assembly, so that a temporary .config file can be written to it.
            // We can't rely on other paths, because there may be no permissions to write to them.
            // Also, we can't rely on Environment.CurrentDirectory, since it may turn out to be %systemroot%\System32
            // or a Visual Studio installation folder
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            string tmpConfigFileName = Path.Combine(Directory.GetParent(execAssembly.Location).FullName, "SmtpConfig.config");
            using (Stream tmpFile = File.Open(tmpConfigFileName, FileMode.Create))
            using (Stream testFile = (execAssembly.GetManifestResourceStream(@"BGC.Services.Tests.TestFiles.SmtpConfig.config")))
            {
                testFile.CopyTo(tmpFile);
            }
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = tmpConfigFileName };
            _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        [Test]
        public void ShouldParseXmlSectionCorrectly()
        {
            SmtpClientConfigurationSection section = _config.GetSection("EmailClients") as SmtpClientConfigurationSection;
            SmtpClientConfigurationElement smtpClient = section.SmtpClients[0];
            
            Assert.AreEqual("InvitationSender", smtpClient.Purpose);
            Assert.AreEqual("user", smtpClient.UserName);
            Assert.AreEqual("password", smtpClient.Password);
            Assert.AreEqual("smtp.smtp-server.domain", smtpClient.Host);
            Assert.AreEqual(587, smtpClient.Port);
            Assert.AreEqual(true, smtpClient.EnableSsl);
            Assert.AreEqual(SmtpDeliveryMethod.Network, smtpClient.DeliveryMethod);
        }

        [Test]
        public void ShouldThrowExceptionOnInvalidPort()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                SmtpClientConfigurationSection section = _config.GetSection("EmailClients") as SmtpClientConfigurationSection;
                SmtpClientConfigurationElement smtpClient = section.SmtpClients[1];
                Assert.Fail($"The given SmtpClient contains an invalid value, {smtpClient.Port}, but no expected exception was thrown.");
            });
        }
    }
}
