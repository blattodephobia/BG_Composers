using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    internal interface IIntermediateRelationalDto<TPrincipal, TDependant>
        where TPrincipal : RelationdalDtoBase
        where TDependant : RelationdalDtoBase
    {
        TPrincipal Principal { get; set; }

        TDependant Dependant { get; set; }
    }
}
