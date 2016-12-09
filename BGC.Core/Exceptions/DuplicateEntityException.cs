using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Exceptions
{
    public class DuplicateEntityException : BgcException
    {
        public DuplicateEntityException(string message = null, Exception innerException = null) :
            base(message, innerException)
        {

        }        
    }
}
