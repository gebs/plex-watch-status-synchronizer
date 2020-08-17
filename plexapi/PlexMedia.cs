using System.Net.Http;
using System.Threading.Tasks;

namespace plexapi
{
    public class PlexMedia
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string RatingKey { get; set; }
        public bool Watched { get; set; }
        public PlexMediaServer MediaServer { get; set; }


        public async Task<bool> SetWatched()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{MediaServer.BaseUrl}/:/scrobble?key={RatingKey}&identifier=com.plexapp.plugins.library&X-Plex-Token={MediaServer.Token}");
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
