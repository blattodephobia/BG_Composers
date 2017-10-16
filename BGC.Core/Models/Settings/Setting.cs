using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    [DebuggerDisplay("{" + nameof(StringValue) + "}")]
    [DebuggerTypeProxy(typeof(SettingDebugView))]
    public partial class Setting : BgcEntity<long>, IParameter<string>
    {
        internal void SetReadOnly() => IsReadOnly = true;

        protected void SetValue<T>(ref T backingStore, T value)
        {
            Shield.AssertOperation(
                value: this,
                predicate: x => !x.IsReadOnly,
                messageProvider: (Setting x) => $"This instance of {GetType().FullName} cannot be modified, since it's read-only.")
            .ThrowOnError();

            backingStore = value;
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                SetValue(ref _name, value);
            }
        }

        [NotMapped]
        public bool IsReadOnly { get; private set; }

        private string _description;

        [Unicode]
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                SetValue(ref _description, value);
            }
        }

        private string _stringValue;
        [Unicode]
        public virtual string StringValue
        {
            get
            {
                return _stringValue;
            }

            set
            {
                SetValue(ref _stringValue, value);
            }
        }

        private SettingPriority _priority;
        public SettingPriority Priority
        {
            get
            {
                return _priority;
            }

            set
            {
                SetValue(ref _priority, value);
            }
        }

        public sealed override string ToString() => StringValue;

        string IParameter<string>.Value
        {
            get
            {
                return StringValue;
            }

            set
            {
                StringValue = value;
            }
        }
    }
}
