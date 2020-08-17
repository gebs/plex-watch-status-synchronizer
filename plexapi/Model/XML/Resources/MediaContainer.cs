using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace plexapi.Model.XML.Resources
{
    [XmlRoot(ElementName = "MediaContainer")]
    public class MediaContainer
    {
        [XmlElement(ElementName = "Device")]
        public List<Device> Device { get; set; }

		[XmlElement(ElementName = "Video")]
		public List<Video> Video { get; set; }

		[XmlElement(ElementName = "Directory")]
		public List<Directory> Directory { get; set; }
		
	}
}
