using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace plexapi.Model.XML.Resources
{
	[XmlRoot(ElementName = "Directory")]
	public class Directory
	{
		[XmlAttribute(AttributeName = "key")]
		public int Key { get; set; }
		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
		[XmlAttribute(AttributeName = "title")]
		public string Name { get; set; }

	}

}
