using System;
using System.Collections.Generic;
using System.Text;

namespace traktapi.Model
{
    public class TraktEpisode
    {
        public int Season { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public TraktIds Ids { get; set; }
    }
}
