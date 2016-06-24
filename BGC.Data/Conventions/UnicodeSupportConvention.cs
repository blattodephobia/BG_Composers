using BGC.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Reflection;

namespace BGC.Data.Conventions
{
    internal class UnicodeSupportConvention : PrimitivePropertyAttributeConfigurationConvention<UnicodeAttribute>
    {
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration, UnicodeAttribute attribute)
        {
            configuration.IsUnicode(attribute.IsUnicode);
        }
    }
}
