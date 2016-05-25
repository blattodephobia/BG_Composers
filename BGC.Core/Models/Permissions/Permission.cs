using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public abstract class Permission : BgcEntity<long>
    {
        private string name;

        /// <summary>
        /// Identifies the permissions in db stores.
        /// This property's value should be the same as the one returned by the GetType().FullName property.
        /// Although this property should technically be a read-only property, the setter is provided for compatibility
        /// with ORM frameworks.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name ?? (this.name = this.GetType().FullName); ; 
            }

            set
            {
                Shield.Assert(
                    value,
                    value == this.GetType().FullName,
                    s => new InvalidOperationException($"Invalid relational mapping detected. Property {nameof(Name)} and the type name returned by the GetType().FullName must match. Expected {this.GetType().FullName}, actual value is {s}"));
                this.name = value;
            }
        }
    }
}
