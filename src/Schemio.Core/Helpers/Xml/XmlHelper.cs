using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Schemio.Core.Helpers.Xml
{
    public static class XmlHelper
    {
        public static string SerializeToXml<T>(T value, XmlSerializerNamespaces namespaces, XmlWriterSettings settings)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null");

            if (settings == null)
                throw new ArgumentNullException(nameof(settings), "Settings cannot be null");

            var xmlStr = new StringBuilder();
            var x = new XmlSerializer(typeof(T));

            using (var writer = XmlWriter.Create(xmlStr, settings))
            {
                x.Serialize(writer, value, namespaces);
                return xmlStr.ToString();
            }
        }
    }
}