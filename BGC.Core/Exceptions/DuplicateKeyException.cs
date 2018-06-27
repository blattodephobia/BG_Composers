using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Exceptions
{
    public class DuplicateKeyException : BgcException
    {
        public DuplicateKeyException(string message = null, Exception innerException = null) :
            base(message, innerException)
        {
        }
    }
}
