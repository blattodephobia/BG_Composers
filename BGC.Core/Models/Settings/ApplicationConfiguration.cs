using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Core.Services;

namespace BGC.Core
{
    public class ApplicationConfiguration : ConfigurationBase
    {
        public ApplicationConfiguration(ISettingsService settingsService) :
            base(settingsService, "Global")
        {
        }

        public string InvitationMessage
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
