using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class EntityNotFoundException : BgcException
    {
        public EntityNotFoundException(object key, Type entityType, string message = null) :
            base(message ?? $"Entity of type {entityType?.FullName ?? "<UnknownType>"} was not found using the key '{key?.ToString() ?? "null"}'")
        {
            EntityKey = key;
            EntityType = entityType;
        }

        public Type EntityType { get; private set; }

        public object EntityKey { get; private set; }
    }
}
