using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktEpisode
    {
        [JsonProperty("season", NullValueHandling = NullValueHandling.Ignore)]
        public int? Season { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("title",NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }


        public TraktEpisode()
        {

        }
        public TraktEpisode(int number)
        {
            Number = number;
        }
    }
}
