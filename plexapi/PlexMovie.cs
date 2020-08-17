using plexapi.Model.XML.Resources;
using System;

namespace plexapi
{
    public class PlexMovie : PlexMedia
    {
        public int Year { get; set; }
        public PlexMovie(Video video, PlexMediaServer server)
        {
            this.MediaServer = server;
            this.Guid = video.Guid;
            this.Title = video.Title;
            this.Year = Convert.ToInt32(video.Year);
            this.RatingKey = video.RatingKey;
            this.Watched = !string.IsNullOrEmpty(video.ViewCount) && Convert.ToInt32(video.ViewCount) > 0;
        }

        public PlexMovie(string imdbId,string title)
        {
            this.Guid = $"com.plexapp.agents.imdb://{imdbId}?lang=en";
            this.Title = title;
            this.Watched = true;
        }
    }
}
