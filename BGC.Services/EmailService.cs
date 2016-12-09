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

        /// <summary>
        /// The name of the configuration section in the current application's config file (app.config or Web.config)
        /// </summary>
        public static readonly string ConfigFileSectionName = "EmailServiceClients";

        private SmtpClientConfigurationElement _configuration;
        private SmtpClientConfigurationElement Configuration => _configuration ?? (_configuration = SmtpClientConfigurationSection
                .FromConfigFile(ConfigFileSectionName).ValueNotNull(ConfigFileSectionName).GetValueOrThrow()
                .SmtpClients
                .Cast<SmtpClientConfigurationElement>()
                .FirstOrDefault(client => client.Purpose == Purpose).ValueNotNull(Purpose).GetValueOrThrow());
        
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
                return _sender ?? (_sender = Configuration.Sender);
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

        public async Task SendAsync(IdentityMessage message)
        {
            using (SmtpClient client = GetEmailClient())
            {
                await client.SendMailAsync(ConvertToMailMessage(message));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="purpose">The kind of emails that will be sent through this service.
        /// The applications's config file must contain a configuration section with the same name as in <see cref="ConfigFileSectionName"/>.
        /// The section must contain an <see cref="SmtpClientConfigurationElement"/> with the same purpose.</param>
        /// <param name="sender">The sender, as they'll appear in the recipient's inbox, if the SMTP host supports email spoofing.
        /// Use null to retrieve that information from the <see cref="SmtpClientConfigurationElement"/>.</param>
        public EmailService(string purpose, string sender = null)
        {
            Purpose = Shield.IsNotNullOrEmpty(purpose, nameof(purpose)).GetValueOrThrow();
            if (sender != null)
            {
                Sender = sender;
            }
        }
    }
}
