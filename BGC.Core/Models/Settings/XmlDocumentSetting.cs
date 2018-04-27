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

            doc.NodeChanged += OnDocumentNodeChanged;
            doc.NodeInserted += OnDocumentNodeChanged;
            doc.NodeRemoved += OnDocumentNodeChanged;
        }

        private void UnsubscribeEvents(XmlDocument doc)
        {
            if (doc == null) return;

            doc.NodeChanged -= OnDocumentNodeChanged;
            doc.NodeInserted -= OnDocumentNodeChanged;
            doc.NodeRemoved -= OnDocumentNodeChanged;
        }

        protected virtual XmlDocument LoadXml(string xml)
        {
            XmlDocument result = new XmlDocument();
            result.LoadXml(xml);

            return result;
        }

        public override string StringValue
        {
            get
            {
                return base.StringValue;
            }

            set
            {
                UnsubscribeEvents(_document);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    _document = LoadXml(value);
                }
                else
                {
                    _document = null;
                }

                base.StringValue = value;

                SubscribeEvents(_document);
            }
        }

        protected virtual void OnDocumentNodeChanged(object sender, XmlNodeChangedEventArgs e)
        {
            base.StringValue = (sender as XmlDocument).InnerXml;
        }

        public override Type ValueType => typeof(XmlDocument);

        private XmlDocument _document;
        [NotMapped]
        public virtual XmlDocument Document
        {
            get
            {
                return _document;
            }

            set
            {
                UnsubscribeEvents(_document);

                _document = value;
                base.StringValue = _document?.InnerXml;

                SubscribeEvents(_document);
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

        public XmlDocumentSetting(string name) :
            base(name)
        {

        }

        public XmlDocumentSetting(string name, XmlDocument doc) :
            base(name)
        {
            Document = doc;
        }

        public XmlDocumentSetting(string name, string xml) :
            base(name)
        {
            StringValue = xml;
        }
    }
}
