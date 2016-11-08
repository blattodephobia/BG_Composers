using BGC.Core;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly string NetworkCredentialValidationErrorMessage =
            $"The supplied {nameof(NetworkCredential)} has one or more invalid properties: " +
            $"{nameof(NetworkCredential.UserName)}, and either one of " +
            $"{nameof(NetworkCredential.Password)} or {nameof(NetworkCredential.SecurePassword)} have to be non-null and non-empty.";

        private SmtpClientConfigurationElement _configuration;
        private SmtpClientConfigurationElement Configuration => _configuration ?? (_configuration = SmtpClientConfigurationSection
                .FromConfigFile()
                .SmtpClients
                .Cast<SmtpClientConfigurationElement>()
                .FirstOrDefault(client => client.Purpose == Purpose));
        
        public string Purpose { get; private set; }

        protected virtual MailMessage ConvertToMailMessage(IdentityMessage message)
        {
            Shield.ArgumentNotNull(message).ThrowOnError();
            Shield.ValueNotNull(message.Destination).ThrowOnError();

            return new MailMessage(new MailAddress(Sender), new MailAddress(message.Destination))
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
        }

        protected virtual SmtpClient GetEmailClient()
        {
            if (Configuration == null)
            {
                throw new InvalidOperationException($"The application's config file contains no SMTP client that corresponds to this {nameof(EmailService)}'s purpose, {Purpose}.");
            }

            return Configuration.ToSmtpClient();
        }

        private string _sender;
        public string Sender
        {
            get
            {
                return _sender;
            }

            set
            {
                Shield.ValueNotNull(value, nameof(Sender));
                _sender = value;
            }
        }

        public void Send(IdentityMessage message)
        {
            using (SmtpClient client = GetEmailClient())
            {
                client.Send(ConvertToMailMessage(message));
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
            using (SmtpClient client = GetEmailClient())
            {
                return client.SendMailAsync(ConvertToMailMessage(message));
            }
        }

        public EmailService(string purpose)
        {
            Purpose = Shield.IsNotNullOrEmpty(purpose, nameof(purpose)).GetValueOrThrow();
        }
    }
}
