using CodeShield;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    public class SmtpClientConfigurationSection : ConfigurationSection
    {
        public static SmtpClientConfigurationSection FromConfigFile(string sectionName)
        {
            Shield.ArgumentNotNull(sectionName, nameof(sectionName)).ThrowOnError();
            try
            {
                return (SmtpClientConfigurationSection)ConfigurationManager.GetSection(sectionName);
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException($"The section with the name {sectionName} is not of type {typeof(SmtpClientConfigurationSection).FullName}.");
            }
        }

        [ConfigurationProperty(nameof(SmtpClients))]
        [ConfigurationCollection(typeof(SmtpClientConfigurationCollection), AddItemName = "add")]
        public SmtpClientConfigurationCollection SmtpClients => this[nameof(SmtpClients)] as SmtpClientConfigurationCollection;
    }
}
