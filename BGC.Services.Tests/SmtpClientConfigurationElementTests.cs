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
        private static Configuration GetConfiguration(string relativePath)
        {
            // Get the directory containing the test assembly. We can't rely on Environment.CurrentDirectory, since
            // it may turn out to be %systemroot%\System32
            DirectoryInfo workingDir = new FileInfo(Assembly.GetAssembly(typeof(SmtpClientConfigurationElementTests)).Location).Directory;
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = workingDir.FullName + relativePath };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }

        [Test]
        public void ShouldParseXmlSectionCorrectly()
        {
            Configuration config = GetConfiguration(@"\TestFiles\SmtpConfig.config");
            SmtpClientConfigurationSection section = config.GetSection("EmailClients") as SmtpClientConfigurationSection;
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
                Configuration config = GetConfiguration(@"\TestFiles\SmtpConfig.config");
                SmtpClientConfigurationSection section = config.GetSection("EmailClients") as SmtpClientConfigurationSection;
                SmtpClientConfigurationElement smtpClient = section.SmtpClients[1];
                Assert.Fail($"The given SmtpClient contains an invalid value, {smtpClient.Port}, but no expected exception was thrown.");
            });
        }
    }
}
