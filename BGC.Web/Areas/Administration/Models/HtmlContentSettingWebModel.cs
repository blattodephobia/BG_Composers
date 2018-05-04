using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Models
{
    public class HtmlContentSettingWebModel : SettingWebModel
    {
        [AllowHtml]
        public override string Value { get => base.Value; set => base.Value = value; }

        public HtmlContentSettingWebModel()
        {
        }

        public HtmlContentSettingWebModel(string name, Type type, string value = null) :
            base(name, type)
        {
            Value = value;
        }
    }
}