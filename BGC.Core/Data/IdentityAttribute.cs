using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
    /// <summary>
    /// Specifies the property of a DTO object which will be used to uniquely identify and map domain entities with DAL DTOs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class IdentityAttribute : Attribute
    {
        public string IdentityPropertyName { get; private set; }

        public IdentityAttribute(string identityPropertyName)
        {
            Shield.Assert(identityPropertyName, !string.IsNullOrWhiteSpace(identityPropertyName), (x) => new ArgumentNullException($"Argument {nameof(identityPropertyName)} cannot be a null, empty or whitespace string.")).ThrowOnError();

            IdentityPropertyName = identityPropertyName;
        }
    }
}
