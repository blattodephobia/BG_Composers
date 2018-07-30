using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BGC.Core.Services
{
    public static class ArticleContentServiceExtensions
    {
        private static readonly string PseudoRootTag = "PSEUDO_ROOT";

        private static void RemoveImagesAndScripts(XmlNode node)
        {
            Queue<XmlNode> sanitizeQueue = new Queue<XmlNode>();
            sanitizeQueue.Enqueue(node);
            while (sanitizeQueue.Count > 0)
            {
                XmlNode current = sanitizeQueue.Dequeue();
                foreach (XmlNode child in current.ChildNodes)
                {
                    if (child.Name == "img" || child.Name == "script")
                    {
                        current.RemoveChild(child);
                    }
                    else
                    {
                        sanitizeQueue.Enqueue(child);
                    }
                }
            }
        }

        private static XmlDocument LoadXml(string xml)
        {
            XmlDocument result = null;
            try
            {
                result = new XmlDocument();
                result.LoadXml(xml);
                return result;
            }
            catch (XmlException) // attempt to handle the multiple root elements corner case
            {
                result = new XmlDocument();
                result.LoadXml($"<{PseudoRootTag}>{xml}</{PseudoRootTag}>");
                return result;
            }
        }

        /// <summary>
        /// Returns a portion of an article's contents.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="articleId"></param>
        /// <returns>An <see cref="XmlNode"/> containing the XML or plain text of the preview,
        /// or null, if the article's content is empty.</returns>
        public static XmlNode GeneratePreview(this IArticleContentService service, Guid articleId)
        {
            Shield.ArgumentNotNull(service).ThrowOnError();

            string articleXml = service.GetEntry(articleId);
            if (string.IsNullOrWhiteSpace(articleXml))
            {
                return null;
            }

            XmlDocument doc = LoadXml(articleXml);
            RemoveImagesAndScripts(doc.FirstChild);

            return doc.FirstChild;
        }
    }
}
