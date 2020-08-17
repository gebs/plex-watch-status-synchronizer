namespace traktapi.Model
{
    public class TraktMedia
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string Type { get; set; }
        public TraktMovie Movie { get; set; }
        public TraktEpisode Episode { get; set; }
        public TraktShow Show { get; set; }
    }



}
