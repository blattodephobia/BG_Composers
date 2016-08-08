using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    /// <summary>
    /// Specifies whether a member of type <see cref="string"/> supports Unicode or not.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    internal sealed class UnicodeAttribute : Attribute
    {
        public bool IsUnicode { get; private set; }

        public UnicodeAttribute() :
            this(true)
        {
        }

        public UnicodeAttribute(bool isUnicode)
        {
            IsUnicode = isUnicode;
        }
    }
}
