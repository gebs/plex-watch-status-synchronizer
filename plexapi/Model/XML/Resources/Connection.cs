using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace plexapi.Model.XML.Resources
{
    [XmlRoot(ElementName = "Connection")]
    public class Connection
    {
		[XmlAttribute(AttributeName = "protocol")]
		public string Protocol { get; set; }

		[XmlAttribute(AttributeName = "address")]
		public string Address { get; set; }

		[XmlAttribute(AttributeName = "port")]
		public int Port { get; set; }

		[XmlAttribute(AttributeName = "uri")]
		public string Uri { get; set; }

		[XmlAttribute(AttributeName = "local")]
		public bool Local { get; set; }
	}
}
