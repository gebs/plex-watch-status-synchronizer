using Newtonsoft.Json;

namespace traktapi.Model
{
    public class TraktMovie
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }
        [JsonProperty("ids", NullValueHandling = NullValueHandling.Ignore)]
        public TraktIds Ids { get; set; }
    }
}
