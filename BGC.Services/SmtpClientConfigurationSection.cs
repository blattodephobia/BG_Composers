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
        public static SmtpClientConfigurationSection FromConfigFile(ConfigurationUserLevel userLevel = ConfigurationUserLevel.None)
        {
            return ConfigurationManager.OpenExeConfiguration(userLevel).Sections.OfType<SmtpClientConfigurationSection>().Single();
        }

        [ConfigurationProperty(nameof(SmtpClients))]
        [ConfigurationCollection(typeof(SmtpClientConfigurationCollection), AddItemName = "add")]
        public SmtpClientConfigurationCollection SmtpClients => this[nameof(SmtpClients)] as SmtpClientConfigurationCollection;
    }
}
