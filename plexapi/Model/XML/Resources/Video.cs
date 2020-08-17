using System.Xml.Serialization;

namespace plexapi.Model.XML.Resources
{
    [XmlRoot(ElementName = "Video")]
    public class Video
    {
        [XmlAttribute(AttributeName = "ratingKey")]
        public string RatingKey { get; set; }


        [XmlAttribute(AttributeName = "guid")]
        public string Guid { get; set; }


        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "viewCount")]
        public string ViewCount { get; set; }

        [XmlAttribute(AttributeName = "year")]
        public string Year { get; set; }


        //Episode
		
		[XmlAttribute(AttributeName = "grandparentTitle")]
		public string GrandparentTitle { get; set; }
		[XmlAttribute(AttributeName = "parentTitle")]
		public string ParentTitle { get; set; }
		[XmlAttribute(AttributeName = "index")]
		public string Index { get; set; }
		[XmlAttribute(AttributeName = "parentIndex")]
		public string ParentIndex { get; set; }
	}
}

