using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktSeason
    {
        [JsonProperty("number")]
        public int Number { get; set; }
        [JsonProperty("episodes", NullValueHandling = NullValueHandling.Ignore)]
        public List<TraktEpisode> Episodes { get; set; }

        public TraktSeason()
        {

        }
        public TraktSeason(int season,int episode)
        {
            Number = season;
            Episodes = new List<TraktEpisode>
            {
                new TraktEpisode(episode)
            };

        }
    }
}
