using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
    internal class CultureInfoConvention : ComplexTypeDiscoveryConvention
    {
        public override void Apply(EdmModel item, DbModel model)
        {
            base.Apply(item, model);
        }
    }
}
