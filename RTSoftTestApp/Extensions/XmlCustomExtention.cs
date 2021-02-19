using System;
using System.Xml;

namespace RTSoftTestApp.Extensions
{
    public static class XmlCustomExtention
    {
        public static Guid? ParseGuidFromXmlAttribute(this XmlNode node, string attrName, string prefix)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node.Attributes.Count == 0)
                return null;

            var attr = node.Attributes[attrName]?.InnerText;
            if (string.IsNullOrEmpty(attr))
                return null;

            return ParseGuid(attr, prefix);
        }

        public static Guid? ParseGuid(string text, string prefix)
        {
            text = text.Replace(prefix, string.Empty);
            if (!Guid.TryParse(text, out Guid result))
                return null;

            return result;
        }
    }
}
