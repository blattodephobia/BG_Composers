using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class BgcUserTokenProvider
    {
		public static class Purposes
        {
            public static readonly string PasswordReset = nameof(PasswordReset);
        }
    }
}
