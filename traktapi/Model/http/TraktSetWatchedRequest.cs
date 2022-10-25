using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktSetWatchedRequest
    {
        [JsonProperty("movies")]
        public List<TraktMovie> Movies { get; set; }
        [JsonProperty("shows")]
        public List<TraktShow> Shows { get; set; }
    }
}
