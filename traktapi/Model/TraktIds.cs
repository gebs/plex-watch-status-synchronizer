using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktIds
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Trakt { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("imdb", NullValueHandling = NullValueHandling.Ignore)]
        public string Imdb { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Tmdb { get; set; }

        [JsonProperty("tvdb", NullValueHandling = NullValueHandling.Ignore)]
        public string Tvdb { get; set; }
    }
}
