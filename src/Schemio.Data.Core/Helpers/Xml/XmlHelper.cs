using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Schemio.Data.Core.Helpers.Xml
{
    public static class XmlHelper
    {
        public static string SerializeToXml<T>(T value, XmlSerializerNamespaces namespaces, XmlWriterSettings settings)
        {
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