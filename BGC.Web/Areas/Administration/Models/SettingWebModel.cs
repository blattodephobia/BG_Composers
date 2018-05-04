using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Models
{
    public class SettingWebModel
    {
        public string Name { get; set; }

        private Type _type;
        public Type Type => _type ?? (_type = Type.GetType(TypeName));

        public string TypeName { get; set; }

        public virtual string Value { get; set; }

        public SettingWebModel()
        {
        }

        public SettingWebModel(string name, Type type, string value = null)
        {
            Name = name;
            TypeName = type.FullName;
            _type = type;
            Value = value;
        }
    }
}