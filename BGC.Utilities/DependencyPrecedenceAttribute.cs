using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Specifies the priority of a <see cref="DependencySource{T}"/> declared in a <see cref="DependencyValue{T}"/>.
    /// Properties with higher <see cref="Priority"/> override those with a lower one.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DependencyPrecedenceAttribute : Attribute
    {
        public uint Priority { get; private set; }

        public DependencyPrecedenceAttribute(uint priority)
        {
            Priority = priority;
        }
    }
}
