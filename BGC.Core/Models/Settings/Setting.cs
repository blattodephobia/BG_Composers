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
        public static T Rename<T>(T setting, string newName) where T : Setting
        {
            Shield.ArgumentNotNull(setting, nameof(setting)).ThrowOnError();
            Shield.ArgumentNotNull(newName, nameof(newName)).ThrowOnError();

            T result = Activator.CreateInstance(typeof(T), nonPublic: true) as T;
            result.Name = newName;
            result.StringValue = setting.StringValue;
            result.Description = setting.Description;
            result.Priority = setting.Priority;
            result.Id = setting.Id;

            return result;
        }

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
        [Required]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                Shield.AssertOperation(nameof(Name), value != null, $"The {nameof(Name)} property cannot be null.").ThrowOnError();
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

        [NotMapped]
        public virtual Type ValueType => typeof(string);

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

        private string _ownerStamp;
        [MaxLength(64)]
        public string OwnerStamp
        {
            get
            {
                return _ownerStamp;
            }

            set
            {
                _ownerStamp = value;
            }
        }

        protected Setting()
        {
        }

        public Setting(string name)
        {
            Shield.IsNotNullOrEmpty(name).ThrowOnError();

            Name = name;
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
