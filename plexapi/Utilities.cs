using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace plexapi
{
    public static class Utilities
    {
        public static T DeserializeXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var stringReader = new StringReader(xml);
            using var xmlTextReader = new XmlTextReader(stringReader);
            return (T)serializer.Deserialize(xmlTextReader);
        }
    }
}
