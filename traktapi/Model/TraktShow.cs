using Newtonsoft.Json;
using System.Collections.Generic;

namespace traktapi.Model
{
    public class TraktShow
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("ids", NullValueHandling = NullValueHandling.Ignore)]
        public TraktIds Ids { get; set; }
        [JsonProperty("seasons", NullValueHandling = NullValueHandling.Ignore)]
        public List<TraktSeason> Seasons { get; set; }

        public TraktShow()
        {

        }
        public TraktShow(string title, string tvdbId, int season, int episode)
        {
            Title = title;
            Ids = new TraktIds() { Tvdb = tvdbId };
            Seasons = new List<TraktSeason>()
            {
                new TraktSeason(season,episode)
            };



        }
    }
}
