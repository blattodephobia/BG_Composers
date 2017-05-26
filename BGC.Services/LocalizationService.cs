using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;

namespace BGC.Services
{
    public class LocalizationService : ILocalizationService
    {
        private static readonly string LocalizationElementName = "LocalizedString";
        private static readonly string TranslationElementName = "Translation";
        private static readonly string KeyAttributeName = "key";
        private static readonly string CultureAttributeName = "culture";
        private static readonly IReadOnlyDictionary<CultureInfo, char[]> Alphabets = new Dictionary<CultureInfo, char[]>()
        {
            { CultureInfo.GetCultureInfo("bg-BG"), "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЬЮЯ".ToCharArray() },
            { CultureInfo.GetCultureInfo("en-US"), "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray() },
            { CultureInfo.GetCultureInfo("de-DE"), "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray() },
        };

        protected LocalizationService()
        {
            this.localizationCache = new Dictionary<string, string>();
            this.DefaultCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        public LocalizationService(XmlDocument localizationXml) :
            this()
        {
            Shield.ArgumentNotNull(localizationXml, nameof(localizationXml)).ThrowOnError();

            XmlNode root = localizationXml.DocumentElement;
            Queue<XmlNode> bfs = new Queue<XmlNode>();
            bfs.Enqueue(root);

            while (bfs.Any())
            {
                XmlNode current = bfs.Dequeue();
                if (current.Name == LocalizationElementName)
                {
                    string key = $"{this.GetNodePath(current.ParentNode)}.{current.Attributes[KeyAttributeName].Value}";
                    for (int i = 0; i < current.ChildNodes.Count; i++)
                    {
                        XmlNode translation = current.ChildNodes[i];
                        if (translation.Name == TranslationElementName)
                        {
                            CultureInfo culture = CultureInfo.GetCultureInfo(translation.Attributes[CultureAttributeName].Value);
                            this.localizationCache.Add(this.GetNormalizedKey(key, culture), translation.InnerText);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < current.ChildNodes.Count; i++)
                    {
                        bfs.Enqueue(current.ChildNodes[i]);
                    }
                }
            }
        }

        private CultureInfo culture;
        public CultureInfo DefaultCulture
        {
            get
            {
                return this.culture;
            }

            set
            {
                this.culture = value.ValueNotNull()
                    .And(value.Assert(value != CultureInfo.InvariantCulture, x => new InvalidOperationException("Invariant culture is not supported for localization"))).GetValueOrThrow();
            }
        }

        public string Localize(string key, CultureInfo culture = null)
        {
            Shield.ArgumentNotNull(key, nameof(key)).ThrowOnError();

            string result;
            key = this.GetNormalizedKey(key, culture ?? DefaultCulture);
            this.localizationCache.TryGetValue(key, out result);
            return result ?? key;
        }

        public char[] GetAlphabet(bool useUpperCase = true, CultureInfo culture = null)
        {
            culture = culture ?? DefaultCulture;

            Shield.AssertOperation(culture, Alphabets.ContainsKey(culture), $"The current culture, {culture}, is not supported for this operation.").ThrowOnError();
            
            char[] result = Alphabets[culture].Select(c => useUpperCase ? c : char.ToLower(c, culture)).ToArray(); // a copy is necessary, because arrays are not read-only and can still be modified
            return result;
        }

        private string GetNormalizedKey(string key, CultureInfo culture)
        {
            return $"[{culture.Name}]+{key}".ToLowerInvariant(); // using InvariantCulture for case conversion is still OK, even though localizing strings for it is not.
        }

        private string GetNodePath(XmlNode node)
        {
            Stack<string> parentChain = new Stack<string>();
            while (node != null && node != node.OwnerDocument.DocumentElement)
            {
                parentChain.Push(node.Name);
                node = node.ParentNode;
            }

            StringBuilder result = parentChain.Aggregate(
                new StringBuilder(),
                (sb, current) => sb.Append($"{current}."),
                sb => sb.Length > 0 ? sb.Remove(sb.Length - 1, 1) : sb);
            return result.ToString();
        }

        private Dictionary<string, string> localizationCache;
    }
}
