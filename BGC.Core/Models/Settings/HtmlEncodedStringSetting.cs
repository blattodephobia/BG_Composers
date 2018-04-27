using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace BGC.Core
{
    /// <summary>
    /// A setting whose value is an HTML-encoded HTML string.
    /// </summary>
    public class HtmlEncodedStringSetting : XmlDocumentSetting, IParameter<HtmlString>
    {
        private static readonly Regex XmlSpecialCharactersRegex = new Regex("[&<>'\"]", RegexOptions.Compiled);
        private static readonly string PseudoRootElementFormatString = "<article_pseudo>{0}</article_pseudo>";
        private static readonly string PseudoRootElement = "article_pseudo";
        
        private static string EnsureEncoded(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            string encodedValue = XmlSpecialCharactersRegex.IsMatch(value)
                ? HttpUtility.HtmlEncode(value)
                : value;

            return encodedValue;
        }

        protected override XmlDocument LoadXml(string xml)
        {
            XmlDocument result = new XmlDocument();
            try
            {
                result.LoadXml(xml);
            }
            catch (XmlException)
            {
                if (!string.IsNullOrWhiteSpace(xml)) // the exception was thrown because the xml string didn't contain a root element
                {
                    string xmlStringWithRoot = string.Format(PseudoRootElementFormatString, xml);
                    result.LoadXml(xmlStringWithRoot);
                }
            }

            return result;
        }

        protected override void OnDocumentNodeChanged(object sender, XmlNodeChangedEventArgs e)
        {
            base.OnDocumentNodeChanged(sender, e);

            Nodes = Document.DocumentElement.ChildNodes.Cast<XmlNode>();
            _stringValue = EnsureEncoded(Document.InnerXml);
            _htmlString = new HtmlString(_stringValue);
        }

        public override Type ValueType => typeof(HtmlString);

        private string _stringValue;
        public override string StringValue
        {
            get
            {
                return _stringValue;
            }

            set
            {
                string encodedValue = null;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    encodedValue = EnsureEncoded(value);
                    _htmlString = new HtmlString(encodedValue);
                    XmlDocument parsedXml = LoadXml(value);
                    if (parsedXml.DocumentElement.Name != PseudoRootElement)
                    {
                        base.Document = parsedXml;
                        Nodes = new XmlNode[] { parsedXml.DocumentElement };
                    }
                    else
                    {
                        base.Document = null;
                        Nodes = parsedXml.DocumentElement.ChildNodes.Cast<XmlNode>();
                    }
                }
                else
                {
                    _htmlString = null;
                    base.Document = null;
                }

                _stringValue = encodedValue;
            }
        }

        private HtmlString _htmlString;
        [NotMapped]
        public HtmlString HtmlString
        {
            get
            {
                return _htmlString;
            }

            set
            {
                StringValue = value?.ToHtmlString();
            }
        }

        /// <summary>
        /// Stores a binary representation of the <see cref="HtmlString"/> value, if that HTML is a proper XML document (i.e. has a single root node).
        /// If the <see cref="HtmlString"/> contains multiple root nodes, this property's value will be null; in this case, refer to the <see cref="Nodes"/> property.
        /// </summary>
        public override XmlDocument Document
        {
            get
            {
                return base.Document;
            }

            set
            {
                if (value != null)
                {
                    if (value.DocumentElement.Name == PseudoRootElement)
                    {
                        base.Document = null;
                        Nodes = value.DocumentElement.ChildNodes.Cast<XmlNode>();
                    }
                    else
                    {
                        base.Document = value;
                        Nodes = new XmlNode[] { value.DocumentElement };
                    }
                }
                else
                {
                    base.Document = null;
                }

                _stringValue = EnsureEncoded(value?.InnerXml);
                _htmlString = _stringValue != null ? new HtmlString(_stringValue) : null;
            }
        }

        /// <summary>
        /// This collection contains the root node(s) of the <see cref="HtmlString"/> property.
        /// </summary>
        public IEnumerable<XmlNode> Nodes { get; private set; }

        protected HtmlEncodedStringSetting()
        {
        }

        public HtmlEncodedStringSetting(string name) :
            base(name)
        {

        }

        public HtmlEncodedStringSetting(string name, string html) :
            base(name)
        {
            StringValue = html;
        }

        string IParameter<HtmlString>.Name => base.Name;

        HtmlString IParameter<HtmlString>.Value { get => HtmlString; set => HtmlString = value; }
    }
}
