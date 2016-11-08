using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    public class SmtpClientConfigurationCollection : ConfigurationElementCollection
    {
        public SmtpClientConfigurationElement this[int index]
        {
            get
            {
                return BaseGet(index) as SmtpClientConfigurationElement;
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SmtpClientConfigurationElement this[string responseString]
        {
            get
            {
                return (SmtpClientConfigurationElement)BaseGet(responseString);
            }

            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SmtpClientConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SmtpClientConfigurationElement)element).Purpose;
        }
    }
}
