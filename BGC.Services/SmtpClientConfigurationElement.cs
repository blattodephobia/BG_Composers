using BGC.Utilities;
using CodeShield;
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
    public class SmtpClientConfigurationElement : ConfigurationElement
    {
        ushort? _port;
        bool? _enableSsl;
        SmtpDeliveryMethod? _deliveryMethod;

        [ConfigurationProperty(nameof(UserName), IsRequired = true)]
        public string UserName => this[nameof(UserName)] as string;

        [ConfigurationProperty(nameof(Password), IsRequired = true)]
        public string Password => this[nameof(Password)] as string;

        [ConfigurationProperty(nameof(Purpose), IsRequired = true)]
        public string Purpose => this[nameof(Purpose)] as string;

        [ConfigurationProperty(nameof(Host), IsRequired = true)]
        public string Host => this[nameof(Host)] as string;

        [ConfigurationProperty(nameof(Port), IsRequired = true)]
        public ushort Port
        {
            get
            {
                if (_port == null)
                {
                    _port = (ushort)this[nameof(Port)];
                    Shield.Assert(_port, _port > 0, (x) => new InvalidOperationException($"{x} is not a valid port value.")).ThrowOnError();
                }

                return _port.Value;
            }
        }

        [ConfigurationProperty(nameof(EnableSsl), IsRequired = true)]
        public bool EnableSsl
        {
            get
            {
                return (_enableSsl ?? (_enableSsl = Convert.ToBoolean(this[nameof(EnableSsl)]))).Value;
            }
        }

        [ConfigurationProperty(nameof(DeliveryMethod), IsRequired = true)]
        public SmtpDeliveryMethod DeliveryMethod
        {
            get
            {
                return (_deliveryMethod ?? (_deliveryMethod = (SmtpDeliveryMethod)this[nameof(DeliveryMethod)])).Value;
            }
        }

        public SmtpClient ToSmtpClient() => new SmtpClient()
        {
            Host = Host,
            Port = Port,
            EnableSsl = EnableSsl,
            UseDefaultCredentials = false,
            DeliveryMethod = DeliveryMethod,
            Credentials = new NetworkCredential(UserName, Password)
        };
    }
}
