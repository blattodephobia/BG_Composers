using Microsoft.AspNet.Identity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BGC.Services.Tests
{
    internal class EmailServiceProxy : EmailService
    {
        // The base classe's behavior is coupled to the .exe's config file.
        // This override will be used for mocking.
        protected override SmtpClient GetEmailClient() 
        {
            return new SmtpClient();
        }

        public MailMessage ConvertToMailMessageProxy(IdentityMessage message) => ConvertToMailMessage(message);

        public EmailServiceProxy() :
            base(purpose: "testing")
        {
        }
    }

    [TestFixture]
    public class ConvertToMailMessageTests
    {
        [Test]
        public void ValidatesIdentityMessages()
        {
            var svc = new EmailServiceProxy() { Sender = "someone@mail.com" };
            Assert.Throws<InvalidOperationException>(() => svc.ConvertToMailMessageProxy(new IdentityMessage() { Destination = null }));
        }

        [Test]
        public void ConvertsDestinationCorrectly()
        {
            var svc = new EmailServiceProxy() { Sender = "someone@mail.com" };
            MailMessage message = svc.ConvertToMailMessageProxy(new IdentityMessage() { Destination = "sample@host.domain" });
            Assert.AreEqual("sample@host.domain", message.To.First().Address);
        }
    }
}
