using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BGC.Core
{
    public class XmlDocumentSetting : Setting, IParameter<XmlDocument>
    {
        private void SubscribeEvents(XmlDocument doc)
        {
            if (doc == null) return;

            doc.NodeChanged += Doc_NodeChanged;
            doc.NodeInserted += Doc_NodeChanged;
            doc.NodeRemoved += Doc_NodeChanged;
        }

        private void UnsubscribeEvents(XmlDocument doc)
        {
            if (doc == null) return;

            doc.NodeChanged -= Doc_NodeChanged;
            doc.NodeInserted -= Doc_NodeChanged;
            doc.NodeRemoved -= Doc_NodeChanged;
        }

        public override string StringValue
        {
            get
            {
                return base.StringValue;
            }

            set
            {
                Shield.AssertOperation(value, !string.IsNullOrWhiteSpace(value), $"{nameof(StringValue)} cannot be null or whitespace.").ThrowOnError();

                UnsubscribeEvents(_document);

                var doc = new XmlDocument();
                doc.LoadXml(value);
                base.StringValue = value;
                _document = doc;

                SubscribeEvents(doc);
            }
        }

        private void Doc_NodeChanged(object sender, XmlNodeChangedEventArgs e)
        {
            base.StringValue = (sender as XmlDocument).InnerXml;
        }

        public override Type ValueType => typeof(XmlDocument);

        private XmlDocument _document;
        [NotMapped]
        public XmlDocument Document
        {
            get
            {
                return _document;
            }

            set
            {
                Shield.AssertOperation(value, value != null, $"{nameof(Document)} property cannot be null.").ThrowOnError();

                UnsubscribeEvents(_document);

                _document = value;
                base.StringValue = _document.InnerXml;

                SubscribeEvents(value);
            }
        }

        string IParameter<XmlDocument>.Name => Name;

        XmlDocument IParameter<XmlDocument>.Value
        {
            get => Document;
            set => Document = value;
        }

        protected XmlDocumentSetting()
        {
        }

        public XmlDocumentSetting(XmlDocument doc)
        {
            Document = doc;
        }

        public XmlDocumentSetting(string xml)
        {
            StringValue = xml;
        }
    }
}
