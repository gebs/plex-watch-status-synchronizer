using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace plexapi.Model.XML.Resources
{
    [XmlRoot(ElementName = "Device")]
    public class Device
    {
		[XmlElement(ElementName = "Connection")]
		public List<Connection> Connection { get; set; }

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "accessToken")]
		public string AccessToken { get; set; }

		[XmlAttribute(AttributeName = "publicAddress")]
		public string PublicAddress { get; set; }

	}
}
