using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class SettingException : BgcException
    {
        public SettingException(string message = null, Exception innerException = null) :
            base(message, innerException)
        {
        }
    }
}
