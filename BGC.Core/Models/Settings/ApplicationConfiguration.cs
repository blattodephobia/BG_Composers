using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using BGC.Core.Services;

namespace BGC.Core
{
    public class ApplicationConfiguration : ConfigurationBase
    {
        public ApplicationConfiguration(ISettingsService settingsService) :
            base(settingsService, "Global")
        {
        }

        public HtmlString InvitationMessage
        {
            get
            {
                return ReadValue<HtmlString>();
            }

            set
            {
                SetValue(value);
            }
        }

        public string InvitationSubject
        {
            get
            {
                return ReadValue<string>();
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
