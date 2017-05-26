using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public interface IPermission : IEquatable<IPermission>
    {
        string Name { get; }

        ICollection<BgcRole> Roles { get; }
    }
}
