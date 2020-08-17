using plexapi.Model.XML.Resources;
using System;

namespace plexapi
{
    public class PlexEpisode : PlexMedia
    {

        public int Season { get; set; }
        public int Episode { get; set; }
        public string Show { get; set; }


        public PlexEpisode(Video video, PlexMediaServer server)
        {
            this.Guid = video.Guid;
            this.Title = video.Title;
            this.Season = Convert.ToInt32(video?.ParentIndex ?? "0");
            this.Episode = Convert.ToInt32(video?.Index ?? "0");
            this.Show = video.GrandparentTitle ?? video.ParentTitle;
            this.RatingKey = video.RatingKey;
            this.Watched = !string.IsNullOrEmpty(video.ViewCount) && Convert.ToInt32(video.ViewCount) > 0;
            this.Title = $"{Show} - S{Season:00}E{Episode:00} - {video.Title}";
            this.MediaServer = server;
        }

        public PlexEpisode(int tvdbid, string title, int season, int episode, string show)
        {
            this.Guid = $"com.plexapp.agents.thetvdb://{tvdbid}/{season}/{episode}?lang=en";
            this.Season = season;
            this.Episode = episode;
            this.Show = show;
            this.Title = $"{Show} - S{Season:00}E{Episode:00} - {title}";
            this.Watched = true;
        }
    }
}
